using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    [HideInInspector]
    public Car car;//داخل تابع SpawnACar اختصاص داده می شود
    [HideInInspector]
    public bool returning = false;
    [HideInInspector]
    public bool getStart;
    // Use this for initialization
    //void Start()
    //{
    //    iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("Road"), "time", 5, "orienttopath", true, "easetype", iTween.EaseType.linear, "oncomplete", "Start"));
    //}
    float start = 0;
    public void DiverARound()
    {
        getStart = true;
        float ratio = Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2") ? 2 : 1;
        ratio = ratio * ObscuredPrefs.GetFloat("carsSpeedTycoon", 1);//carsSpeedBoostsTycoon
        Hashtable hash = iTween.Hash("path", iTweenPath.GetPath("Road"), "orienttopath", true, "speed", car.speed * ratio, "easetype", iTween.EaseType.linear,
            "oncomplete", "CompleteMoving");
        start = Time.realtimeSinceStartup;
        iTween.MoveTo(gameObject, hash);
    }
    private void CompleteMoving()//وقتی کامل شد یک دور حرکت
    {
        //Debug.Log("complete time: " + (Time.realtimeSinceStartup - start));
        DiverARound();
    }
    public void Return()
    {
        Debug.Log("Return");
        returning = true;
        //Sound.instance.Play(Sound.Others.Return);
        iTween.Stop(gameObject);
        Hashtable hash = iTween.Hash("position", car.transform.position, "orienttopath", true, "speed", 20, "easetype", iTween.EaseType.linear, "oncomplete", "CompleteReturn");
        iTween.MoveTo(gameObject, hash);
    }
    private void CompleteReturn()//کامل کردن پروسه بازگشت ماشین 
    {//جدا شده است این تکه از کد که وقتی کاملا مسیر را طی کرد این اتفاقات بیافتد
        returning = false;
        Destroy(gameObject);
        car.OnCompleteReturn();//رنگ ماشیند داخل پارکینگ را کامل می کند و پارامتر حرکت را غیرفعال می کند
        if (ObscuredPrefs.GetInt("returned_car", 0) == 0)
        {
            ObscuredPrefs.SetInt("returned_car", 1);//راهنمایی تموم شده است با بازگشت ماشین در حال حرکت
            //Controller.instance.guideManager.HideGuides();//پنل راهنما را غیرفعال می کند
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {//وقتی وارد کلایدر با تگ پایان شد
        //Debug.Log("Coilder Tag: " + col.tag);
        if (getStart && col.tag == "FinishGoal")
        {
            getStart = false;
            car.FinishRound();
        }
    }
}
