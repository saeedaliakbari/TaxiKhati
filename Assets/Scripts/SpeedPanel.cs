using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedPanel : MonoBehaviour
{
    public Controller controller;
    public Text txtTimer;
    public Image imgProgress;
    private int COST = 3, BUY_TIME = 150;
    public void ShowDialog()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        float timeValue = Mathf.Max(0, (float)(Math.Round(Manager.GetActionTime("speed_x2") - Manager.GetCurrentTime())));
        TimeSpan t = TimeSpan.FromSeconds(timeValue);
        TimeSpan max = new TimeSpan(0, 30, 0);
        //Debug.Log("t : " + t + " max: " + max);
        if (t > max)
        {
            Manager.SetActionTime("speed_x2", (Manager.GetActionTime("speed_x2") - (t - max).TotalSeconds));
            t = max;
        }
        txtTimer.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        float percent = Mathf.Min(1, timeValue / (BUY_TIME * 12));
        imgProgress.fillAmount = percent;
    }

    void FixedUpdate()
    {
        UpdateTime();
    }

    public void BuySpeedX2WithGem()
    {
        //Sound.instance.Play(Sound.Others.Buy);
        if (ObscuredPrefs.GetDouble("gem", 0) >= COST)
        {
            float timeValue = Mathf.Max(0, (float)(Manager.GetActionTime("speed_x2") - Manager.GetCurrentTime()));
            float plus = 150;
            if (timeValue + 150 > 1800)
            {
                plus = 1800 - timeValue;
            }
            Debug.Log("plus: " + plus + " time: " + timeValue);
            double nowtime = Math.Round(Manager.GetCurrentTime());
            double plusTime = Math.Round(plus + timeValue + Manager.GetCurrentTime());
            Debug.Log("New Time : " + plusTime);
            Manager.SetActionTime("speed_x2", plusTime);
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 0) - COST);
            controller.SetText();
        }
        else
        {
            controller.txtError.text = "به مقدار کافی الماس ندارید";
            controller.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                controller.txtError.gameObject.SetActive(false);
            });
        }
    }
}
