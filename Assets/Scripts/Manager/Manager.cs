using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Manager
{
    public static double GetCurrentTime()
    {//زمان فعلی را بصورت مقدار ثانیه برمیگرداند
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalSeconds;
    }

    public static double GetActionTime(String action)
    {//زمان مربوط به اکشن مورد نظر را بصورت جمع ثانیه ها برمیگرداند
        //Debug.Log(action + "_time: " + PlayerPrefs.GetFloat(action + "_time"));
        return CodeStage.AntiCheat.ObscuredTypes.ObscuredPrefs.GetDouble(action + "_time");
        //return double.Parse(PlayerPrefs.GetFloat(action + "_time").ToString());
    }

    public static void SetActionTime(String action, double time)
    {
        //Debug.Log(action + "_time: " + PlayerPrefs.GetFloat(action + "_time"));
        CodeStage.AntiCheat.ObscuredTypes.ObscuredPrefs.SetDouble(action + "_time", (float)time);
        //PlayerPrefs.SetFloat(action + "_time", (float)time);
        //Debug.Log(action + "_time: " + PlayerPrefs.GetFloat(action + "_time"));
    }
    public static string ChangeNumber(float number)
    {
        string outStr = number.ToString();
        if (number >= 1000000000000000000000000f)
        {
            outStr = ((number / 1000000000000000000000000f).ToString("0.0") + "AD");
        }
        else if (number >= 1000000000000000000000f)
        {
            outStr = ((number / 1000000000000000000000f).ToString("0.0") + "AC");
        }
        else if (number >= 1000000000000000000f)
        {
            outStr = ((number / 1000000000000000000f).ToString("0.0") + "AB");
        }
        else if (number >= 1000000000000000)
        {
            outStr = ((number / 1000000000000000).ToString("0.0") + "AA");
        }
        else if (number >= 1000000000000)
        {
            outStr = ((number / 1000000000000).ToString("0.0") + "T");
        }
        else if (number >= 1000000000)
        {
            outStr = ((number / 1000000000).ToString("0.0") + "B");
        }
        else if (number >= 1000000)
        {
            outStr = ((number / 1000000).ToString("0.0") + "M");
        }
        else if (number >= 1000)
        {
            outStr = ((number / 1000).ToString("0.0") + "K");
        }
        return outStr;
    }
}
