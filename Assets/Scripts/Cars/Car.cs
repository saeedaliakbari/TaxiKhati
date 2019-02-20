using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//کلاس ماشین پارک شده داخل پارکینگ
public class Car : MonoBehaviour
{
    public int level;
    public int xp;
    public float earnings;
    public float speed;
    public float increasePercent;
    [HideInInspector]
    public TrimNumberText txtCoin;
    [HideInInspector]
    public bool moving = false;
    [HideInInspector]
    public ParkingPlace parkingPlace;
    [HideInInspector]
    public Controller controller;
    public MoveCar moveCarPrefab;
    private MoveCar moveCar;
    private const float timeLab = 19.39939f;
    private void OnMouseDrag()//کشیدن  ماشین
    {
        //Debug.Log("On Mouse Drag");
        //if (HomeController.instance.IsDialogShowed()) return;//اگر پنلی فعال باشد در صفحه امکان درگ کردن نباشد
        if (!moving)//اگر ماشبن در حال حرکت نبود
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPos;
            GetComponent<SpriteRenderer>().sortingOrder = 10;//برای اینکه روی بقیه آبجکت ها قرار گیرد
        }
    }

    private void OnMouseUp()//کشیدن را تمام کرد
    {
        //Debug.Log("On Mouse Up "+moving+":>>"+ Controller.instance.IsDialogShowed());
        if (Controller.instance.IsDialogShowed()) return;//اگر پنلی فعال باشد در صفحه امکان درگ کردن نباشد

        if (!moving)//اگر ماشین درحال حرکت نبود
        {
            ParkingPlace nearPlace = Controller.instance.parkingManager.GetNearestPlace(transform.position);//نزدیکترین مکان 
            //Debug.Log("!moving");
            if (nearPlace != null && nearPlace != parkingPlace)//اگر نزدیکترین مکان خالی نبود و با مکان فعلی یکسان نبود
            {
                if (!nearPlace.IsEmpty())//اگر نزدیکترین مکان خالی نبود
                {
                    Car car2 = nearPlace.GetCar();
                    if (car2 != null && !car2.moving)//اگر ماشین داخل پارکینگ نزدیک بود و داخل پارکینگ پارک بود
                    {
                        transform.position = nearPlace.transform.position;
                        if (level == car2.level && level < Controller.instance.carPrefabs.Length)//اگر لول دوتا ماشین یکی بود و لول از تعداد ماشین ها کمتر با هم مرج شوند
                        {//Merge
                            if (PlayerPrefs.GetInt("returned_car", 0) == 0)//اگر راهنما تمام نشده بود
                            {
                                PlayerPrefs.SetInt("merged_car", 1);//0 false , 1 true
                                //HomeController.instance.guideManager.HideGuides();
                                //HomeController.instance.guideManager.UpdateAfter(1);
                            }
                            PlayerPrefs.SetInt("mergeCarForVideo", PlayerPrefs.GetInt("mergeCarForVideo", 1) + 1);
                            GetComponent<Animator>().Play("Merge");//انیمیشن مرج اجرا شود
                            GetComponent<SpriteRenderer>().enabled = false;
                            Destroy(car2.gameObject);//ماشین دومی را از بین می بریم که یک ماشین باقی بماند
                            //بعد از نیم ثانیه کارهای زیر را انجام بده
                            Timer.Schedule(this, 0.5f, (Timer.Task)(() =>
                            {
                                Controller.instance.SpawnACar((int)level, (ParkingPlace)nearPlace, (bool)true);//ماشین جدید ساخته می شه به جای ماشین که تاچ شه
                                PlayerPrefs.SetInt("checkLevel", 0);
                                Destroy(gameObject);//ماشین فعلی از بین میره
                            }));
                            int unlockedLevel = PlayerPrefs.GetInt("unlocked_car", 1);
                            //Debug.Log("unlockedLevel" + unlockedLevel + "level" + level);
                            if (level + 1 > unlockedLevel)
                            {//اگر لول بیشتر از لول ماشین ماکس باشد
                                PlayerPrefs.SetInt("unlocked_car", level + 1);
                                PlayerPrefs.SetInt("curr_car_index", controller.lastSalableCoreLevel[PlayerPrefs.GetInt("unlocked_car", 1) - 1] - 1);
                                Controller.instance.ShowMergeNewCar(level - 1);//در صورتی که ماشین جدید باز شود پنل مرج باز می شود که مرج انجام می شود
                            }
                            else {//در صورتی که ماشین لول جدید باز نشود با مرج و ماشین های قبلی تولید شود وقت صدای مرج پخش می شود
                                //Sound.instance.Play(Sound.Others.Merge);
                            }
                            //باید مقدار افزوده شدن ایکس پی اضافه شود
                            //Debug.Log("XP: " + PlayerPrefs.GetInt("Xp", 0));
                            PlayerPrefs.SetInt("Xp", PlayerPrefs.GetInt("Xp", 0) + xp);
                            //Debug.Log("XP: " + PlayerPrefs.GetInt("Xp", 0));
                            Controller.instance.playerLevel.UpdateProgress(/*(int)Mathf.Pow(1.5f, level)*/);//مقدار ایکس و لول ست شود

                        }
                        else
                        {//دوتاماشین با هم جابه جا می شوند
                            ParkingPlace lastPlace = parkingPlace;//ماشین اول در جایگاه ماشین دوم قرار میگیرد
                            parkingPlace = nearPlace;
                            transform.SetParent(nearPlace.transform);
                            car2.MoveToPlace(lastPlace);//ماشین دوم به سمت جایگاه ماشین اول می رود
                        }
                    }
                    else//اگر ماشین داخل پارکینگ نزدیک نباشه یا در حال حرکت باشد
                    {
                        transform.position = parkingPlace.transform.position;//ماشین جابه جا نمی شود و به مکان اولیه برمیگردد
                        //Sound.instance.Play(Sound.Others.Unswap);
                    }
                }
                else//جابجایی به مکان جدید
                {
                    transform.position = nearPlace.transform.position;//موقعیت ماشین را به موقعیت نزدیکترین مکان که خالی هم هست تغییر می دهد
                    parkingPlace = nearPlace;//پارکینگش را عوض می کند
                    transform.SetParent(nearPlace.transform);//داخل هایرارکی فرزند آبجکت پارکینگ جدید شد
                }
            }
            else
            {//اگر نزدیکترین مکانی تشخیص نداد یا اینکه ان مکان با مکان فعلی یکسان بود
                if (!Controller.instance.slotManager.IsFull() /*جایگاه های استارت پر نباشد*/&& Vector3.Distance(transform.position, Controller.instance.slotManager.transform.position) < 0.5f/*فاصله اش تا جایگاه استارت کمتر از 0.5 باشد*/)
                {//اگر گذاشته شود در نقطه استارت
                    StartDrive();//شروع پرواز
                    //Sound.instance.Play(Sound.Others.Start);
                }
                else if (Vector3.Distance(transform.position, Controller.instance.deleteBin.transform.position) < 0.5f)
                {//اگر آن ماشین را حذف کند
                    DismantleCar();
                }
                else
                {//در مکان اولیه قرار می گیردوهیچ تغییری اعمال نمی شود
                    transform.position = parkingPlace.transform.position;
                }
            }
        }
        else//اگر ماشین در حال حرکت بود
        {
            Debug.Log("Not Move" + moveCar.returning);
            if (!moveCar.returning)
            {
                moveCar.Return();//به همون مکان برگردد
                Controller.instance.slotManager.StopACar(GetEarningPerSecond());
            }
        }
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        PlayerPrefs.SetInt("checkLevel", 0);
    }
    private void DismantleCar()
    {
        //Sound.instance.Play(Sound.Others.Goal);
        //CurrencyController.CreditBalance(0, Mathf.RoundToInt(Superpow.Utils.GetPrice(level - 1) * 0.5f));
        Controller.instance.ShowCoinEffect(Controller.instance.deleteBin.transform.position);//در موقعیت سطل آشغال یک افکت سکه ایجاد می کند
        PlayerPrefs.SetInt("checkLevel", 0);
        Destroy(gameObject);//این ماشین را از بین می برد
    }
    public float GetEarningPerSecond()//مقدار بدست آوردن سکه در هر ثانیه
    {//یعنی این ماشین چقدر سکه در هرثانیه بدست می اورد با توجه به لولش
        return ((earnings * speed) / timeLab);
    }
    public void OnCompleteReturn()
    {
        GetComponent<SpriteRenderer>().color = Color.white;//رنگ ماشین داخل پارکینگ را که کمرنگ کرده بودیم را کامل می کنیم
        moving = false;
    }
    public void StartDrive()//شروع حرکت
    {
        if (PlayerPrefs.GetInt("returned_car", 0) == 0)
        {//اگر راهنما تمام نشده بود
            PlayerPrefs.SetInt("runned_car", 1);//0 false , 1 true
            //Superpow.Utils.SetRunnedPlane();
            //Controller.instance.guideManager.HideGuides();
            //Controller.instance.guideManager.UpdateAfter(3);
        }
        moving = true;
        moveCar = SpawnACar();
        moveCar.transform.position = Controller.instance.slotManager.transform.position;//موقعیت ماشین درحال حرکت در مکان استارت قرار میگیرد
        moveCar.DiverARound();//حرکت ماشین 
        transform.position = parkingPlace.transform.position;//موقعیت ماشین ایستاده با پارکینگ پر می شود
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);//رنگ را کم رنگتر می کند
        Controller.instance.slotManager.RunACar(GetEarningPerSecond());//یک ماشین در حال حرکت ایجاد می کند با مقدار سکه اضافه شده
    }
    public void StartDriveInSecond(float time)
    {//بعد از زمان مشخص شده ای شروع حرکت انجام می شود
        Timer.Schedule(this, time, () =>
        {
            StartDrive();
        });
    }
    private MoveCar SpawnACar()
    {
        MoveCar mCar = (MoveCar)Instantiate(moveCarPrefab, Vector3.zero, Quaternion.identity);
        mCar.transform.localScale = Vector3.one * 0.55f;//اسکیل ماشین در حال حرکت ایجاد شده
        mCar.car = this;//اختصاص دادن این ماشین به پارامتر ماشین در اسکریپت ماشین در حال حرکت ایجاد شده.
        return mCar;
    }
    public void MoveToPlace(ParkingPlace newPlace)
    {//جابجایی به مکان جدید
        parkingPlace = newPlace;
        transform.SetParent(newPlace.transform);
        Hashtable hash = iTween.Hash("position", newPlace.transform.position, "speed", speed * 3, "easetype", iTween.EaseType.linear);//ماشین را به مکان مشخص شده حرکت می دهد
        iTween.MoveTo(gameObject, hash);
    }
    public void FinishRound()
    {
        //Debug.Log("Get Coin");
        //Sound.instance.Play(Sound.Others.Goal);
        int index = PlayerPrefs.GetInt("unlocked_airline", 1) - 1;//لول آخرین ماشین باز شده را می دهد
        //CurrencyController.CreditBalance(0, (int)(earnings * Const.AIRLINE_INCREASE_PERCENT[index]));
        //Debug.Log("Coin : " + PlayerPrefs.GetFloat("coin") + " PLUS: " + (int)(earnings * increasePercent));
        float ratio = ((Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2")) ? 2 : 1) /** Const.AIRLINE_INCREASE_PERCENT[index]*/;//ریت بدست آوردن سکه
        ratio *= ((Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m")) ? 5 : 1);
        ratio *= ((Manager.GetCurrentTime() < Manager.GetActionTime("2x_speed_for_150s")) ? 2 : 1);
        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + (int)(earnings * increasePercent * ratio * PlayerPrefs.GetFloat("incomeLine", 1)));
        //if (Manager.GetCurrentTime() < Manager.GetActionTime("income_x2"))
        //{
        //    if (Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m"))
        //    {
               
        //    }
        //    else
        //    {
        //        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + (int)(earnings * increasePercent * 2 * PlayerPrefs.GetFloat("incomeLine", 1)));
        //    }
        //    //Debug.Log("income_x2 : " + (Manager.GetActionTime("income_x2") - Manager.GetCurrentTime()));

        //}
        //else {
        //    if (Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m"))
        //    {
        //        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + (int)(earnings * increasePercent * 5 * PlayerPrefs.GetFloat("incomeLine", 1)));
        //    }
        //    else {
        //        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + (int)(earnings * increasePercent * PlayerPrefs.GetFloat("incomeLine", 1)));
        //    }
        //}

        txtCoin.text = PlayerPrefs.GetFloat("coin", 5000).ToString();
        Controller.instance.slotManager.ShowGoalAnimation();//وقتی به نقطه پایان می رسد
    }

}
