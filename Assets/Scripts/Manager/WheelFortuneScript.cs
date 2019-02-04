using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class WheelFortuneScript : MonoBehaviour
{
    public VideoAds videoAds;

    public List<int> prize;
    public List<AnimationCurve> animationCurves;

    private bool spinning;
    private float anglePerItem, chornoTimeCounter;
    private int randomTime;
    private int itemNumber;

    public Text txtNumVideoWheel, txtTimeRemain;
    public GameObject objTimeVideo, objNumVideoWheel;
    public Button btnWheelVideo;
    public GameObject goWheel;
    private string[] timeWheel = { "timeWheel1", "timeWheel2", "timeWheel3" };
    void Start()
    {
        CheckVideoTime();
        //objNumVideoWheel.SetActive(false);
    }
    private void CheckVideoTime()//این تابع در ابتدا مقادیر را داخل تکست باکس ها ست می کند و سپس با توجه به زمان فعلی و اینکه تعداد شانس ها کمتر از 3 باشد زمان شانس بعدی را می سنجد تا اضافه شود
    {
        //Debug.Log("CheckVideoTime >" + PlayerPrefs.GetInt("VideoWheel", 3));
        txtNumVideoWheel.text = PlayerPrefs.GetInt("VideoWheel", 3).ToString();
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
                        txtNumVideoWheel.text = PlayerPrefs.GetInt("VideoWheel", 3).ToString();
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
    }
    public void GiftWheelWithVideo()//وقتی که یک بار از ویدئو استفاده کرد برای چرخاندن گردونه شانس باید این تابع فراخوانی شود
    {
        Debug.Log("GiftWheelWithVideo");
        PlayerPrefs.SetInt("VideoWheel", PlayerPrefs.GetInt("VideoWheel", 3) - 1);
        Debug.Log("GiftWheelWithVideo" + PlayerPrefs.GetInt("VideoWheel", 3));
        btnWheelVideo.interactable = false;
        if (PlayerPrefs.GetString("TimeVideoWheel", "NotSet") == "NotSet")
        {
            TimeSpan nowTimeSpan = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan plusTimeSpan = new TimeSpan(0, 8, 0, 0);
            TimeSpan result = plusTimeSpan + nowTimeSpan;
            PlayerPrefs.SetString("TimeVideoWheel", DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + result.Days.ToString() + "," + result.Hours.ToString() + "," + result.Minutes.ToString() + "," + result.Seconds.ToString());
            Debug.Log("NOT SET >>" + PlayerPrefs.GetString("TimeVideoWheel"));
        }
        CheckVideoTime();
        WheelStart();
        btnWheelVideo.interactable = true;
    }
    IEnumerator IETimerVideoWheel(float deltaTime)//تابع تایمر ویدئو
    {
        Debug.Log("Start Remain Time :" + deltaTime);
        for (; deltaTime > 0; deltaTime -= 1f)
        {
            int s = (int)(deltaTime % 60);
            int m = (int)((deltaTime % 3600) / 60);
            int h = (int)(deltaTime / 3600);
            txtTimeRemain.text = "" + h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            //Debug.Log("txtTimeRemain: " + txtTimeRemain.text);
            if (m == 0 && s == 1)
            {
                txtTimeRemain.text = "00:00:00";
            }
            yield return new WaitForSeconds(1f);
        }
        if (deltaTime == 0)
        {
            txtTimeRemain.text = "00:00:00";
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
        //yield return new WaitForSeconds(2f);
        spinning = true;

        float timer = 0.0f;
        float startAngle = goWheel.transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle + 90f;

        int animationCurveNumber = UnityEngine.Random.Range(0, animationCurves.Count);
        Debug.Log("Animation Curve No. : " + animationCurveNumber);

        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            goWheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }

        goWheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;

        Debug.Log("Prize: " + prize[itemNumber]);//use prize[itemNumnber] as per requirement
        //if (prize[itemNumber] == 123)
        //{
        //    PlayerPrefs.SetInt("WheelOfFourune", 1);
        //    txtNumVideoWheel.text = PlayerPrefs.GetInt("WheelOfFourune", 1).ToString();
        //    Debug.Log("shans mojadad.");
        //}
        //else
        //{
        //    //PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin", 0) + prize[itemNumber]);
        //    //txtCoin.text = PlayerPrefs.GetInt("coin", 0).ToString();
        //    //panelMessage.gameObject.SetActive(true);
        //}
        //if (PlayerPrefs.GetInt("WheelOfFourune", 1) == 0)
        //{
        //    //btnWheel.interactable = false;
        //    btnWheelVideo.interactable = false;
        //}
        //else
        //{
        //    btnWheelVideo.interactable = true;
        //    //btnWheel.interactable = true;
        //}
    }
    public void WheelStart()
    {
        if (!spinning)
        {
            goWheel.gameObject.SetActive(true);
            //btnWheel.gameObject.SetActive(false);
        }
        else
        {
            //btnWheel.interactable = true;
            btnWheelVideo.interactable = true;
        }
        txtNumVideoWheel.text = PlayerPrefs.GetInt("VideoWheel", 3).ToString();
        randomTime = UnityEngine.Random.Range(1, 7);
        int iPercent = UnityEngine.Random.Range(0, 100);
        Debug.Log("darsad>>" + iPercent);
        if (iPercent < 5)
        {//2000
            itemNumber = 5;
        }
        else if (iPercent < 20)
        {//try
            itemNumber = 3;
        }
        else if (iPercent < 40)
        {//30
            itemNumber = 7;
        }
        else if (iPercent < 55)
        {//100
            itemNumber = 0;
        }
        else if (iPercent < 70)
        {//150
            itemNumber = 1;
        }
        else if (iPercent < 80)
        {//500
            itemNumber = 2;
        }
        else if (iPercent < 90)
        {//800
            itemNumber = 4;
        }
        else if (iPercent < 100)
        {//1000
            itemNumber = 6;
        }
        Debug.Log("itemnum>>" + itemNumber);
        float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);

        StartCoroutine(SpinTheWheel(5 * randomTime, maxAngle));
    }

    #endregion//باید کدهاش بررسی شود
    public void BtnWheelWithGem()
    {
        if (PlayerPrefs.GetFloat("gem", 0) >= 5)
        {
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") - 5);

            WheelStart();
        }
        else
        {
            Debug.Log("Gem<5");
        }
    }

    #region ghadimi
    #region Calculate Time
    public void calcuteRemainTime(string strRemainTime, Text txtTimeRemain, Button btnWheel, GameObject imgTime)
    {
        Debug.Log(strRemainTime);
        string[] arr = strRemainTime.Split(',');
        int year = Int32.Parse(arr[0]);
        int month = Int32.Parse(arr[1]);
        int day = Int32.Parse(arr[2]);
        int hours = Int32.Parse(arr[3]);
        int minute = Int32.Parse(arr[4]);
        int sec = Int32.Parse(arr[5]);
        StartCoroutine(GetDateTime.IEGetDateTime((status) =>
        {
            Debug.Log("status : " + status);
            if (year >= status.Year)
            {
                if (month >= status.Month)
                {
                    TimeSpan remainTime = new TimeSpan(day, hours, minute, sec);
                    TimeSpan nowTimeSpan = new TimeSpan(status.Day, status.Hour, status.Minute, status.Second);
                    TimeSpan result = remainTime - nowTimeSpan;
                    chornoTimeCounter = (float)((result.Hours * 3600) + (result.Minutes * 60) + result.Seconds);
                }
                else
                {
                    //bayad faal shavad.
                    chornoTimeCounter = 0;
                }
            }
            else
            {
                //bayad faal shavad.
                chornoTimeCounter = 0;
            }
            if (chornoTimeCounter <= 0)
            {
                chornoTimeCounter = 0;

            }
            Debug.Log("chornotime>>>>" + chornoTimeCounter);
            StartCoroutine(IEtimeRemain(chornoTimeCounter, txtTimeRemain, btnWheel, imgTime));
        }));
    }
    IEnumerator IEtimeRemain(float deltaTime, Text txtTimeRemain, Button btnWheel, GameObject imgTime)
    {
        Debug.Log("IEtimeRemain" + deltaTime);
        btnWheel.interactable = false;
        imgTime.SetActive(true);
        for (; deltaTime > 0; deltaTime -= 1f)
        {
            int s = (int)(deltaTime % 60);
            int m = (int)((deltaTime % 3600) / 60);
            int h = (int)(deltaTime / 3600);
            txtTimeRemain.text = "" + h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            //Debug.Log("txtTimeRemain: " + txtTimeRemain.text);
            if (m == 0 && s == 1)
            {
                imgTime.SetActive(false);
                txtTimeRemain.text = "00:00:00";
            }
            yield return new WaitForSeconds(1f);
        }
        if (deltaTime == 0)
        {
            imgTime.SetActive(false);
            txtTimeRemain.text = "00:00:00";
            btnWheel.interactable = true;
            if (PlayerPrefs.GetInt(videoAds.zoneWheelOfFurtune.zoneId) == 0)
            {
                videoAds.LoadAd(videoAds.zoneWheelOfFurtune);
            }
            CheckTheTimeWheel();
            if (true)
            {
                //PlayerPrefs.SetInt("WheelOfFourune", PlayerPrefs.GetInt("WheelOfFourune", 1) + 1);
            }
        }
        yield return new WaitForSeconds(0f);
    }
    #endregion
    private void CheckTheTimeWheel()
    {
        Debug.Log("start Check");
        float[] checkTime = { 0, 0, 0 };
        int[] year = new int[3], month = new int[3], day = new int[3], hours = new int[3], minute = new int[3], sec = new int[3];
        for (int i = 0; i < 3; i++)
        {
            string[] arr = PlayerPrefs.GetString(timeWheel[i], "1992,11,30,00,00,00").Split(',');
            year[i] = Int32.Parse(arr[0]);
            month[i] = Int32.Parse(arr[1]);
            day[i] = Int32.Parse(arr[2]);
            hours[i] = Int32.Parse(arr[3]);
            minute[i] = Int32.Parse(arr[4]);
            sec[i] = Int32.Parse(arr[5]);
        }
        StartCoroutine(GetDateTime.IEGetDateTime((status) =>
        {
            Debug.Log("NOw Time: " + status);
            for (int i = 0; i < 3; i++)
            {
                if (year[i] >= status.Year)
                {
                    if (month[i] >= status.Month)
                    {
                        TimeSpan remainTime = new TimeSpan(day[i], hours[i], minute[i], sec[i]);
                        TimeSpan nowTimeSpan = new TimeSpan(status.Day, status.Hour, status.Minute, status.Second);
                        TimeSpan result = remainTime - nowTimeSpan;
                        checkTime[i] = (float)((result.Hours * 3600) + (result.Minutes * 60) + result.Seconds);
                    }
                    else
                    {
                        checkTime[i] = 0;
                    }
                }
                else
                {
                    checkTime[i] = 0;
                }
                if (checkTime[i] <= 0)
                {
                    checkTime[i] = 0;
                }
            }
            Debug.Log("chekTime: " + checkTime[0] + " " + checkTime[1] + " " + checkTime[2]);
            if (checkTime[0] == 0)
            {
                if (checkTime[1] == 0)
                {
                    if (checkTime[2] == 0)
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 3);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 2);
                    }
                }
                else
                {
                    if (checkTime[2] == 0)
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 2);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 1);
                    }
                }
            }
            else
            {
                if (checkTime[1] == 0)
                {
                    if (checkTime[2] == 0)
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 2);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 1);
                    }
                }
                else
                {
                    if (checkTime[2] == 0)
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 1);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("WheelOfFourune", 0);
                        #region Time
                        float[] timeRemain = { 0f, 0f, 0f };
                        timeRemain = TimeRemain(year, month, day, hours, minute, sec);
                        int index = -1;
                        for (int i = 0; i < 3; i++)
                        {
                            if (timeRemain[i] == MinTime(timeRemain))
                            {
                                index = i;
                            }
                        }
                        Debug.Log("MIN TIME: " + index);
                        StartCoroutine(IEtimeRemain(checkTime[index], txtTimeRemain, btnWheelVideo, objTimeVideo));
                        #endregion
                    }
                }
            }
            Debug.Log("End CHECK");
            ManageButtons();
        }));

    }
    private void ChangeStatus(bool status)
    {
        if (status)//یعنی می تواند بچرخاند
        {

        }
        else
        {

        }
    }
    private float MinTime(float[] timeRemain)
    {

        float minTime = Mathf.Min(timeRemain[0], timeRemain[0], timeRemain[0]);
        Debug.Log("Min Time:" + minTime);
        return minTime;
    }
    private float[] TimeRemain(int[] year, int[] month, int[] day, int[] hours, int[] minute, int[] sec)
    {
        float[] timeRemain = { 0f, 0f, 0f };
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(">>" + year[i] + month[i] + day[i] + hours[i] + minute[i] + sec[i]);
            timeRemain[i] = year[i] * 10000000000f + month[i] * 100000000f + day[i] * 1000000f + hours[i] * 10000f + minute[i] * 100f + sec[i];

        }
        Debug.Log(">>" + year[0] + month[0] + day[0] + hours[0] + minute[0] + sec[0] + "2 " + year[1] + month[1] + day[1] + hours[1] + minute[1] + sec[2] + "3" + year[2] + month[2] + day[2] + hours[2] + minute[2] + sec[2]);
        Debug.Log("time Remain: " + timeRemain[0] + " " + timeRemain[0] + " " + timeRemain[0]);
        return timeRemain;
    }
    private void ManageButtons()
    {
        if (PlayerPrefs.GetInt("WheelOfFourune", 1) == 0)
        {
            //btnWheel.interactable = false;
            btnWheelVideo.interactable = false;
            objNumVideoWheel.SetActive(false);
            objTimeVideo.SetActive(true);
        }
        else
        {
            objTimeVideo.SetActive(false);
            txtTimeRemain.text = "00:00:00";
            btnWheelVideo.interactable = true;
            //btnWheel.interactable = true;
            objNumVideoWheel.SetActive(true);
            txtNumVideoWheel.text = PlayerPrefs.GetInt("WheelOfFourune", 1).ToString();
        }
        txtNumVideoWheel.text = PlayerPrefs.GetInt("WheelOfFourune", 1).ToString();
        spinning = false;
        anglePerItem = 360 / prize.Count;
    }

    private void UseWheel()
    {
        PlayerPrefs.SetInt("WheelOfFourune", PlayerPrefs.GetInt("WheelOfFourune", 1) - 1);
        int[] year = new int[3], month = new int[3], day = new int[3], hours = new int[3], minute = new int[3], sec = new int[3];
        for (int i = 0; i < 3; i++)
        {
            string[] arr = PlayerPrefs.GetString(timeWheel[i], "1992,11,30,00,00,00").Split(',');
            year[i] = Int32.Parse(arr[0]);
            month[i] = Int32.Parse(arr[1]);
            day[i] = Int32.Parse(arr[2]);
            hours[i] = Int32.Parse(arr[3]);
            minute[i] = Int32.Parse(arr[4]);
            sec[i] = Int32.Parse(arr[5]);
        }
        float[] timeRemain = { 0f, 0f, 0f };
        timeRemain = TimeRemain(year, month, day, hours, minute, sec);
        int index = -1;
        for (int i = 0; i < 3; i++)
        {
            if (timeRemain[i] == MinTime(timeRemain))
            {
                index = i;
            }
        }
        StartCoroutine(GetDateTime.IEGetDateTime((status) =>
        {
            TimeSpan nowTimeSpan = new TimeSpan(status.Day, status.Hour, status.Minute, status.Second);
            Debug.Log(nowTimeSpan.Days + "//" + nowTimeSpan.Hours + ":" + nowTimeSpan.Minutes + ":" + nowTimeSpan.Seconds);
            TimeSpan plusTimeSpan = new TimeSpan(0, 3, 0, 0);
            TimeSpan result = plusTimeSpan + nowTimeSpan;
            Debug.Log(result.Days + "//" + result.Hours + ":" + result.Minutes + ":" + result.Seconds);
            PlayerPrefs.SetString(timeWheel[index], DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + result.Days.ToString() + "," + result.Hours.ToString() + "," + result.Minutes.ToString() + "," + result.Seconds.ToString());
        }));
    }
    #endregion
}
