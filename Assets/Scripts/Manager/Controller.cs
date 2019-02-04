using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    public Car[] carPrefabs;//ماشین ها
    public RangeLevel[] taxiDefferenceLvl;
    public GiftBox boxPrefab;
    public Text buyPrice, txtGem, txtError, txtPanelMessage;
    public ParkingManager parkingManager;
    public RunSlotManager slotManager;
    public PlayerLevel playerLevel;
    public VideoAds videoAds;
    //public ShopDialog shop;
    //public ExchangeSpeedDialog exchangeDialog;
    public MergeCar mergeCar;
    public LevelUpBonus levelBonus;
    public GameObject deleteBin, panelMessage;
    public GameObject coinEffectPrefab;
    public OfflineEraning offEarning;
    public TrimNumberText txtCoin;
    public TrimNumberText txtToken;
    //public RubyShop rubyShop;
    public static Controller instance;


    [Header("Config Cars")]
    public string[] carName;
    public Sprite[] activeCar;
    public Sprite[] inActiveCar;
    public float[] earning;
    public float[] speed;
    public float[] basePrice; public int[] increaseRate;
    public float offlineEarningRate;

    void Awake()
    {
        //Debug.Log(
        instance = this;
        PlayerPrefs.SetInt("mainAchiv16", PlayerPrefs.GetInt("mainAchiv16", 0) + 1);
        SetText();
    }
    public void SetText()
    {
        txtToken.text = PlayerPrefs.GetFloat("token", 0).ToString();
        txtCoin.text = PlayerPrefs.GetFloat("coin", 0).ToString();
        txtGem.text = PlayerPrefs.GetFloat("gem").ToString();
    }
    // Use this for initialization
    void Start()
    {
        PlayerPrefs.SetFloat("coin", 12323564264);
        iTween.dimensionMode = iTween.DimensionMode.mode2D;//دوبعدی کردن حرکت ماشین
        if (Manager.GetCurrentTime() < Manager.GetActionTime("speed_x2"))//اگر سرعت دو برابرنیست موزیک اصلی پخش شود
        {
            //Music.instance.Play(Music.Type.MainMusic);
        }
        InitGame();
        LoadGame();
        NewCarTimeing();
    }
    private void InitGame()
    {
        parkingManager.SpawnPlaces();//پارکینگ ها را می سازد
        slotManager.InitSlots();//لاین های شروع را ایجاد می کند
        //CurrencyController.onBalanceChanged();
        UpdatePrice();
        txtCoin.text = PlayerPrefs.GetFloat("coin", 5000).ToString();//برای نمایش تعداد سکه ها در ابتدای بازی
        if (PlayerPrefs.GetInt("returned_car", 0) == 0)//اگر راهنما به پایان نرسیده بود هنوز
        {
            //guideManager.UpdateAfter(0.5f);
        }
    }
    public void UpdatePrice()
    {//قیمت ماشین ها را می گذارد
        int index = PlayerPrefs.GetInt("curr_car_index", 0);
        buyPrice.text = PlayerPrefs.GetFloat("car_price_" + index, (float)(basePrice[index] * (1 + increaseRate[index]))).ToString();
    }
    public void OnBuyClick()
    {
        //Sound.instance.Play(Sound.Others.Buy);
        int index = PlayerPrefs.GetInt("curr_car_index", 0);
        CheckAndSpawnNewCar(index, false);
        if (PlayerPrefs.GetInt("returned_car", 0) == 0)
        {
            PlayerPrefs.SetInt("buyed_car", PlayerPrefs.GetInt("buyed_car", 0) + 1);
            if (PlayerPrefs.GetInt("buyed_car", 0) == 2)
            {
                //guideManager.HideGuides();
            }
            //guideManager.UpdateAfter(1);
        }
    }
    public const int GEM_CAR_INDEX = 51;
    public void CheckAndSpawnNewCar(int index, bool hasBox)
    {
        bool useGem = index >= GEM_CAR_INDEX;//اگر عدد آیتمی که میخواد ساخته بشه بیشتر از یک مقداری بود نیاز به روبی دارد

        float price = useGem ? PlayerPrefs.GetInt("car_price_gem_" + index, (int)Mathf.Pow(2, (index - 6))) : PlayerPrefs.GetFloat("car_price_" + index, (int)(basePrice[index] * (1 + increaseRate[index])));
        Debug.Log("Price Car: " + price);
        if ((useGem ? PlayerPrefs.GetFloat("gem", 0) : PlayerPrefs.GetFloat("coin", 5000)) >= price)//روی سکه و روبی که اینجا نوشته شده است دقت شود که چه مقداری باید باشد
        {
            PlayerPrefs.SetInt("mainAchiv8", PlayerPrefs.GetInt("mainAchiv8",0)+1);
            PlayerPrefs.SetInt("mainAchiv14", PlayerPrefs.GetInt("mainAchiv14", 0) + 1);
            ParkingPlace parkPlace = parkingManager.GetEmptyPlace();
            if (parkPlace != null)
            {
                if (useGem)
                {
                    PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem", 0) - price);
                }
                else
                {
                    PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) - price);
                }
                txtCoin.text = PlayerPrefs.GetFloat("coin", 5000).ToString();
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
                    SpawnABox(index, parkPlace);
                else
                    SpawnACar(index, parkPlace);
                //}

                float newPrice = useGem ? price + 2 : (float)(price * (1 + increaseRate[index] / 100f));
                if (useGem)
                    PlayerPrefs.SetFloat("car_price_gem_" + index, newPrice);
                else
                    PlayerPrefs.SetFloat("car_price_" + index, newPrice);
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
            txtError.text = "مقدار کافی " + (useGem ? "جم " : "سکه ") + "ندارید!";
            txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                txtError.gameObject.SetActive(false);
            });
            //Debug.LogError("Not enough " + (useGem ? "gem" : "coins") + "! YourCoin: " + PlayerPrefs.GetFloat("coin", 5000));
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
            SpawnABox(index, parkPlace);
        }
    }
    public Car SpawnACar(int carIndex, ParkingPlace parkPlace, bool scaleUp = false)
    {
        Car car = Instantiate(carPrefabs[carIndex], Vector3.zero, Quaternion.identity);//یک ماشین جدید ساخته می شود با لول داده شده ساخته می شود
        car.txtCoin = txtCoin;
        car.transform.SetParent(parkPlace.transform);//پرنت در هایرارکی را پارکینگ فعلی مشخص میکند
        car.transform.localScale = Vector3.one * 0.1f;
        car.transform.position = parkPlace.transform.position;//موقعیت به موقعبت مکان فعلی تغییر میکند
        car.parkingPlace = parkPlace;//پارکنینگ را بهش میده
        if (scaleUp) car.GetComponent<Animator>().Play("MergeDone");//انیمیشن تمام شدن مرج رو ماشین جدیدی که ساخته شده انجام میشه
        PlayerPrefs.SetInt("checkLevel", 0);
        return car;//ماشین رو برمیگردونه
    }
    public GiftBox SpawnABox(int carIndex, ParkingPlace parkPlace)
    {
        GiftBox box = Instantiate(boxPrefab, Vector3.zero, Quaternion.identity);//ایجاد کرد یک باکس
        box.transform.SetParent(parkPlace.transform);//پرنت در هایرارکی مکان پارکینگ تعیین می شود
        box.transform.localScale = Vector3.one;
        box.transform.position = parkPlace.transform.position;
        box.SetUpBox(carIndex, parkPlace);
        return box;
    }
    public void NewCarTimeing()
    {
        float randomDelay = Random.Range(20, 31);
        Timer.Schedule(this, randomDelay, () =>
         {
             Debug.Log("NewCarTimeing");
             SpawnABoxTime();
             NewCarTimeing();
         });
    }
    public void SpawnABoxTime()
    {
        ParkingPlace parkPlace = parkingManager.GetEmptyPlace();
        int taxiLvl = PlayerPrefs.GetInt("unlocked_car", 1);
        int index = taxiLvl - Random.Range(taxiDefferenceLvl[taxiLvl - 1].min, taxiDefferenceLvl[taxiLvl - 1].max);
        Debug.Log("taxiLvl: " + PlayerPrefs.GetInt("unlocked_car", 1) + " UNLOCK CAR : " + index);
        if (parkPlace != null)
        {
            SpawnABox(index - 1, parkPlace);
        }
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

    public void CloseDialog()
    {
        ////Sound.instance.PlayButton();
        ////career.gameObject.SetActive(false);
        //exchangeDialog.gameObject.SetActive(false);
        //speedX2Dialog.gameObject.SetActive(false);
    }

    public void CloseShop()
    {
        ////Sound.instance.PlayButton();
        //shop.gameObject.SetActive(false);
        //parkingManager.OpenGiftBoxes();
    }

    public void CloseMergeCar()
    {
        //Sound.instance.PlayButton();
        mergeCar.gameObject.SetActive(false);
    }

    public void CloseBonus()
    {
        ////Sound.instance.PlayButton();
        //levelBonus.gameObject.SetActive(false);
    }

    public void CloseOffEarning()
    {//بستن پنل بدست آوردن سکه
        //Sound.instance.PlayButton();
        offEarning.gameObject.SetActive(false);
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
        PlayerPrefs.SetString("saved_list_cars", json);//ذخیره سازی اطلاعات داخل پلیرپرفس
    }
    public void OnApplicationPause(bool pause)
    {
        PlayerPrefs.Save();
        if (pause == false)
        {
            //Timer.Schedule(this, 0.5f, () =>
            //{
            //    CUtils.ShowInterstitialAd();
            //});
        }

        if (pause)
        {
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
        SaveGame();
        Manager.SetActionTime("offline_earning", Manager.GetCurrentTime());
    }
    public void LoadGame()
    {//اطلاعات بازی را لود می کند
        //Debug.Log("Load Game");
        string json = PlayerPrefs.GetString("saved_list_cars", "{\"listCars\":[],\"listBoxes\":[]}");//لیست ماشین ها و باکس ها داخل پارکینگ ها ذخیره شده را لود می کند
        SaveObject saveObj = JsonUtility.FromJson<SaveObject>(json);//رشته جیسون را بصورت کلاس ذخیره سازی تبدیل می کند
        if (saveObj.listCars.Count == 0)
        {
            //Debug.Log("Save Data is Empty");
            SaveObject newSaveObj = new SaveObject();
            CarObject carObj = new CarObject();
            carObj.driving = false;
            carObj.level = 1;
            carObj.parkingIndex = 1;
            newSaveObj.listCars.Add(carObj);
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
                GiftBox box = SpawnABox(boxObj.carLevel - 1, place);//یک پارکینگ با مشخصات داده شده ایجاد می کند
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
        if (time >= 60 && slotManager.EarningPerSec > 0)
        {
            offEarning.gameObject.SetActive(true);//پنل آفلاین بدست آوردن سکه را فعال می کند
            offEarning.txtCoin = txtCoin;
            offEarning.ShowEarning(time, offlineEarningRate);//با توجه به نرخی که می ذاریم مقدار سکه را زیاد می کنیم
        }
    }
    public void ShowCoinEffect(Vector3 position)
    {
        //GameObject eff = Instantiate(coinEffectPrefab, Vector3.zero, Quaternion.identity);
        //eff.transform.localScale = Vector3.one;
        //eff.transform.position = position;
    }

}
[System.Serializable]
public class RangeLevel
{
    public int min, max;
}
