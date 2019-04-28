using CodeStage.AntiCheat.ObscuredTypes;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//کلاس ماشین پارک شده داخل پارکینگ
public class Car : MonoBehaviour
{
    public int level;
    public int xp;
    public double earnings;
    public float speed;
    [HideInInspector]
    public TrimNumberText txtCoin;
    [HideInInspector]
    public bool moving = false;
    [HideInInspector]
    public ParkingPlace parkingPlace;
    [HideInInspector]
    public Controller controller;
    public MoveCar moveCarPrefab;
    public AudioSource audioSource;
    public AudioMixerGroup audioMixerMaster, audioMixerKhatePayan;
    public AudioClip khatePayan, trash, carPlacement, combindCar;
    private MoveCar moveCar;
    private const float timeLab = 19.39939f;
    private void OnMouseDrag()//کشیدن  ماشین
    {
        //Debug.Log("On Mouse Drag");
        //if (Homecontroller.IsDialogShowed()) return;//اگر پنلی فعال باشد در صفحه امکان درگ کردن نباشد
        if (!moving)//اگر ماشبن در حال حرکت نبود
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPos;
            GetComponent<SpriteRenderer>().sortingOrder = 21;//برای اینکه روی بقیه آبجکت ها قرار گیرد
        }
    }
    private void OnMouseDown()
    {
        if (!moving)
        {
            for (int i = 0; i < controller.parkingManager.places.Count; i++)
            {
                if (controller.parkingManager.places[i].GetCar() != null)
                {
                    if (controller.parkingManager.places[i].GetCar().level == level)
                    {
                        if (controller.parkingManager.places[i].GetCar().moving == false)
                        {
                            if (controller.parkingManager.places[i] != parkingPlace)
                            {
                                controller.parkingManager.places[i].helpPlace.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }
    private void OnMouseUp()//کشیدن را تمام کرد
    {
        Manager.SetActionTime("helpBuyCar", Manager.GetCurrentTime() + 30);
        Manager.SetActionTime("helpRunCar", Manager.GetCurrentTime() + 40);
        for (int i = 0; i < controller.parkingManager.places.Count; i++)
        {
            controller.parkingManager.places[i].helpPlace.SetActive(false);
        }
        //Debug.Log("On Mouse Up "+moving+":>>"+ controller.IsDialogShowed());
        if (controller.IsDialogShowed()) return;//اگر پنلی فعال باشد در صفحه امکان درگ کردن نباشد

        if (!moving)//اگر ماشین درحال حرکت نبود
        {
            controller.handRun.SetActive(false);
            controller.hand.SetActive(false);
            ParkingPlace nearPlace = controller.parkingManager.GetNearestPlace(transform.position);//نزدیکترین مکان 
            //Debug.Log("!moving");
            if (nearPlace != null && nearPlace != parkingPlace)//اگر نزدیکترین مکان خالی نبود و با مکان فعلی یکسان نبود
            {
                parkingPlace.objBack.SetActive(false);
                if (!nearPlace.IsEmpty())//اگر نزدیکترین مکان خالی نبود
                {
                    Car car2 = nearPlace.GetCar();
                    if (car2 != null && !car2.moving)//اگر ماشین داخل پارکینگ نزدیک بود و داخل پارکینگ پارک بود
                    {
                        transform.position = nearPlace.transform.position;
                        if (level == car2.level && level < controller.carPrefabs.Length)//اگر لول دوتا ماشین یکی بود و لول از تعداد ماشین ها کمتر با هم مرج شوند
                        {//Merge
                            controller.guideManager.MergeStep();
                            //Homecontroller.guideManager.HideGsuides();
                            //Homecontroller.guideManager.UpdateAfter(1);
                            ObscuredPrefs.SetInt("mergeCarForVideo", ObscuredPrefs.GetInt("mergeCarForVideo", 1) + 1);
                            GetComponent<Animator>().Play("Merge");//انیمیشن مرج اجرا شود

                            GetComponent<SpriteRenderer>().enabled = false;
                            car2.gameObject.SetActive(false);
                            int unlockedLevel = ObscuredPrefs.GetInt("unlocked_car", 1);
                            //بعد از نیم ثانیه کارهای زیر را انجام بده
                            Timer.Schedule(this, 0.5f, (Timer.Task)(() =>
                            {
                                controller.SpawnACar((int)level, (ParkingPlace)nearPlace, (bool)true);//ماشین جدید ساخته می شه به جای ماشین که تاچ شه
                                ObscuredPrefs.SetInt("checkLevel", 0);
                                if (level + 1 > unlockedLevel)
                                {//اگر لول بیشتر از لول ماشین ماکس باشد
                                    if (((unlockedLevel + 1) % 7 == 0))
                                    {
                                        controller.cafeIntent.panelComment.SetActive(true);
                                        controller.parkingManager.DisableCarInPark();
                                    }
                                }
                                nearPlace.animLight.Play("Merge");
                                Destroy(car2.gameObject);//ماشین دومی را از بین می بریم که یک ماشین باقی بماند
                                Destroy(gameObject);//ماشین فعلی از بین میره
                            }));
                            if (level + 1 > unlockedLevel)
                            {//اگر لول بیشتر از لول ماشین ماکس باشد
                                controller.guideManager.MergeStep2();
                                ObscuredPrefs.SetInt("unlocked_car", level + 1);
                                GameAnalytics.NewDesignEvent("Car Level:" + ObscuredPrefs.GetInt("unlocked_car", 1));
                                ObscuredPrefs.SetInt("curr_car_index", controller.lastSalableCoreLevel[ObscuredPrefs.GetInt("unlocked_car", 1) - 1] - 1);
                                controller.ShowMergeNewCar(level - 1);//در صورتی که ماشین جدید باز شود پنل مرج باز می شود که مرج انجام می شود
                                controller.UpdatePrice();//برای اینکه باتن کور آپدیت شود
                                controller.internetStorageSpace.SaveData(false);
                            }
                            else {//در صورتی که ماشین لول جدید باز نشود با مرج و ماشین های قبلی تولید شود وقت صدای مرج پخش می شود
                                audioSource.outputAudioMixerGroup = audioMixerMaster;
                                audioSource.clip = combindCar;
                                audioSource.Play();
                            }
                            //باید مقدار افزوده شدن ایکس پی اضافه شود
                            //Debug.Log("XP: " + ObscuredPrefs.GetInt("Xp", 0));
                            ObscuredPrefs.SetInt("Xp", ObscuredPrefs.GetInt("Xp", 0) + xp);
                            nearPlace.XPTrail.target = controller.XpBarTranform;
                            nearPlace.XPTrail.MyGameObject.SetActive(true);
                            //Debug.Log("XP: " + ObscuredPrefs.GetInt("Xp", 0));
                            controller.playerLevel.UpdateProgress(xp);//مقدار ایکس و لول ست شود
                        }
                        else
                        {//دوتاماشین با هم جابه جا می شوند
                            audioSource.outputAudioMixerGroup = audioMixerMaster;
                            audioSource.clip = carPlacement;
                            audioSource.Play();
                            ParkingPlace lastPlace = parkingPlace;//ماشین اول در جایگاه ماشین دوم قرار میگیرد
                            parkingPlace = nearPlace;
                            transform.SetParent(nearPlace.transform);
                            car2.MoveToPlace(lastPlace);//ماشین دوم به سمت جایگاه ماشین اول می رود
                        }
                    }
                    else//اگر ماشین داخل پارکینگ نزدیک نباشه یا در حال حرکت باشد
                    {
                        nearPlace.objBack.SetActive(true);
                        transform.position = parkingPlace.transform.position;//ماشین جابه جا نمی شود و به مکان اولیه برمیگردد
                        //Sound.instance.Play(Sound.Others.Unswap);
                    }
                }
                else//جابجایی به مکان جدید
                {
                    audioSource.outputAudioMixerGroup = audioMixerMaster;
                    audioSource.clip = carPlacement;
                    audioSource.Play();
                    transform.position = nearPlace.transform.position;//موقعیت ماشین را به موقعیت نزدیکترین مکان که خالی هم هست تغییر می دهد
                    parkingPlace = nearPlace;//پارکینگش را عوض می کند
                    transform.SetParent(nearPlace.transform);//داخل هایرارکی فرزند آبجکت پارکینگ جدید شد
                }
            }
            else
            {//اگر نزدیکترین مکانی تشخیص نداد یا اینکه ان مکان با مکان فعلی یکسان بود
                if (!controller.slotManager.IsFull() /*جایگاه های استارت پر نباشد*/&& (Mathf.Abs(transform.position.x - controller.slotManager.transform.position.x) < 0.5f && Mathf.Abs(transform.position.y - controller.slotManager.transform.position.y) < 2f) /*Vector3.Distance(transform.position, controller.slotManager.transform.position) < 0.8f*//*فاصله اش تا جایگاه استارت کمتر از 0.5 باشد*/)
                {//اگر گذاشته شود در نقطه استارت
                    //Debug.Log("parking : " + parkingPlace.objBack.activeSelf);
                    parkingPlace.objBack.SetActive(true);
                    //Debug.Log("parking : " + parkingPlace.objBack.activeSelf);
                    StartDrive();//شروع پرواز
                    //Sound.instance.Play(Sound.Others.Start);
                }
                else if (Vector3.Distance(transform.position, controller.deleteBin.transform.position) < 0.5f)
                {//اگر آن ماشین را حذف کند
                    parkingPlace.objBack.SetActive(false);
                    DismantleCar();
                }
                else
                {//در مکان اولیه قرار می گیردوهیچ تغییری اعمال نمی شود
                    parkingPlace.objBack.SetActive(false);
                    transform.position = parkingPlace.transform.position;
                }
            }
        }
        else//اگر ماشین در حال حرکت بود
        {
            parkingPlace.objBack.SetActive(false);
            //Debug.Log("Not Move" + moveCar.returning);
            if (!moveCar.returning)
            {
                controller.guideManager.ReturnCar();
                moveCar.Return();//به همون مکان برگردد
                controller.slotManager.StopACar(GetEarningPerSecond());
            }
        }
        GetComponent<SpriteRenderer>().sortingOrder = 15;
        ObscuredPrefs.SetInt("checkLevel", 0);
    }
    private void DismantleCar()
    {
        audioSource.outputAudioMixerGroup = audioMixerMaster;
        audioSource.clip = trash;
        audioSource.Play();
        //CurrencyController.CreditBalance(0, Mathf.RoundToInt(Superpow.Utils.GetPrice(level - 1) * 0.5f));
        //Debug.Log("base{rice : " + controller.basePrice[level - 1]);
        ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + controller.basePrice[level - 1]);
        controller.SetText();
        controller.ShowCoinEffect(controller.deleteBin.transform.position);//در موقعیت سطل آشغال یک افکت سکه ایجاد می کند
        ObscuredPrefs.SetInt("checkLevel", 0);
        Destroy(gameObject);//این ماشین را از بین می برد
    }
    public double GetEarningPerSecond()//مقدار بدست آوردن سکه در هر ثانیه
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
        controller.hand.SetActive(false);
        controller.handRun.SetActive(false);
        parkingPlace.objBack.SetActive(true);
        controller.colliderCarHelp = GetComponent<Collider2D>();
        controller.guideManager.StartDrive();
        moving = true;
        moveCar = SpawnACar();
        moveCar.transform.position = controller.slotManager.transform.position;//موقعیت ماشین درحال حرکت در مکان استارت قرار میگیرد
        moveCar.DiverARound();//حرکت ماشین 
        transform.position = parkingPlace.transform.position;//موقعیت ماشین ایستاده با پارکینگ پر می شود
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);//رنگ را کم رنگتر می کند
        controller.slotManager.RunACar(GetEarningPerSecond());//یک ماشین در حال حرکت ایجاد می کند با مقدار سکه اضافه شده
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
        MoveCar mCar = (MoveCar)Instantiate(moveCarPrefab, Vector3.zero, Quaternion.EulerAngles(0f, 0f, 0f)/*, Quaternion.identity*/);
        mCar.transform.localScale = Vector3.one * 0.45f;//اسکیل ماشین در حال حرکت ایجاد شده
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
        try
        {
            audioSource.outputAudioMixerGroup = audioMixerKhatePayan;
            audioSource.clip = khatePayan;
            audioSource.Play();
            float ratio = controller.EarningRatio();
            ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + (double)(earnings * ratio));
            ObscuredPrefs.SetDouble("coinTotal", ObscuredPrefs.GetDouble("coinTotal", 5000) + (double)(earnings * ratio));
            controller.SetText();
            controller.slotManager.ShowGoalAnimation();//وقتی به نقطه پایان می رسد
        }
        catch
        {
            Debug.Log("Error in Finish Round");
        }

    }

}
