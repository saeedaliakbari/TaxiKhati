using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SimpleAndroidNotifications
{
    public class localNotification : MonoBehaviour
    {
        private double value = 0;
        void Start()
        {
            TimeSpan delay;
            string[] arr = ObscuredPrefs.GetString("TimeVideoWheel", "1992,11,30,00,00,00").Split(',');
            DateTime wheelTime = new DateTime(Int32.Parse(arr[0]), Int32.Parse(arr[1]), Int32.Parse(arr[2]), Int32.Parse(arr[3]), Int32.Parse(arr[4]), Int32.Parse(arr[5]));
            if (wheelTime > DateTime.Now)
            {
                delay = wheelTime.Subtract(DateTime.Now);
                NotificationWheel(delay);
            }
            NotificationWheel(new TimeSpan(0, 0, 1));
        }
        public void OnApplicationPause(bool pause)
        {
            ObscuredPrefs.Save();
            if (pause == false)
            {
            }

            if (pause)
            {
                float rate = ObscuredPrefs.GetFloat("offlineEarnTycoonBoosts", 1) + ObscuredPrefs.GetFloat("offliceEarnVip", 1);
                value = ObscuredPrefs.GetDouble("earnpersec") * 21600;
                value = value * rate;
                NotificationOfflineEarn(value.ToString("0.##"));
            }
        }
        private void OnApplicationQuit()
        {
            float rate = ObscuredPrefs.GetFloat("offlineEarnTycoonBoosts", 1) + ObscuredPrefs.GetFloat("offliceEarnVip", 1);
            value = ObscuredPrefs.GetDouble("earnpersec") * 21600;
            value = value * rate;
            NotificationOfflineEarn(value.ToString("0.##"));
        }
        public void NotificationOfflineEarn(string value)
        {
            NotificationManager.Cancel(1);//با وارد شدن داخل بازی باید زمان دی لی غیرفعال شود و زمان جدیدی ثبت شود
            var notificationParams = new NotificationParams
            {
                Id = 1,
                Delay = TimeSpan.FromMinutes(1),
                Title = "وقشته تاکسی جدید بخری",
                Message = "تاکسی های شیفت " + value + " درآمد کسب کردند",
                Ticker = "وقتشه تاکسی جدید بخری",
                Sound = true,
                Vibrate = true,
                Light = true,
                SmallIcon = NotificationIcon.Message,
                SmallIconColor = new Color(0, 0.6f, 1),
                LargeIcon = "app_icon"
            };
            NotificationManager.SendCustom(notificationParams);//نوتیفیکیشن جدید ساخته می شود
        }
        public void NotificationWheel(TimeSpan delay)
        {
            NotificationManager.Cancel(2);//با وارد شدن داخل بازی باید زمان دی لی غیرفعال شود و زمان جدیدی ثبت شود
            var notificationParams = new NotificationParams
            {
                Id = 2,
                Delay = delay,
                //Delay = TimeSpan.FromMinutes(3),
                Title = "بیا تو بازی",
                Message = "فرصت رایگان گردونه شانس فعال شد",
                Ticker = "بیا تو بازی",
                Sound = true,
                Vibrate = true,
                Light = true,
                SmallIcon = NotificationIcon.Message,
                SmallIconColor = new Color(0, 0.6f, 1),
                LargeIcon = "app_icon"
            };
            NotificationManager.SendCustom(notificationParams);//نوتیفیکیشن جدید ساخته می شود
        }
    }
}
