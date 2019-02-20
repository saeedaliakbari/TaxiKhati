using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
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
    public GameObject objTimeVideo;
    public GameObject goWheel;
    private string[] timeWheel = { "timeWheel1", "timeWheel2", "timeWheel3" };
    void Start()
    {
        CheckVideoTime();
    }
    private void CheckVideoTime()//این تابع در ابتدا مقادیر را داخل تکست باکس ها ست می کند و سپس با توجه به زمان فعلی و اینکه تعداد شانس ها کمتر از 3 باشد زمان شانس بعدی را می سنجد تا اضافه شود
    {
        //Debug.Log("CheckVideoTime >" + PlayerPrefs.GetInt("VideoWheel", 3));
        txtNumVideoWheel.text = PlayerPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
        string[] arr = PlayerPrefs.GetString("TimeVideoWheel", "1992,11,30,00,00,00").Split(',');
        DateTime wheelTime = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
        if (PlayerPrefs.GetInt("VideoWheel", 3) < 3)
        {
            Debug.Log("VideoWheel< 3 ");
            StartCoroutine(GetDateTime.IEGetDateTime((status) =>
            {
                Debug.Log("status" + status + " WHeelTIME: " + wheelTime);
                TimeSpan remainTime = new TimeSpan(wheelTime.Day - status.Day, wheelTime.Hour - status.Hour, wheelTime.Minute - status.Minute, wheelTime.Second - status.Second);
                float reamainSec = remainTime.Days * 3600 * 24 + remainTime.Hours * 3600 + remainTime.Minutes * 60 + remainTime.Seconds;
                Debug.Log("reamainSec >" + (-reamainSec));
                if (wheelTime <= status)
                {
                    Debug.Log("wheelTime <= status");
                    while ((-reamainSec) - 28800 >= 0 && PlayerPrefs.GetInt("VideoWheel", 3) < 3)
                    {
                        Debug.Log("Up +1");
                        PlayerPrefs.SetInt("VideoWheel", PlayerPrefs.GetInt("VideoWheel", 3) + 1);
                        txtNumVideoWheel.text = PlayerPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
                        reamainSec += 28800f;
                        TimeSpan nowTimeSpan = new TimeSpan(status.Day, status.Hour, status.Minute, status.Second);
                        TimeSpan plusTimeSpan = new TimeSpan(0, 8, 0, 0);
                        TimeSpan result = plusTimeSpan + nowTimeSpan;
                        PlayerPrefs.SetString("TimeVideoWheel", DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + result.Days.ToString() + "," + result.Hours.ToString() + "," + result.Minutes.ToString() + "," + result.Seconds.ToString());
                    }
                    if (PlayerPrefs.GetInt("VideoWheel", 3) < 3)
                    {
                        Debug.Log("wheelTime <= status AND VideoWheel< 3");
                        TimeSpan nowTimeSpan = new TimeSpan(status.Day, status.Hour, status.Minute, status.Second);
                        TimeSpan plusTimeSpan = new TimeSpan(0, 8, 0, 0);
                        TimeSpan result = plusTimeSpan + nowTimeSpan;
                        PlayerPrefs.SetString("TimeVideoWheel", DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + result.Days.ToString() + "," + result.Hours.ToString() + "," + result.Minutes.ToString() + "," + result.Seconds.ToString());
                        StartCoroutine(IETimerVideoWheel(28800));//زمان 8 ساعت را می شمارد
                    }
                }
                else
                {
                    StartCoroutine(IETimerVideoWheel(reamainSec));
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
        PlayerPrefs.SetInt("VideoWheel", PlayerPrefs.GetInt("VideoWheel", 3) - 1);
        Debug.Log("GiftWheelWithVideo" + PlayerPrefs.GetInt("VideoWheel", 3));
        if (PlayerPrefs.GetString("TimeVideoWheel", "NotSet") == "NotSet")
        {
            TimeSpan nowTimeSpan = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan plusTimeSpan = new TimeSpan(0, 8, 0, 0);
            TimeSpan result = plusTimeSpan + nowTimeSpan;
            PlayerPrefs.SetString("TimeVideoWheel", DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + result.Days.ToString() + "," + result.Hours.ToString() + "," + result.Minutes.ToString() + "," + result.Seconds.ToString());
            Debug.Log("NOT SET >>" + PlayerPrefs.GetString("TimeVideoWheel"));
        }
        CheckVideoTime();
        WheelStart(true);
    }
    IEnumerator IETimerVideoWheel(float deltaTime)//تابع تایمر ویدئو
    {
        Debug.Log("Start Remain Time :" + deltaTime);
        for (; deltaTime > 0; deltaTime -= 1f)
        {
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
            if (PlayerPrefs.GetInt("VideoWheel", 3) < 3)
            {
                PlayerPrefs.SetInt("VideoWheel", PlayerPrefs.GetInt("VideoWheel", 1) + 1);
            }
            CheckVideoTime();
        }
        yield return new WaitForSeconds(0f);
    }
    #region Wheel
    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
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
        goWheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle);
        btnWheelGem.interactable = true;
        btnWheelVideo.interactable = true;
        spinning = false;
        ManageGift(itemNumber);
    }
    public void WheelStart(bool video)
    {
        btnWheelVideo.interactable = false;
        btnWheelGem.interactable = false;
        anglePerItem = 360 / maxRotaiton.Length;
        txtNumVideoWheel.text = PlayerPrefs.GetInt("VideoWheel", 3).ToString() + "/3";
        randomTime = UnityEngine.Random.Range(3, 8);
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
        StartCoroutine(SpinTheWheel(3 * randomTime, maxAngle));
    }
    #endregion//باید کدهاش بررسی شود
    public void BtnWheelWithGem()
    {
        if (PlayerPrefs.GetFloat("gem", 0) >= 5)
        {
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") - 5);
            WheelStart(false);
        }
        else
        {
            Debug.Log("Gem<5");
        }
    }
    private void ManageGift(int itemNumber)
    {
        videoAds.shopPanel.controller.panelMessage.SetActive(true);
        if (itemNumber == 0)
        {
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") + 20);
            videoAds.shopPanel.controller.SetText();
            videoAds.shopPanel.controller.txtPanelMessage.text = "20 الماس اضافه شد";
        }
        else if (itemNumber == 1)
        {
            Manager.SetActionTime("5x_earning_for_1m", (60 + Manager.GetCurrentTime()));
            videoAds.shopPanel.controller.txtPanelMessage.text = "به مدت 1 دقیقه در آمد شما 5 برابر شد";
            videoAds.shopPanel.controller.slotManager.UpdateEarningSpeedText();
        }
        else if (itemNumber == 2)
        {
            Manager.SetActionTime("2x_speed_for_150s", (150 + Manager.GetCurrentTime()));
            videoAds.shopPanel.controller.txtPanelMessage.text = "به مدت 150 ثانیه سرعت شما 2 برابر شد";
            videoAds.shopPanel.controller.slotManager.UpdateEarningSpeedText();
        }
        else if (itemNumber == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                videoAds.shopPanel.controller.SpawnABoxWheel();
            }
            videoAds.shopPanel.controller.txtPanelMessage.text = "4 جعبه طلایی به پارکینگ شما اضافه شد";
        }
        else if (itemNumber == 4)
        {
            PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin") + (videoAds.shopPanel.controller.slotManager.earnPerSec * 4 * 60 * 60));
            videoAds.shopPanel.controller.SetText();
            videoAds.shopPanel.controller.txtPanelMessage.text = "به اندازه 4 ساعت درآمد فعلی  به شما پرداخت شد";
        }
    }
}
