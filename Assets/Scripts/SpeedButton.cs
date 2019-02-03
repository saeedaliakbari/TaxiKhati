using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public GameObject objImgSpeed;
    public Text txtSpeedTime;
    private bool showTime = false;
    public void UpdateButtonState(bool showTime)
    {
        objImgSpeed.SetActive(!showTime);
        this.showTime = showTime;
        if (showTime)
        {
            UpdateTime();
        }
        else
        {
            txtSpeedTime.text = "";
        }
    }
    void FixedUpdate()
    {
        UpdateTime();
    }
    private void UpdateTime()
    {
        if (showTime)
        {
            float timeValue = Mathf.Max(0, (float)(Manager.GetActionTime("speed_x2") - Manager.GetCurrentTime()));
            TimeSpan t = TimeSpan.FromSeconds(timeValue);
            txtSpeedTime.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }
    }
}
