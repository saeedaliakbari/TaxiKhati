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
        if (PlayerPrefs.GetFloat("gem", 0) >= COST)
        {
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem", 0) - COST);
            controller.SetText();
            float timeValue = Mathf.Max(0, (float)(Manager.GetActionTime("speed_x2") - Manager.GetCurrentTime()));
            double nowtime = Math.Round(Manager.GetCurrentTime());
            double plusTime = Math.Round(150 + timeValue + Manager.GetCurrentTime());
            Manager.SetActionTime("speed_x2", plusTime);
        }
        else
        {
            controller.txtError.text = "به مقدار کافی جم ندارید";
            controller.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                controller.txtError.gameObject.SetActive(false);
            });
        }
    }
}
