using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSlotManager : MonoBehaviour
{
    public RunSlot slotPrefab;
    private List<RunSlot> listSlot = new List<RunSlot>();
    private int numRun = 0;//تعداد ماشین های در حال حرکت
    private double earningPerSec;
    public double earnPerSec;
    public TrimNumberText earningPerSecTxt, txtEarningWithSec;
    public SpriteRenderer goal;
    public Sprite[] goalSprites;
    public SpeedButton speedBtn;

    public TextMesh txtNum;
    private bool hasSpeedX2 = false;
    private Coroutine lastRoutine = null;
    public double EarningPerSec
    {
        get
        {
            return earningPerSec;
        }
        set
        {
            earningPerSec = value;
            UpdateEarningSpeedText();
        }
    }
    void FixedUpdate()
    {
        if (hasSpeedX2 != (Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2")))
        {//اگر که سرعت دوبرابر فعال باشد و همچنین زمان فعلی از زمان سرعت دوبرابر کمتر بود
            UpdateEarningSpeedText();
            hasSpeedX2 = (Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2"));
            speedBtn.UpdateButtonState(hasSpeedX2);
            //Music.instance.Play(hasSpeedX2 ? Music.Type.SpeedX2 : Music.Type.MainMusic);
        }
    }
    public void UpdateEarningSpeedText()
    {
        int index = ObscuredPrefs.GetInt("unlocked_airline", 1) - 1;//عدد آخرین هواپیمای باز شده
        //Debug.Log("index unlocked_airline: " + index + ">>>earningPerSec: " + earningPerSec);
        float ratio = ((Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2")) ? 2 : 0) /** Const.AIRLINE_INCREASE_PERCENT[index]*/;//ریت بدست آوردن سکه
        ratio += ((Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m")) ? 5 : 0);
        ratio += ((Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m_special")) ? 5 : 0);
        ratio += ((Manager.GetCurrentTime() < Manager.GetActionTime("2x_speed_for_150s")) ? 2 : 0);
        ratio += ObscuredPrefs.GetFloat("speedVip", 0);
        ratio += ObscuredPrefs.GetFloat("incomeLine", 1);
        earnPerSec = System.Math.Round(earningPerSec * ratio);
        earningPerSecTxt.text = earnPerSec.ToString("0.##");
        txtEarningWithSec.text = "ﻪﯿﻧﺎﺛ / " + earningPerSecTxt.text;
    }

    public void InitSlots()
    {
        EarningPerSec = 0;
        int num = ObscuredPrefs.GetInt("num_of_slot", 2);//تعداد لاین های شروع 
        num += ObscuredPrefs.GetInt("num_of_slot_vip", 0);
        ObscuredPrefs.SetInt("num_total_slot", num);
        for (int i = 0; i < num; i++)
        {//به تعداد لاین هایی که هست ، لاین ایجاد می کند
            SpawnASlot();
        }
        UpdatePosition();//موقعیت تمامی لاین ها را درست می کند
        UpdateState();
        UpdateStartGoal();
    }
    public void InitSlotsVIP()
    {
        int num = ObscuredPrefs.GetInt("num_of_slot", 2);//تعداد لاین های شروع 
        num += ObscuredPrefs.GetInt("num_of_slot_vip", 0);
        Debug.Log("SLOT :num: " + ObscuredPrefs.GetInt("num_of_slot", 2) + " numVIP: " + ObscuredPrefs.GetInt("num_of_slot_vip", 0) + " TOTAL: " + ObscuredPrefs.GetInt("num_total_slot"));
        if (num > ObscuredPrefs.GetInt("num_total_slot") && ObscuredPrefs.GetInt("num_total_slot") < 13)
        {
            int newSlot = num - ObscuredPrefs.GetInt("num_of_slot", 2);
            for (int i = 0; i < newSlot; i++)
            {//به تعداد لاین هایی که هست ، لاین ایجاد می کند
                ObscuredPrefs.SetInt("num_total_slot", ObscuredPrefs.GetInt("num_total_slot") + 1);
                SpawnASlot();
            }
            UpdatePosition();//موقعیت تمامی لاین ها را درست می کند
            UpdateState();
            UpdateStartGoal();
        }

    }
    public void SpawnASlot()
    {
        RunSlot slot = (RunSlot)Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);//یک لاین ایجاد می کند
        //Debug.Log("scale: " + slot.transform.localScale);
        slot.transform.SetParent(transform);//پدر را در هایرارکی همین آبجکت قرار می دهد
        slot.transform.localScale = Vector3.one * 0.2f;//اسکیل لاین را یک قرار می دهد
        //Debug.Log("scale: " + slot.transform.localScale);
        listSlot.Add(slot);//داخل لیستی که لاین ها هستن این لاین را اضافه می کندبه آخر لیست
    }
    public void UpdatePosition()
    {
        //LineAnimation();
        for (int i = 0; i < listSlot.Count; i++)
        {//به تعداد لاین هایی که در لیست لاین ها هست
            listSlot[i].transform.localPosition = new Vector3(0, (i - ((listSlot.Count - 1) / 2f)) * 0.09f);//موقعیت ایکس تغییری نمی کند. و موقعیت ایگری به نسبت شماره داخل لیست 
        }
        txtNum.transform.localPosition = new Vector3(0, (listSlot.Count - ((listSlot.Count - 1) / 2f)) * 0.09f);
    }
    public void UpdateState()
    {
        int countStartRun = 0;
        for (int i = 0; i < listSlot.Count; i++)//با توجه به تعداد ایستگاه های استارت
        {
            listSlot[i].UpdateSprite(i < numRun);//حالات ایستگاه ها را آپدیت می کند
            if (i < numRun)
                countStartRun += 1;
        }
        txtNum.text = countStartRun + "/" + (ObscuredPrefs.GetInt("num_of_slot", 2) + ObscuredPrefs.GetInt("num_of_slot_vip", 0));
        LineAnimation();
        //Debug.Log(countStartRun + "/" + (ObscuredPrefs.GetInt("num_of_slot", 2) + ObscuredPrefs.GetInt("num_of_slot_vip", 0)));
    }
    public void UpdateStartGoal()
    {
        UpdateState();
        int index = ObscuredPrefs.GetInt("unlocked_airline", 1) - 1;
        goal.sprite = goalSprites[index];//اسپرایت خط پایان را با توجه به مدل لاین شروع تغییر می دهد
        UpdateEarningSpeedText();
    }
    public bool IsFull()//آیا جایگاه های استارت پر است؟
    {
        return numRun == listSlot.Count;
    }
    public bool IsEmpty()
    {
        return numRun == 0;
    }
    public void RunACar(double earning)//تابع هنگام حرکت کردن یک ماشین فراخوانی می شود جهت آپدیت شدن بدست آوردن سکه و هچنین ایستگاه های شروع
    {
        if (!IsFull())
        {
            EarningPerSec += earning;
            //Debug.Log("EarningPerSec: " + EarningPerSec + " + earning: " + earning);
            numRun++;
            UpdateState();
        }
    }
    public void StopACar(double earning)//تابع هنگام ایستادن یک ماشین فراخوانی می شود جهت آپدیت شدن مقدار بدست آوردن سکه و همچنین ایستگاه های شروع
    {
        if (numRun >= 1)
        {
            EarningPerSec = System.Math.Max(0, EarningPerSec - earning);
            numRun--;
            UpdateState();
        }
    }


    public void ShowGoalAnimation()
    {
        goal.GetComponent<Animator>().Play("ReachToGoal");
        Controller.instance.ShowCoinEffect(goal.transform.position);
    }

    public void LineAnimation()
    {
        if (IsEmpty()) ///// اگر لاین خالی بود
        {
            if (lastRoutine != null)
            {
                StopCoroutine(lastRoutine);
            }
            lastRoutine = StartCoroutine(LineAnimator());
        }
        else
        {
            StopCoroutine(lastRoutine);
        }

    }

    public IEnumerator LineAnimator()
    {

        for (int i = 0; i <= (listSlot.Count + 2); i++)
        {
            for (int j = 0; j < listSlot.Count; j++)
            {
                listSlot[j].changeSprite(false, false);
            }

            if (i <= (listSlot.Count - 1))
            {
                listSlot[i].changeSprite(true, false);
            }

            if (i >= 1 && i <= listSlot.Count)
            {
                listSlot[i - 1].changeSprite(true, true);
            }

            if (i >= 2 && i <= (listSlot.Count + 1))
            {
                listSlot[i - 2].changeSprite(true, false);
            }


            yield return new WaitForSeconds(0.1f);
        }


        LineAnimation();


    }
}
