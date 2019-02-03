using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GetDateTime : MonoBehaviour
{
    //StartCoroutine(getDateTime.IEGetDateTime((status) =>{status.ToString("yyyy/MM/dd HH:mm");}));
    private static DateTime nowTime;
    private static string urlTime = "http://185.55.226.163/moshtary/Time.php";

    public static IEnumerator IEGetDateTime(Action<DateTime> callback)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("net is not reachable");
            nowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            callback(nowTime);
        }
        else {
            Debug.Log("internet is reachable");
            WWW www = new WWW(urlTime);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                if (www.isDone)
                {
                    string[] arr = www.text.Split(',');
                    int year = Int32.Parse(arr[0]);
                    int month = Int32.Parse(arr[1]);
                    int day = Int32.Parse(arr[2]);
                    int hours = Int32.Parse(arr[3]);
                    int minute = Int32.Parse(arr[4]);
                    int sec = Int32.Parse(arr[5]);
                    nowTime = new DateTime(year, month, day, hours, minute, sec);
                    //yield return nowTime;
                }
                else
                {
                    nowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
            }
            else
            {
                nowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
            callback(nowTime);
        }
    }
}
