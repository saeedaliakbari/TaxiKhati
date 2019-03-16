using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;

public class WheelFortuneScript : MonoBehaviour
{
    public VideoAds videoAds;
    public List<AnimationCurve> animationCurves;
    private bool spinning;
    private float anglePerItem, chornoTimeCounter;
    public float[] maxRotaiton;
    private int randomTime;
    private int itemNumber;

    public Text txtNumVideoWheel, txtTimeRemain;
    public Button btnWheelVideo, btnWheelGem;
    public GameObject objTimeVideo, lblFree;
    public GameObject goWheel;
    public AudioSource audioWheel, audioGift;
    private string[] timeWheel = { "timeWheel1", "timeWheel2", "timeWheel3" };
    private Coroutine lastRoutine = null;
    void Start()
    {
        checkTimeWheel();
    }
    IEnumerator IETimerVideoWheel(double deltaTime)//تابع تایمر ویدئو
    {
        objTimeVideo.SetActive(true);
        for (; deltaTime > 0; deltaTime -= 1f)
        {
            objTimeVideo.SetActive(true);
            int s = (int)(deltaTime % 60);
            int m = (int)((deltaTime % 3600) / 60);
            int h = (int)(deltaTime / 3600);
            txtTimeRemain.text = "" + h.ToString("D1") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            if (m == 0 && s == 1)
            {
                txtTimeRemain.text = "0:00:00";
            }
            yield return new WaitForSeconds(1f);
        }
        if (deltaTime <= 0)
        {
            txtTimeRemain.text = "0:00:00";
            objTimeVideo.SetActive(false);
            checkTimeWheel();
        }
        yield return new WaitForSeconds(0f);
    }
    #region Wheel
    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        audioWheel.Play();
        spinning = true;
        float timer = 0.0f;
        int animationCurveNumber = UnityEngine.Random.Range(0, animationCurves.Count);
        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            goWheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
            timer += Time.deltaTime;
            yield return 0;
        }
        audioWheel.Stop();
        goWheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle);
        btnWheelGem.interactable = true;
        btnWheelVideo.interactable = true;
        spinning = false;
        ManageGift(itemNumber);
    }
    public void WheelStart(bool video)
    {
        ObscuredPrefs.SetInt("mainAchiv9", ObscuredPrefs.GetInt("mainAchiv9", 0) + 1);
        videoAds.controller.achivmentManager.CheckAchivments();
        btnWheelVideo.interactable = false;
        btnWheelGem.interactable = false;
        anglePerItem = 360 / maxRotaiton.Length;
        txtNumVideoWheel.text = ObscuredPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
        randomTime = UnityEngine.Random.Range(3, 6);
        int iPercent = UnityEngine.Random.Range(0, 100);
        //Debug.Log("darsad>>" + iPercent);
        if (video)
        {
            if (iPercent < 30)//5x Earning for 1m
            {
                itemNumber = 0;
            }
            else if (iPercent < 70)//2x Speed For 150s
            {
                itemNumber = 1;
            }
            else if (iPercent < 90)//4 Golden Box
            {
                itemNumber = 2;
            }
            else if (iPercent < 95)//4h time boost
            {
                itemNumber = 3;
            }
            else if (iPercent < 100)//20 Gem
            {
                itemNumber = 4;
            }
        }
        else {
            if (iPercent < 30)//5x Earning for 1m
            {
                itemNumber = 0;
            }
            else if (iPercent < 50)//2x Speed For 150s
            {
                itemNumber = 1;
            }
            else if (iPercent < 70)//4 Golden Box
            {
                itemNumber = 2;
            }
            else if (iPercent < 80)//4h time boost
            {
                itemNumber = 3;
            }
            else if (iPercent < 100)//20 Gem
            {
                itemNumber = 4;
            }
        }
        //Debug.Log("itemnum>>" + itemNumber);
        float maxAngle = 360 * randomTime + maxRotaiton[itemNumber];
        StartCoroutine(SpinTheWheel(randomTime, maxAngle));
    }
    #endregion//باید کدهاش بررسی شود
    public void BtnWheelWithGem()
    {
        if (ObscuredPrefs.GetDouble("gem", 0) >= 5)
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") - 5);
            videoAds.controller.SetText();
            WheelStart(false);
        }
        else
        {
            videoAds.controller.panelNoGem.SetActive(true);
        }
    }
    private void ManageGift(int itemNumber)
    {
        audioGift.Play();
        //itemNumber = 2;
        videoAds.controller.panelMessage.SetActive(true);
        if (itemNumber == 0)
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") + 20);
            videoAds.controller.SetText();
            videoAds.controller.txtPanelMessage.text = "20 الماس اضافه شد";
        }
        else if (itemNumber == 1)
        {
            if (Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m"))
            {
                Manager.SetActionTime("5x_earning_for_1m", (Manager.GetActionTime("5x_earning_for_1m") + 60 + Manager.GetCurrentTime()));
            }
            else {
                Manager.SetActionTime("5x_earning_for_1m", (60 + Manager.GetCurrentTime()));
            }
            videoAds.controller.txtPanelMessage.text = "به مدت 1 دقیقه در آمد شما 5 برابر شد";
            videoAds.controller.slotManager.UpdateEarningSpeedText();
            StartCoroutine(videoAds.controller.IEEarningRatio());
        }
        else if (itemNumber == 2)
        {
            if (Manager.GetCurrentTime() < Manager.GetActionTime("2x_speed_for_150s"))
            {
                Manager.SetActionTime("2x_speed_for_150s", (150 + Manager.GetActionTime("2x_speed_for_150s") + Manager.GetCurrentTime()));
            }
            else {
                Manager.SetActionTime("2x_speed_for_150s", (150 + Manager.GetCurrentTime()));
            }
            videoAds.controller.txtPanelMessage.text = "به مدت 150 ثانیه سرعت شما 2 برابر شد";
            videoAds.controller.slotManager.UpdateEarningSpeedText();
            StartCoroutine(videoAds.controller.IESpeedRatio());
        }
        else if (itemNumber == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                videoAds.controller.SpawnABoxWheel();
            }
            videoAds.controller.txtPanelMessage.text = "4 جعبه طلایی به پارکینگ شما اضافه شد";
        }
        else if (itemNumber == 4)
        {
            ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + (videoAds.controller.slotManager.earnPerSec * 4 * 60 * 60));
            ObscuredPrefs.SetDouble("coinTotal", ObscuredPrefs.GetDouble("coinTotal", 0) + (videoAds.controller.slotManager.earnPerSec * 4 * 60 * 60));
            videoAds.controller.SetText();
            videoAds.controller.txtPanelMessage.text = "به اندازه 4 ساعت درآمد فعلی  به شما پرداخت شد";
        }
    }
    public void CheckLblFree()
    {
        if (ObscuredPrefs.GetInt("VideoWheel", 3) > 0)
        {
            lblFree.SetActive(true);
        }
        else
        {
            lblFree.SetActive(false);
        }
    }
    public void checkTimeWheel()
    {
        string[] arr = ObscuredPrefs.GetString("TimeVideoWheel1", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime1 = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        arr = ObscuredPrefs.GetString("TimeVideoWheel2", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime2 = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        arr = ObscuredPrefs.GetString("TimeVideoWheel3", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime3 = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        if (ObscuredPrefs.GetInt("VideoWheel", 3) < 3)//اگر تعداد شانس های گردونه شانس کمتر از 3 تا بود
        {
            StartCoroutine(GetDateTime.IEGetDateTime((status) =>
            {
                TimeSpan remain1 = wheelTime1.Subtract(status);
                TimeSpan remain2 = wheelTime2.Subtract(status);
                TimeSpan remain3 = wheelTime3.Subtract(status);
                double[] remainSec = new double[3];
                remainSec[0] = remain1.TotalSeconds;
                remainSec[1] = remain2.TotalSeconds;
                remainSec[2] = remain3.TotalSeconds;
                if (remainSec[0] >= 0)
                {
                    if (remainSec[1] >= 0)
                    {
                        if (remainSec[2] >= 0)
                        {
                            if (lastRoutine != null)
                            {
                                StopCoroutine(lastRoutine);
                            }
                            lastRoutine = StartCoroutine(IETimerVideoWheel(Min(remainSec)));
                            ObscuredPrefs.SetInt("VideoWheel", 0);
                        }
                        else
                        {
                            Debug.Log(Math.Min(remainSec[0], remainSec[1]));
                            if (lastRoutine != null)
                            {
                                StopCoroutine(lastRoutine);
                            }
                            lastRoutine = StartCoroutine(IETimerVideoWheel(Math.Min(remainSec[0], remainSec[1])));
                            ObscuredPrefs.SetInt("VideoWheel", 1);
                        }
                    }
                    else
                    {
                        if (remainSec[2] >= 0)
                        {
                            if (lastRoutine != null)
                            {
                                StopCoroutine(lastRoutine);
                            }
                            lastRoutine = StartCoroutine(IETimerVideoWheel(Math.Min(remainSec[0], remainSec[2])));
                            ObscuredPrefs.SetInt("VideoWheel", 1);
                        }
                        else
                        {
                            if (lastRoutine != null)
                            {
                                StopCoroutine(lastRoutine);
                            }
                            lastRoutine = StartCoroutine(IETimerVideoWheel(remainSec[0]));
                            ObscuredPrefs.SetInt("VideoWheel", 2);
                        }
                    }
                }
                else
                {
                    if (remainSec[1] >= 0)
                    {
                        if (remainSec[2] >= 0)
                        {
                            if (lastRoutine != null)
                            {
                                StopCoroutine(lastRoutine);
                            }
                            lastRoutine = StartCoroutine(IETimerVideoWheel(Math.Min(remainSec[1], remainSec[2])));
                            ObscuredPrefs.SetInt("VideoWheel", 1);
                        }
                        else
                        {
                            if (lastRoutine != null)
                            {
                                StopCoroutine(lastRoutine);
                            }
                            lastRoutine = StartCoroutine(IETimerVideoWheel(remainSec[1]));
                            ObscuredPrefs.SetInt("VideoWheel", 2);
                        }
                    }
                    else
                    {
                        if (remainSec[2] >= 0)
                        {
                            if (lastRoutine != null)
                            {
                                StopCoroutine(lastRoutine);
                            }
                            lastRoutine = StartCoroutine(IETimerVideoWheel(remainSec[2]));
                            ObscuredPrefs.SetInt("VideoWheel", 2);
                        }
                        else
                        {
                            ObscuredPrefs.SetInt("VideoWheel", 3);
                            btnWheelVideo.interactable = true;
                            objTimeVideo.SetActive(false);
                        }
                    }
                }
                txtNumVideoWheel.text = ObscuredPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
            }));
        }
        else//اگر تعداد شانس های گردونه شانس کامل بود
        {
            btnWheelVideo.interactable = true;
            objTimeVideo.SetActive(false);
        }
    }
    public void giftWheelVideo()
    {
        ObscuredPrefs.SetInt("VideoWheel", ObscuredPrefs.GetInt("VideoWheel", 3) - 1);
        CheckLblFree();
        WheelStart(true);
        string[] arr = ObscuredPrefs.GetString("TimeVideoWheel1", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime1 = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        arr = ObscuredPrefs.GetString("TimeVideoWheel2", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime2 = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        arr = ObscuredPrefs.GetString("TimeVideoWheel3", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime3 = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        StartCoroutine(GetDateTime.IEGetDateTime((status) =>
        {
            TimeSpan remain1 = wheelTime1.Subtract(status);
            TimeSpan remain2 = wheelTime2.Subtract(status);
            TimeSpan remain3 = wheelTime3.Subtract(status);
            double[] remainSec = new double[3];
            remainSec[0] = remain1.TotalSeconds;
            remainSec[1] = remain2.TotalSeconds;
            remainSec[2] = remain3.TotalSeconds;
            TimeSpan max = TimeSpan.FromSeconds(Max(remainSec));
            TimeSpan min = TimeSpan.FromSeconds(Min(remainSec));
            DateTime newwheeltime = new DateTime();
            if (max == remain1)
            {
                if (max.TotalSeconds > 0)
                {
                    newwheeltime = wheelTime1.AddHours(8);
                }
                else
                {
                    newwheeltime = status.AddHours(8);
                }

            }
            else if (max == remain2)
            {
                if (max.TotalSeconds > 0)
                {
                    newwheeltime = wheelTime2.AddHours(8);
                }
                else
                {
                    newwheeltime = status.AddHours(8);
                }
            }
            else if (max == remain3)
            {
                if (max.TotalSeconds > 0)
                {
                    newwheeltime = wheelTime3.AddHours(8);
                }
                else
                {
                    newwheeltime = status.AddHours(8);
                }
            }
            if (min == remain1)
            {
                ObscuredPrefs.SetString("TimeVideoWheel1", newwheeltime.Year.ToString() + "," + newwheeltime.Month.ToString() + "," + newwheeltime.Day.ToString() + "," + newwheeltime.Hour.ToString() + "," + newwheeltime.Minute.ToString() + "," + newwheeltime.Second.ToString());
            }
            else if (min == remain2)
            {
                ObscuredPrefs.SetString("TimeVideoWheel2", newwheeltime.Year.ToString() + "," + newwheeltime.Month.ToString() + "," + newwheeltime.Day.ToString() + "," + newwheeltime.Hour.ToString() + "," + newwheeltime.Minute.ToString() + "," + newwheeltime.Second.ToString());
            }
            else if (min == remain3)
            {
                ObscuredPrefs.SetString("TimeVideoWheel3", newwheeltime.Year.ToString() + "," + newwheeltime.Month.ToString() + "," + newwheeltime.Day.ToString() + "," + newwheeltime.Hour.ToString() + "," + newwheeltime.Minute.ToString() + "," + newwheeltime.Second.ToString());
            }
            checkTimeWheel();
        }));

    }
    public void TestVideo()
    {
        if (ObscuredPrefs.GetInt("VideoWheel", 3) > 0)
        {
            giftWheelVideo();
        }
        else
        {
            videoAds.controller.txtPanelMessage.text = "فرصت ویدئویي رایگان وجود ندارد";
            videoAds.controller.panelMessage.SetActive(true);
            //Debug.Log("Bayad Ta Zaman Baz Shodan Video Sabr Konid");
        }
    }
    public double Max(params double[] values)
    {
        return Enumerable.Max(values);
    }
    public double Min(params double[] values)
    {
        return Enumerable.Min(values);
    }
}
