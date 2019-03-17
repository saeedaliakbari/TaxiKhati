using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Batch;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class Controller : MonoBehaviour
{
    public Car[] carPrefabs;//ماشین ها
    public RangeLevel[] taxiDefferenceLvl;
    public GiftBox boxPrefab;
    public Text /*txtLevelBuyCar,*/ txtError, txtPanelMessage;
    public InputField inputFieldName;
    public Image imgBuyCar, imgSliderSplash;
    public ParkingManager parkingManager;
    public RunSlotManager slotManager;
    public PlayerLevel playerLevel;
    public AchivmentManager achivmentManager;
    public VideoAds videoAds;
    public BatchPlugin batchPlugin;
    public GuideManager guideManager;
    public SettingPanel settingPanel;
    public CafeIntent cafeIntent;
    public AudioSource audioSourceCore, audioSourceCoreFast;
    public MoveAnim moveAnimHandRun;
    //public ShopDialog shop;
    //public ExchangeSpeedDialog exchangeDialog;
    public MergeCar mergeCar;
    public SpeedPanel speedPanel;
    public LevelUpBonus levelBonus;
    public GiftPanel myGiftPanel;
    public GameObject deleteBin, panelMessage, panelShopGem, btnVip, btnGoToVipPanelMessage;
    public GameObject /*coinEffectPrefab,*/ panelSplash, panelWait, panelNoGem, objSpeed, objEarning, hand, handRun;
    public List<GameObject> coinEffect;
    public OfflineEraning offEarning;
    public Text txtGem, txtSpeed, txtEarning;
    public TrimNumberText txtCoin, txtCoinTop;
    public TrimNumberText txtToken, buyPrice;
    public Animator animIncome;
    //public RubyShop rubyShop;
    public static Controller instance;
    [HideInInspector]
    public Collider2D colliderCarHelp;

    [Header("Config Cars")]
    public string[] carName;
    public Sprite[] activeCar;
    public Sprite[] inActiveCar;
    public string[] earning;
    public float[] speed;
    public float[] basePrice; public int[] increaseRate;
    public float[] baseGemPrice;
    public int[] lastSalableLevel;
    public int[] lastSalableCoreLevel;
    public Transform XpBarTranform;///برای هدف پارتیکل میباشد
    private int def;
    private Coroutine lastRoutineSpecialBox = null, lastRoutineWheelBox = null, lastRoutineTime = null;
    void Awake()
    {
      
        instance = this;
        ObscuredPrefs.SetInt("mainAchiv16", ObscuredPrefs.GetInt("mainAchiv16", 0) + 1);
        ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 5) /*+ 1000000*/);
        ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 21000));
        ObscuredPrefs.SetDouble("coinTotal", ObscuredPrefs.GetDouble("coinTotal", 21000));
        ObscuredPrefs.SetDouble("token", ObscuredPrefs.GetDouble("token", 0) /*+ 100000000000*/);
        SetText();
    }
    public void SetText()
    {
        txtToken.text = ObscuredPrefs.GetDouble("token", 0).ToString("0.##");
        txtCoin.text = txtCoinTop.text = ObscuredPrefs.GetDouble("coin", 5000).ToString("0.##");
        txtGem.text = ObscuredPrefs.GetDouble("gem").ToString();
        videoAds.shopPanel.UpdateCarItems();
    }
    public IEnumerator IEEarningRatio()
    {
        while (EarningRatio() > 1)
        {
            EarningRatio();
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(2f);
        EarningRatio();
    }
    public float EarningRatio()
    {
        float ratio = ((Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m")) ? 5 : 0);
        ratio += ((Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m_special")) ? 5 : 0);
        ratio += ObscuredPrefs.GetFloat("incomeLine", 1);
        txtEarning.text = ratio + " برابر";
        if (ratio == 1)
        {
            objEarning.SetActive(false);
        }
        else
        {
            objEarning.SetActive(true);
        }
        return ratio;
    }
    public IEnumerator IESpeedRatio()
    {
        while (SpeedRatio() > 1)
        {
            SpeedRatio();
            if (!audioSourceCoreFast.isPlaying)
            {
                audioSourceCore.Stop();
                audioSourceCoreFast.Play();
            }
            yield return new WaitForSeconds(8f);
        }

        SpeedRatio();

        //Debug.Log("speed normal");
    }
    public float SpeedRatio()
    {
        float ratio = Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2") ? 2 : 1;
        ratio = ratio + (Manager.GetCurrentTime() < Manager.GetActionTime("2x_speed_for_150s") ? 2 : 0);
        ratio = ratio + (ObscuredPrefs.GetFloat("carsSpeedTycoon", 1) - 1);//carsSpeedBoostsTycoon
        txtSpeed.text = ratio + " برابر";
        if (ratio == 1)
        {
            objSpeed.SetActive(false);
            if (!audioSourceCore.isPlaying)
            {
                audioSourceCore.Play();
            }
            audioSourceCoreFast.Stop();
        }
        else
        {
            objSpeed.SetActive(true);
        }
        return ratio;
    }
    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ObscuredPrefs.SetInt("countCloseShop", 1);
        ConfigBatch();
        iTween.dimensionMode = iTween.DimensionMode.mode2D;//دوبعدی کردن حرکت ماشین
        if (Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2"))//اگر سرعت دو برابرنیست موزیک اصلی پخش شود
        {
            //Music.instance.Play(Music.Type.MainMusic);
        }
        InitGame();
        LoadGame();
        NewCarTimeing();
        HelpBuyCarTiming();
        HelpRunCar();
        GiftDaily();//اگر عضو بود بهش الماس روزانه میدهد
                    //Timer.Schedule(this, 5f, () =>
                    //{
        StartCoroutine(IESliderPanelSplash());
        UpdateTimeSpeed2X();// تایمر سرعت دوبرابر فعال شود
        StartCoroutine(IESpeedRatio());
        StartCoroutine(IEEarningRatio());
        GlimGames.Phone.OnCollected += Gifts_Collected;
        //}
        //);
    }

    void Gifts_Collected(int categoryIndex, int item)
    {
        //Debug.Log("category " + categoryIndex + ", item " + item);
        ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 5) + item);
        SetText();
    }
    public void OpenPanelPhone()
    {
        GlimGames.Phone.Activate();
    }
    public void ClosePanelPhone()
    {

    }
    IEnumerator IESliderPanelSplash()
    {

        DateTime time = DateTime.Now.AddSeconds(5);
        double timer = time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        while (time > DateTime.Now)
        {
            settingPanel.muteSfx.TransitionTo(1f);
            double now = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            imgSliderSplash.fillAmount = 1 - ((float)(timer - now) / 5);
            yield return new WaitForSeconds(0.1f);
        }
        if (PlayerPrefs.GetInt("SfxMute", 1) == 1)
            settingPanel.unmuteSfx.TransitionTo(0.2f);
        panelSplash.SetActive(false);
        if (ObscuredPrefs.GetInt("helpStep", 0) != 22)
        {
            //ObscuredPrefs.SetInt("helpStep", 0);
            //guideManager.panelLockGuide.SetActive(true);
            //ObscuredPrefs.SetInt("helpStep", 3);
            int level = ObscuredPrefs.GetInt("helpStep", 0);
            //Debug.Log("level Help: " + level);
            for (int i = 0; i <= level; i++)
            {
                guideManager.Step(i);
            }
        }
        else
        {
            Destroy(guideManager.panelLockGuide);
            Destroy(guideManager.gameObject);
        }
        yield return 0;
    }
    private void ConfigBatch()
    {
        batchPlugin.Push.GCMSenderID = "762423700935";
        Config config = new Config();
        //config.AndroidAPIKey = "DEV5C63FE3878D7620D2126816E614"; // dev key
        config.AndroidAPIKey = "5C63FE3878A18A81983095D989590D";// live key
        batchPlugin.Push.Setup();
        batchPlugin.StartPlugin(config);
    }
    private void InitGame()
    {
        parkingManager.SpawnPlaces();//پارکینگ ها را می سازد
        slotManager.InitSlots();//لاین های شروع را ایجاد می کند
                                //CurrencyController.onBalanceChanged();
        UpdatePrice();
    }
    public void UpdatePrice()
    {//قیمت ماشین ها را می گذارد
        int index = ObscuredPrefs.GetInt("curr_car_index", 0);//az 0 shoro mishavad
        buyPrice.text = (ObscuredPrefs.GetDouble("car_price_" + index, System.Math.Round(basePrice[index]))* (1 - ObscuredPrefs.GetFloat("offCar", 0))).ToString("0.##");
        //txtLevelBuyCar.text = "خرید ماشین سطح " + (index + 1);
        //Debug.Log("index Car : " + index + " sprite Name :" + activeCar[index].name);
        imgBuyCar.sprite = activeCar[index];
    }
    public void OnBuyClick()
    {
        //Sound.instance.Play(Sound.Others.Buy);
        int index = ObscuredPrefs.GetInt("curr_car_index", 0);
        //Debug.Log("current Car : " + index);
        CheckAndSpawnNewCar(index, false, 0);
        if (ObscuredPrefs.GetInt("returned_car", 0) == 0)
        {
            ObscuredPrefs.SetInt("buyed_car", ObscuredPrefs.GetInt("buyed_car", 0) + 1);
            if (ObscuredPrefs.GetInt("buyed_car", 0) == 2)
            {
                //guideManager.HideGuides();
            }
            //guideManager.UpdateAfter(1);
        }
    }
    public void CheckAndSpawnNewCar(int index, bool hasBox, int modelBox)
    {
        int lastSalableTaxiLevel = lastSalableLevel[ObscuredPrefs.GetInt("unlocked_car", 1) - 1];
        if (lastSalableTaxiLevel == 1)
        {
            def = 0;
        }
        else if (lastSalableTaxiLevel == 2)
        {
            def = 1;
        }
        else
        {
            def = 2;
        }
        bool useGem = index >= (lastSalableTaxiLevel - def);//اگر عدد آیتمی که میخواد ساخته بشه بیشتر از یک مقداری بود نیاز به روبی دارد

        double price = useGem ? baseGemPrice[index] : ObscuredPrefs.GetDouble("car_price_" + index, System.Math.Round(basePrice[index])) * (1 - ObscuredPrefs.GetFloat("offCar", 0));
        //Debug.Log("Price Car: " + price);
        if ((useGem ? ObscuredPrefs.GetDouble("gem", 0) : ObscuredPrefs.GetDouble("coin", 5000)) >= price)//روی سکه و روبی که اینجا نوشته شده است دقت شود که چه مقداری باید باشد
        {
            ObscuredPrefs.SetInt("mainAchiv8", ObscuredPrefs.GetInt("mainAchiv8", 0) + 1);
            ObscuredPrefs.SetInt("mainAchiv14", ObscuredPrefs.GetInt("mainAchiv14", 0) + 1);
            achivmentManager.CheckAchivments();
            ParkingPlace parkPlace = parkingManager.GetEmptyPlace();
            if (parkPlace != null)
            {
                if (useGem)
                {
                    ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 0) - price);
                }
                else
                {
                    ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) - price);
                }
                SetText();
                //if (index == 3)//این برای این است که بعضی مواقع ماشینی که خریده جدید پنل بالا بردن لول براش بیاد که که لولش بره بالا با ویدئو
                //{
                //    Debug.Log("index: " + index);
                //    videoAds.imgNowCar.sprite = mergeCar.showSprites[index];
                //    videoAds.imgUpCar.sprite = mergeCar.showSprites[index + 1];
                //    videoAds.panelTaxiUpVideo.SetActive(true);
                //    videoAds.indexCar = index;
                //}
                //else
                //{
                if (hasBox)
                    SpawnABox(index, parkPlace, modelBox);
                else
                    SpawnACar(index, parkPlace);
                //}
                //Debug.Log(price + "*((100+" + increaseRate[index] + ")/" + 100 + ")====>>>" + (price * ((100f + increaseRate[index]) / 100)) + ">>>>" + (System.Math.Round(price * ((100 + increaseRate[index]) / 100))));
                double newPrice = System.Math.Round(ObscuredPrefs.GetDouble("car_price_" + index, System.Math.Round(basePrice[index])) * ((100f + increaseRate[index]) / 100) );//قیمت جدید را بدست می آورد
                                                                                                  //Debug.Log("newPrice : " + newPrice);
                if (!useGem)
                    ObscuredPrefs.SetDouble("car_price_" + index, newPrice);

                UpdatePrice();
            }
            else
            {
                txtError.text = "پارکینگ خالی ندارید";
                txtError.gameObject.SetActive(true);
                Timer.Schedule(this, 3f, () =>
                {
                    txtError.gameObject.SetActive(false);
                });
                //Debug.LogError("No more parking space!");
                //Toast.instance.ShowMessage("No more parking space!");
            }
        }
        else
        {
            if (useGem)
            {
                parkingManager.DisableCarInPark();
                panelNoGem.SetActive(true);
            }
            else {
                txtError.text = "مقدار کافی سکه ندارید! ";
                txtError.gameObject.SetActive(true);
                Timer.Schedule(this, 3f, () =>
                {
                    txtError.gameObject.SetActive(false);
                });
            }
            //Debug.LogError("Not enough " + (useGem ? "gem" : "coins") + "! YourCoin: " + ObscuredPrefs.GetDouble("coin", 5000));
            //Toast.instance.ShowMessage("Not enough " + (useRuby ? "rubies" : "coins") + "!");
        }
    }
    public void SpawnACarWithVideo(int index)
    {
        ParkingPlace parkPlace = parkingManager.GetEmptyPlace();
        if (parkPlace == null)
        {
            txtError.text = "پارکینگ خالی ندارید";
            txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                txtError.gameObject.SetActive(false);
            });
        }
        else
        {
            SpawnABox(index, parkPlace, 2);
        }
    }
    public Car SpawnACar(int carIndex, ParkingPlace parkPlace, bool scaleUp = false)
    {
        Car car = Instantiate(carPrefabs[carIndex], Vector3.zero, Quaternion.identity);//یک ماشین جدید ساخته می شود با لول داده شده ساخته می شود
        car.txtCoin = txtCoin;
        car.controller = this;
        car.transform.SetParent(parkPlace.transform);//پرنت در هایرارکی را پارکینگ فعلی مشخص میکند
        car.transform.localScale = Vector3.one * 1f;
        car.transform.position = parkPlace.transform.position;//موقعیت به موقعبت مکان فعلی تغییر میکند
        car.parkingPlace = parkPlace;//پارکنینگ را بهش میده
        if (scaleUp) car.GetComponent<Animator>().Play("MergeDone");//انیمیشن تمام شدن مرج رو ماشین جدیدی که ساخته شده انجام میشه
        ObscuredPrefs.SetInt("checkLevel", 0);
        return car;//ماشین رو برمیگردونه
    }
    public GiftBox SpawnABox(int carIndex, ParkingPlace parkPlace, int modelBox)
    {
        ObscuredPrefs.SetInt("mainAchiv15", ObscuredPrefs.GetInt("mainAchiv15", 0) + 1);
        achivmentManager.CheckAchivments();
        GiftBox box = Instantiate(boxPrefab, Vector3.zero, Quaternion.identity);//ایجاد کرد یک باکس
        box.controller = this;
        box.transform.SetParent(parkPlace.transform);//پرنت در هایرارکی مکان پارکینگ تعیین می شود
        box.transform.localScale = Vector3.one;
        box.transform.position = parkPlace.transform.position;
        box.SetUpBox(carIndex, parkPlace, modelBox);
        if (ObscuredPrefs.GetInt("helpStep", 0) > 21)
        {
            box.StartAutoOpen();
        }
        return box;
    }
    public void NewCarTimeing()
    {
        float randomDelay = UnityEngine.Random.Range(20, 31);
        Timer.Schedule(this, randomDelay, () =>
         {
             //Debug.Log("NewCarTimeing");
             if (ObscuredPrefs.GetInt("helpStep", 0) == 22)
                 SpawnABoxTime();
         });
    }
    public void SpawnABoxTime()
    {
        ParkingPlace parkPlace = parkingManager.GetEmptyPlace();
        int taxiLvl = ObscuredPrefs.GetInt("unlocked_car", 1);
        int index = taxiLvl - UnityEngine.Random.Range(taxiDefferenceLvl[taxiLvl - 1].min, taxiDefferenceLvl[taxiLvl - 1].max);
        index = index > 0 ? index : 1;
        //Debug.Log("taxiLvl: " + ObscuredPrefs.GetInt("unlocked_car", 1) + " UNLOCK CAR : " + index);
        try
        {
            if (parkPlace.IsEmpty())
            {
                //Debug.Log("parkPlace Is Empty");
                SpawnABox(index - 1, parkPlace, 0);
                NewCarTimeing();
            }
            else
            {
                //Debug.Log("Try new Place For Car time");
                if (lastRoutineTime != null)
                {
                    StopCoroutine(lastRoutineTime);
                }
                lastRoutineTime = StartCoroutine(IESpawnABoxTime());

            }
        }
        catch (System.Exception)
        {
            //Debug.Log("Catch New Spawn");
            if (lastRoutineTime != null)
            {
                StopCoroutine(lastRoutineTime);
            }
            lastRoutineTime = StartCoroutine(IESpawnABoxTime());
        }

    }
    IEnumerator IESpawnABoxTime()
    {
        yield return new WaitForSeconds(2f);
        SpawnABoxTime();
    }
    public void SpawnABoxWheel()
    {
        ObscuredPrefs.SetInt("mainAchiv10", ObscuredPrefs.GetInt("mainAchiv10", 0) + 1);
        achivmentManager.CheckAchivments();
        ParkingPlace parkPlace = parkingManager.GetEmptyPlace();
        int taxiLvl = ObscuredPrefs.GetInt("unlocked_car", 1);
        int index = taxiLvl - 4;
        index = index > 0 ? index : 1;
        //Debug.Log("taxiLvl: " + ObscuredPrefs.GetInt("unlocked_car", 1) + " UNLOCK CAR : " + index);
        try
        {
            if (parkPlace.IsEmpty())
            {
                //Debug.Log("parkPlace Is Empty");
                SpawnABox(index - 1, parkPlace, 1);
                //NewCarTimeing();
            }
            else
            {
                //Debug.Log("Try new Place For Car time");
                if (lastRoutineWheelBox != null)
                {
                    StopCoroutine(lastRoutineWheelBox);
                }
                lastRoutineWheelBox = StartCoroutine(IESpawnABoxWheel());
            }
        }
        catch (System.Exception)
        {
            //Debug.Log("Catch New Spawn");
            if (lastRoutineWheelBox != null)
            {
                StopCoroutine(lastRoutineWheelBox);
            }
            lastRoutineWheelBox = StartCoroutine(IESpawnABoxWheel());
        }
    }
    IEnumerator IESpawnABoxWheel()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 3f));
        SpawnABoxWheel();
    }
    public void SpawnABoxSpecialOffer()
    {
        ObscuredPrefs.SetInt("mainAchiv10", ObscuredPrefs.GetInt("mainAchiv10", 0) + 1);
        achivmentManager.CheckAchivments();
        ParkingPlace parkPlace = parkingManager.GetEmptyPlace();
        int taxiLvl = ObscuredPrefs.GetInt("unlocked_car", 1);
        int index = taxiLvl - 5;
        index = index > 0 ? index : 1;
        //Debug.Log("taxiLvl: " + ObscuredPrefs.GetInt("unlocked_car", 1) + " UNLOCK CAR : " + index);
        try
        {
            if (parkPlace.IsEmpty())
            {
                //Debug.Log("parkPlace Is Empty");
                SpawnABox(index - 1, parkPlace, 1);
                //NewCarTimeing();
            }
            else
            {
                //Debug.Log("Try new Place For Car time");
                if (lastRoutineSpecialBox != null)
                {
                    StopCoroutine(lastRoutineSpecialBox);
                }
                lastRoutineSpecialBox = StartCoroutine(IESpawnABoxSpecialOffer());
            }
        }
        catch (System.Exception)
        {
            //Debug.Log("Catch New Spawn");
            if (lastRoutineSpecialBox != null)
            {
                StopCoroutine(lastRoutineSpecialBox);
            }
            lastRoutineSpecialBox = StartCoroutine(IESpawnABoxSpecialOffer());
        }
    }
    IEnumerator IESpawnABoxSpecialOffer()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 3f));
        SpawnABoxSpecialOffer();
    }
    //public void ShowCareerDialog()
    //{
    //    //Sound.instance.PlayButton();
    //    career.gameObject.SetActive(true);
    //    career.ShowDialog();
    //    //CUtils.ShowInterstitialAd();//پخش تبلیغات
    //}
    public void ShowShopDialog()
    {
        //shop.gameObject.SetActive(true);
        //shop.ShowDialog();
        ////CUtils.ShowInterstitialAd();
    }
    public void ShowExchangeDialog()
    {
        ////Sound.instance.PlayButton();
        //exchangeDialog.gameObject.SetActive(true);
        //exchangeDialog.ShowDialog();
        ////CUtils.ShowInterstitialAd();
    }

    public void ShowSpeedX2Dialog()
    {
        ////Sound.instance.PlayButton();
        //speedX2Dialog.gameObject.SetActive(true);
        //speedX2Dialog.ShowDialog();
        ////CUtils.ShowInterstitialAd();
    }

    public void ShowMergeNewCar(int fromIndex)
    {
        mergeCar.gameObject.SetActive(true);
        mergeCar.ShowMergeCar(fromIndex);
    }

    public void ShowLevelBonus(int newLevel)
    {
        StartCoroutine(IEShowBonus(newLevel));
    }

    private IEnumerator IEShowBonus(int newLevel)
    {
        while (IsDialogShowed())
        {
            yield return new WaitForSeconds(0.5f);
        }
        levelBonus.gameObject.SetActive(true);
        levelBonus.ShowLevelUpBonus(newLevel);
    }

    public bool IsDialogShowed()
    {
        return false;
        //return mergeCar.gameObject.activeSelf || shop.gameObject.activeSelf /*|| career.gameObject.activeSelf*/ ||
        //    exchangeDialog.gameObject.activeSelf || speedX2Dialog.gameObject.activeSelf || levelBonus.gameObject.activeSelf;
    }
    public void CloseOffEarning()
    {//بستن پنل بدست آوردن سکه
     //Sound.instance.PlayButton();
     //offEarning.gameObject.SetActive(false);
        Close(offEarning.gameObject);
    }
    public void CloseSetting()
    {
        if (inputFieldName.text.Length != 0)
        {
            ObscuredPrefs.SetString("username", inputFieldName.text);
        }
    }
    public void OpenSetting()
    {
        if (ObscuredPrefs.GetString("username", "") == "")
        {
            ObscuredPrefs.SetString("username", "تاکسی ران " + UnityEngine.Random.Range(100000, 999999));
        }
        inputFieldName.text = ObscuredPrefs.GetString("username", "");
    }
    public void SaveGame()
    {
        SaveObject saveObj = new SaveObject();
        List<Car> cars = new List<Car>();
        List<GiftBox> boxes = new List<GiftBox>();
        foreach (ParkingPlace place in parkingManager.places)//به ازای هر مکانی که داخل لیست پارکینگ ها داریم
        {
            if (!place.IsEmpty())//اگرکه خالی نبود
            {
                Car car = place.GetCar();//اگر داخل این پارکینگ ماشینی بود اونو برمیگردونه
                if (car != null)//اگر ماشینی برگرداند
                {
                    cars.Add(car);//به لیست ماشین ها اضافه ش کن
                }
                else
                {
                    GiftBox box = place.GetBox();//اگرماشین نبود پس جعبه است که اون جعبه رو میگریم.
                    if (box != null)//اگر جعبه خالی نبود
                    {
                        boxes.Add(box);//به لیست جعبه ها اضافه ش می کنیم.
                    }
                }
            }
        }
        foreach (Car car in cars)
        {//برای هر ماشین در لیست ماشین ها 
            CarObject carObj = new CarObject
            {
                level = car.level,
                driving = car.moving,
                parkingIndex = car.parkingPlace.GetPlaceIndex()
            };//با توجه به مشخصات هر ماشین یک آبجکت برای آن ماشین میسازیم و داخل لیست ماشین های اضافه اش می کنیم.
            saveObj.listCars.Add(carObj);
        }
        foreach (GiftBox box in boxes)
        {//به ازای هر جعبه در لیست جعبه ها
            BoxObject boxObj = new BoxObject
            {
                carLevel = box.carIndex + 1,
                parkingIndex = box.parkPlace.GetPlaceIndex()
            };
            saveObj.listBoxes.Add(boxObj);
        }
        string json = JsonUtility.ToJson(saveObj);
        ObscuredPrefs.SetString("saved_list_cars", json);//ذخیره سازی اطلاعات داخل پلیرپرفس
    }
    public void OnApplicationPause(bool pause)
    {
        ObscuredPrefs.Save();
        if (pause == false)
        {
            //Timer.Schedule(this, 0.5f, () =>
            //{
            //    CUtils.ShowInterstitialAd();
            //});
        }

        if (pause)
        {
            ObscuredPrefs.SetDouble("earnpersec", slotManager.EarningPerSec);
            SaveGame();
            Manager.SetActionTime("offline_earning", Manager.GetCurrentTime());

        }
        else
        {
            CheckAndShowOfflineEarning();
        }
    }
    private void OnApplicationQuit()
    {
        ObscuredPrefs.SetDouble("earnpersec", slotManager.EarningPerSec);
        SaveGame();
        Manager.SetActionTime("offline_earning", Manager.GetCurrentTime());
        //start local notification

    }
    public void LoadGame()
    {//اطلاعات بازی را لود می کند
     //Debug.Log("Load Game");
        string json = ObscuredPrefs.GetString("saved_list_cars", "{\"listCars\":[],\"listBoxes\":[]}");//لیست ماشین ها و باکس ها داخل پارکینگ ها ذخیره شده را لود می کند
        SaveObject saveObj = JsonUtility.FromJson<SaveObject>(json);//رشته جیسون را بصورت کلاس ذخیره سازی تبدیل می کند
        if (saveObj.listCars.Count == 0)
        {
            //Debug.Log("Save Data is Empty");
            SaveObject newSaveObj = new SaveObject();
            CarObject carObj = new CarObject();
            //carObj.driving = false;
            //carObj.level = 1;
            //carObj.parkingIndex = 1;
            //newSaveObj.listCars.Add(carObj);
            saveObj = newSaveObj;
        }
        float time = 0;
        foreach (CarObject carObj in saveObj.listCars)
        {
            ParkingPlace place = parkingManager.GetPlace(carObj.parkingIndex);//با توجه به شماره پارکینگ هر ماشین پارکینگ مورد نظر را برمیگرداند
            if (place != null)//اگر پارینگ خالی نبود
            {
                Car car = SpawnACar(carObj.level - 1, place);//یک ماشین با مشخصات داده شده ایجاد می کند
                if (carObj.driving)//اگر ماشین در حال حرکت بود
                {
                    car.StartDriveInSecond(time);//بعد از تایم مشخص حرکت را شروع کند
                    time += 0.5f;//به تایم یک مقداری اضافه می کنیم که همه همزمان شروع به حرکت نکنند و بینشون فاصله باشد
                }
            }
        }
        //بعد از اینکه کامل همه ماشین ها را در حال حرکت را گذاشت محاسبه میکنه چقدر بدست آوردن در حالت آفلاین و نشون میده
        Timer.Schedule(this, time, () =>
        {
            CheckAndShowOfflineEarning();
        });

        foreach (BoxObject boxObj in saveObj.listBoxes)
        {//به تعداد باکس های داخل لیست ذخیره شده
            ParkingPlace place = parkingManager.GetPlace(boxObj.parkingIndex);// پارکینگ مورد نظر را برمیگرداند
            if (place != null)//اگر پارکینگ خالی نبود
            {
                GiftBox box = SpawnABox(boxObj.carLevel - 1, place, 0);//یک پارکینگ با مشخصات داده شده ایجاد می کند
                box.StartAutoOpen();
            }
        }
    }
    public void CheckAndShowOfflineEarning()
    {
        int time = 0;
        if (Manager.GetActionTime("offline_earning") == 0)
        {
            time = 0;
        }
        else
        {
            time = (int)(Manager.GetCurrentTime() - Manager.GetActionTime("offline_earning"));
        }
        if (time >= 180 && slotManager.EarningPerSec > 0)
        {

            offEarning.txtCoin = txtCoin;
            offEarning.ShowEarning(time);//با توجه به نرخی که می ذاریم مقدار سکه را زیاد می کنیم
        }
    }
    public void ShowCoinEffect(Vector3 position)
    {
        animIncome.Play("IncomeCoin");

        for (int i = 0; i < 15; i++)
        {
            if (!coinEffect[i].activeSelf)
            {
                coinEffect[i].SetActive(true);
                Timer.Schedule(this, 1f, () =>
                {
                    coinEffect[i].SetActive(false);
                });
                break;
            }
        }
    }
    public void GiftDaily()
    {
        if (ObscuredPrefs.GetInt("gemPerDay", 0) == 1)
        {
            StartCoroutine(GetDateTime.IEGetDateTime((status) =>
            {
                int today = int.Parse(status.ToString("yyyyMMdd"));
                if (ObscuredPrefs.GetInt("todayDate", 19921030) < today)
                {
                    ObscuredPrefs.SetInt("todayDate", today);
                    ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") + 10);
                    panelMessage.SetActive(true);
                    txtPanelMessage.text = "10 الماس به شما اضافه شد";
                    SetText();
                }
            }));
        }
    }
    public void ClosePanelShopCar()
    {
        int random = UnityEngine.Random.Range(4, 7);
        //Debug.Log("Vasiat Tablighat removeAds: " + ObscuredPrefs.GetInt("removeAds", 0));
        if (ObscuredPrefs.GetInt("removeAds", 0) == 0)//اگر تبلیغات براش فعال نبود   
        {
            //Debug.Log("Tedad Bastan Shop : " + ObscuredPrefs.GetInt("countCloseShop", 1) + "<adad Random :" + random);
            if (ObscuredPrefs.GetInt("countCloseShop", 1) < random)//اگر کمتر از 3 بار پنل باز شده بود
            {
                ObscuredPrefs.SetInt("countCloseShop", ObscuredPrefs.GetInt("countCloseShop", 1) + 1);
            }
            else
            {
                ObscuredPrefs.SetInt("countCloseShop", 1);
                videoAds.BtnCloseShopCar();//نمایش تبلیغ بنری
            }
        }

    }
    public void Close(GameObject panel)
    {
        StartCoroutine(IEClose(panel));
    }
    public IEnumerator IEClose(GameObject panel)
    {
        yield return new WaitForSeconds((20 / 60f));
        panel.SetActive(false);
    }
    public void CloseAnimSetting(GameObject panel)
    {
        StartCoroutine(IECloseAnimSetting(panel));
    }
    public IEnumerator IECloseAnimSetting(GameObject panel)
    {
        yield return new WaitForSeconds((15 / 60f));
        panel.SetActive(false);
    }
    //public IEnumerator IEPlus(double plus, double now, Text txt)
    //{
    //    double nowPlus = 0;
    //    while (nowPlus <= plus)
    //    {
    //        nowPlus += 1;
    //        txt.text = (now + nowPlus).ToString();
    //        yield return new WaitForSeconds(0.1F);
    //    }
    //    txt.text=ObscuredDouble.
    //}
    public void UpdateTimeSpeed2X()
    {
        float timeValue = Mathf.Max(0, (float)(Math.Round(Manager.GetActionTime("speed_x2") - Manager.GetCurrentTime())));
        if (timeValue > 1800)
        {
            Manager.SetActionTime("speed_x2", (Manager.GetActionTime("speed_x2") - (timeValue - 1800)));
            timeValue = 1800;
        }
        if (speedPanel.lastRoutinenu != null)
        {
            StopCoroutine(speedPanel.lastRoutinenu);
        }
        speedPanel.lastRoutinenu = StartCoroutine(IETimerSpeed2X(timeValue));
    }

    private IEnumerator IETimerSpeed2X(float timeValue)
    {
        while (Manager.GetActionTime("speed_x2") > Manager.GetCurrentTime())
        {
            TimeSpan t = TimeSpan.FromSeconds(timeValue);
            speedPanel.txtTimer.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            float percent = Mathf.Min(1, timeValue / (150 * 12));
            speedPanel.imgProgress.fillAmount = percent;
            yield return new WaitForSecondsRealtime(1f);
            timeValue--;
            Manager.SetActionTime("speed_x2", (Manager.GetActionTime("speed_x2") - 1));
        }
        speedPanel.imgProgress.fillAmount = 0;
        speedPanel.txtTimer.text = "00:00";
        yield return 0;
    }
    public void HelpBuyCarTiming()
    {
        Manager.SetActionTime("helpBuyCar", Manager.GetCurrentTime() + 30);
        if (ObscuredPrefs.GetInt("helpStep", 0) == 22)
            StartCoroutine(IEHelpBuyCarTiming());
    }
    private IEnumerator IEHelpBuyCarTiming()
    {
        while (Manager.GetCurrentTime() < Manager.GetActionTime("helpBuyCar"))
        {
            //Debug.Log("Help Buy Car timing: " + Manager.GetCurrentTime() + " / " + Manager.GetActionTime("helpBuyCar"));
            yield return new WaitForSeconds(1f);
        }
        for (int i = 0; i < parkingManager.places.Count; i++)
        {
            if (parkingManager.places[i].GetCar() == null)
            {
                int index = ObscuredPrefs.GetInt("curr_car_index", 0);
                if (ObscuredPrefs.GetDouble("coin", 21000) >= ObscuredPrefs.GetDouble("car_price_" + index, System.Math.Round(basePrice[index])))
                {
                    hand.SetActive(true);
                    break;
                }
            }
        }
        HelpBuyCarTiming();
    }
    public void HelpRunCar()
    {
        Manager.SetActionTime("helpRunCar", Manager.GetCurrentTime() + 40);
        if (ObscuredPrefs.GetInt("helpStep", 0) == 22)
            StartCoroutine(IEHelpRunCar());
    }
    private IEnumerator IEHelpRunCar()
    {
        while (Manager.GetCurrentTime() < Manager.GetActionTime("helpRunCar"))
        {
            yield return new WaitForSeconds(1f);
        }
        if (!slotManager.IsFull())
        {
            for (int i = 0; i < parkingManager.places.Count; i++)
            {
                if (parkingManager.places[i].GetCar() != null)
                {
                    if (!parkingManager.places[i].GetCar().moving)
                    {
                        moveAnimHandRun.startTarget = parkingManager.places[i].transform;
                        handRun.SetActive(true);
                        break;
                    }
                }
            }
        }
        HelpRunCar();
    }
    public void PlusTimeHelp()
    {
        Manager.SetActionTime("helpRunCar", Manager.GetCurrentTime() + 40);
        Manager.SetActionTime("helpBuyCar", Manager.GetCurrentTime() + 30);
        handRun.SetActive(false);
        hand.SetActive(false);
    }
}

[System.Serializable]
public class RangeLevel
{
    public int min, max;
}
