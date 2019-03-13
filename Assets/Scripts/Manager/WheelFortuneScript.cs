using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

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
        CheckVideoTime();
    }
    private void CheckVideoTime()//این تابع در ابتدا مقادیر را داخل تکست باکس ها ست می کند و سپس با توجه به زمان فعلی و اینکه تعداد شانس ها کمتر از 3 باشد زمان شانس بعدی را می سنجد تا اضافه شود
    {
        CheckLblFree();
        Debug.Log("CheckVideoTime >" + ObscuredPrefs.GetInt("VideoWheel", 3));
        txtNumVideoWheel.text = ObscuredPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
        string[] arr = ObscuredPrefs.GetString("TimeVideoWheel", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        if (ObscuredPrefs.GetInt("VideoWheel", 3) < 3)
        {

            //Debug.Log("VideoWheel< 3 ");
            StartCoroutine(GetDateTime.IEGetDateTime((status) =>
            {
                //Debug.Log("status" + status + " WHeelTIME: " + wheelTime);
                TimeSpan remain = wheelTime.Subtract(status);
                //Debug.Log("remain: " + remain.ToString());
                double reamainSec = remain.TotalSeconds;
                //Debug.Log("reamainSec >" + reamainSec);
                if (wheelTime <= status)
                {
                    Debug.Log("wheelTime <= status");
                    while ((-reamainSec) - 28800 >= 0 && ObscuredPrefs.GetInt("VideoWheel", 3) <= 3)
                    {
                        //Debug.Log("Up +1");
                        ObscuredPrefs.SetInt("VideoWheel", ObscuredPrefs.GetInt("VideoWheel", 3) + 1);
                        txtNumVideoWheel.text = ObscuredPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
                        objTimeVideo.SetActive(false);
                        reamainSec += 28800;
                        //Debug.Log("Wheel: " + wheelTime);
                        wheelTime = wheelTime.AddHours(8);
                        //Debug.Log("Wheel: " + wheelTime);
                        //Debug.Log("Old Wheel Time : " + ObscuredPrefs.GetString("TimeVideoWheel", "1992,11,30,00,00,00"));
                        ObscuredPrefs.SetString("TimeVideoWheel", wheelTime.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + wheelTime.Day.ToString() + "," + wheelTime.Hour.ToString() + "," + wheelTime.Minute.ToString() + "," + wheelTime.Second.ToString());
                        //Debug.Log("New Wheel Time : " + ObscuredPrefs.GetString("TimeVideoWheel", "1992,11,30,00,00,00"));
                    }
                    if (ObscuredPrefs.GetInt("VideoWheel", 3) <= 3)
                    {
                        CheckVideoTime();
                    }
                }
                else
                {
                    if (lastRoutine != null)
                    {
                        StopCoroutine(lastRoutine);
                    }
                    lastRoutine = StartCoroutine(IETimerVideoWheel(reamainSec));
                }
            }));
        }
        else
        {
            objTimeVideo.SetActive(false);
        }
    }
    public void GiftWheelWithVideo()//وقتی که یک بار از ویدئو استفاده کرد برای چرخاندن گردونه شانس باید این تابع فراخوانی شود
    {
        Debug.Log("GiftWheelWithVideo");
        ObscuredPrefs.SetInt("VideoWheel", ObscuredPrefs.GetInt("VideoWheel", 3) - 1);
        CheckLblFree();
        Debug.Log("GiftWheelWithVideo" + ObscuredPrefs.GetInt("VideoWheel", 3));
        if (ObscuredPrefs.GetString("TimeVideoWheel", "1992,11,30,00,00,00") == "1992,11,30,00,00,00")
        {
            TimeSpan nowTimeSpan = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan plusTimeSpan = new TimeSpan(0, 8, 0, 0);
            //TimeSpan plusTimeSpan = new TimeSpan(0, 0, 2, 0);
            TimeSpan result = plusTimeSpan + nowTimeSpan;
            ObscuredPrefs.SetString("TimeVideoWheel", DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + result.Days.ToString() + "," + result.Hours.ToString() + "," + result.Minutes.ToString() + "," + result.Seconds.ToString());
            Debug.Log("NOT SET >>" + ObscuredPrefs.GetString("TimeVideoWheel"));
        }
        CheckVideoTime();
        WheelStart(true);
    }
    IEnumerator IETimerVideoWheel(double deltaTime)//تابع تایمر ویدئو
    {
        Debug.Log("Start Remain  Video :" + deltaTime);
        objTimeVideo.SetActive(true);
        for (; deltaTime > 0; deltaTime -= 1f)
        {
            objTimeVideo.SetActive(true);
            int s = (int)(deltaTime % 60);
            int m = (int)((deltaTime % 3600) / 60);
            int h = (int)(deltaTime / 3600);
            txtTimeRemain.text = "" + h.ToString("D1") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            //Debug.Log("txtTimeRemain: " + txtTimeRemain.text);
            if (m == 0 && s == 1)
            {
                txtTimeRemain.text = "0:00:00";
            }
            yield return new WaitForSeconds(1f);
        }
        if (deltaTime == 0)
        {
            txtTimeRemain.text = "0:00:00";
            objTimeVideo.SetActive(false);
            if (ObscuredPrefs.GetInt("VideoWheel", 3) < 3)
            {
                ObscuredPrefs.SetInt("VideoWheel", ObscuredPrefs.GetInt("VideoWheel", 1) + 1);
            }
            CheckVideoTime();
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
        videoAds.controller.achivmentManager.OpenPanel();
        btnWheelVideo.interactable = false;
        btnWheelGem.interactable = false;
        anglePerItem = 360 / maxRotaiton.Length;
        txtNumVideoWheel.text = ObscuredPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
        randomTime = UnityEngine.Random.Range(3, 6);
        int iPercent = UnityEngine.Random.Range(0, 100);
        Debug.Log("darsad>>" + iPercent);
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
        Debug.Log("itemnum>>" + itemNumber);
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
}
