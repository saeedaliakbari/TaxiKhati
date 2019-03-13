using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ParkingManager : MonoBehaviour
{

    public ParkingPosManage[] parkPosManage;
    [HideInInspector]
    public List<ParkingPlace> places = new List<ParkingPlace>();//لیست پارکینگ ها
    [HideInInspector]
    public List<GameObject> placesPosition = new List<GameObject>();//لیست پارکینگ ها
    public ParkingPlace placePrefab;
    public ParkingPlaceVIP placeVIP;
    public Controller controller;
    private bool VipPlace = false;
    public CarSpeedTycoonBoosts carSpeedTycoonBoosts;
    public EarningOfflineTycoonBoosts earningOfflineTycoonBoosts;
    public ExchangeDeclineTycoonBoosts exchangeDeclineTycoonBoosts;
    public Button btnCarSpeed, btnOfflineEarning, btnExchangeDecline;
    public Image imgPeykanOfflineEarn, imgCarOfflienEarn, imgPeykanSpeed, imgCarSpeed, imgPeykanExchangeDecline, imgCarExchangeDecline;
    public Text txtCarSpeed, txtSpeedNew, txtSpeedOld, txtLevelSpeed, txtOfflineEarning, txtOfflineEarnNew, txtOfflineEarnOld, txtLevelOfflineEarn, txtExchangeDecline, txtExchangeDeclineNew, txtExchangeDeclineOld, txtLevelExchangeDecline;
    public GameObject objCarOfflineEarn, objCarSpeed, objCarExchangeDecline, lblNewOffline, lblNewSpeed, lblNewExchangeDecline;
    public void SpawnPlaces()
    {//با توجه به تعداد مکان ها ، پارکینگ ها را ایجاد می کند
        int numPlaces = ObscuredPrefs.GetInt("num_of_places", 4);
        int numPlacesVIP = ObscuredPrefs.GetInt("num_of_places_vip", 0);

        ObscuredPrefs.SetInt("num_total_places", numPlaces + numPlacesVIP);
        for (int i = 0; i < numPlaces + numPlacesVIP; i++)
        {
            SpawnNewPlace();
        }
        UpdatePlacePosition();
    }
    public void SpawnPlacesVIP()
    {
        int num = ObscuredPrefs.GetInt("num_of_places", 4);//تعداد لاین های شروع 
        num += ObscuredPrefs.GetInt("num_of_places_vip", 0);
        Debug.Log("Place>>>num: " + ObscuredPrefs.GetInt("num_of_places", 4) + " numVIP: " + ObscuredPrefs.GetInt("num_of_places_vip", 0) + " TOTAL: " + ObscuredPrefs.GetInt("num_total_places"));
        if (num > ObscuredPrefs.GetInt("num_total_places") && ObscuredPrefs.GetInt("num_total_places") < 16)
        {
            Debug.Log("num > num_total_places");
            int newPlace = num - ObscuredPrefs.GetInt("num_total_places");
            for (int i = 0; i < newPlace; i++)
            {
                ObscuredPrefs.SetInt("num_total_places", ObscuredPrefs.GetInt("num_total_places") + 1);
                SpawnNewPlace();
            }
            UpdatePlacePosition();
        }
    }
    void FixedUpdate()//برای چک کردن تعداد ماشین های هر لول در پارکینگ ها
    {
        if (ObscuredPrefs.GetInt("checkLevel", 0) == 0)
        {
            PrintLevelsCarsInPark();
        }
    }
    private void PrintLevelsCarsInPark()
    {
        int[] carsLevel = new int[50];
        ObscuredPrefs.SetInt("checkLevel", 1);
        Timer.Schedule(this, 0.5f, (Timer.Task)(() =>
        {
            btnCarSpeed.interactable = false;
            btnExchangeDecline.interactable = false;
            btnOfflineEarning.interactable = false;
            for (int i = 0; i < places.Count; i++)
            {
                if (!places[i].IsEmpty() && places[i].GetBox() == null)
                {
                    carsLevel[places[i].GetCar().level - 1] += 1;
                }
            }
            //for (int i = 0; i < carsLevel.Length; i++)
            //{
            //    Debug.Log(i + ">" + carsLevel[i]);
            //}
            ObscuredPrefs.SetInt("mainAchiv1", carsLevel[4]);
            ObscuredPrefs.SetInt("mainAchiv2", carsLevel[6]);
            ObscuredPrefs.SetInt("mainAchiv3", carsLevel[10]);
            ObscuredPrefs.SetInt("mainAchiv6", carsLevel[14]);
            ObscuredPrefs.SetInt("mainAchiv7", carsLevel[24]);
            controller.achivmentManager.CheckAchivments();
            CheckCarSpeedTycoon(carsLevel);
            CheckOfflineEarningTycoon(carsLevel);
            CheckExchangeRateDecline(carsLevel);
        }));
    }
    public void OpenPanelTycoon()
    {
        PrintLevelsCarsInPark();
        for (int i = 0; i < placesPosition.Count; i++)
        {
            placesPosition[i].SetActive(false);
        }
    }
    public void ClosePanelTycoon()
    {
        for (int i = 0; i < placesPosition.Count; i++)
        {
            placesPosition[i].SetActive(true);
        }
    }
    private void CheckCarSpeedTycoon(int[] carsLevel)
    {
        lblNewSpeed.SetActive(false);
        int levelSpeed = ObscuredPrefs.GetInt("carSpeedTycoonLevel", 0);
        txtCarSpeed.text = "سطح " + (levelSpeed + 1);
        imgCarSpeed.sprite = controller.activeCar[carSpeedTycoonBoosts.level[levelSpeed] - 1];
        txtLevelSpeed.text = (carsLevel[carSpeedTycoonBoosts.level[levelSpeed] - 1] >= 3 ? "3" :
            carsLevel[carSpeedTycoonBoosts.level[levelSpeed] - 1].ToString()) + "/3";
        txtSpeedOld.text = levelSpeed >= 1 ? ((Mathf.RoundToInt((carSpeedTycoonBoosts.incSpeed[levelSpeed - 1] - 1f) * 1000)) / 10f).ToString() + "%" : "0%";
        txtSpeedNew.text = "+" + ((Mathf.RoundToInt((carSpeedTycoonBoosts.incSpeed[levelSpeed] - 1f) * 1000)) / 10f).ToString() + "%";
        //imgSlideGreenOfflineEarning.fillAmount = (earningOfflineTycoonBoosts.incEarn[levelOfflineEarning] - 1f) / 0.325f;
        if (levelSpeed > 12)
        {
            txtSpeedNew.gameObject.SetActive(false);
            imgPeykanSpeed.gameObject.SetActive(false);
            btnCarSpeed.gameObject.SetActive(false);
            lblNewSpeed.SetActive(false);
            //Debug.Log("FULL ");
        }
        else {
            if (carsLevel[carSpeedTycoonBoosts.level[levelSpeed] - 1] >= 3)
            {
                //Debug.Log("Get Gift" + earningOfflineTycoonBoosts.level[ObscuredPrefs.GetInt("offlineEarnTycoonLevel", 0)]);
                btnCarSpeed.interactable = true;
                lblNewSpeed.SetActive(true);
            }
        }
    }
    public void GetGiftCarSpeed()
    {
        btnCarSpeed.interactable = false;
        ObscuredPrefs.SetFloat("carsSpeedTycoon", carSpeedTycoonBoosts.incSpeed[ObscuredPrefs.GetInt("carSpeedTycoonLevel", 0)]);
        ObscuredPrefs.SetInt("carSpeedTycoonLevel", ObscuredPrefs.GetInt("carSpeedTycoonLevel", 0) + 1);
        //txtCarSpeed.text = "Level Car Speed " + (ObscuredPrefs.GetInt("carSpeedTycoonLevel", 0) + 1);
        StartCoroutine(controller.IESpeedRatio());
        PrintLevelsCarsInPark();
    }
    private void CheckOfflineEarningTycoon(int[] carsLevel)
    {
        lblNewOffline.SetActive(false);
        int levelOfflineEarning = ObscuredPrefs.GetInt("offlineEarnTycoonLevel", 0);
        txtOfflineEarning.text = "سطح " + (levelOfflineEarning + 1);
        //Debug.Log("CheckOfflineEarningTycoon LEVEL CAR :" + earningOfflineTycoonBoosts.level[levelOfflineEarning]);
        imgCarOfflienEarn.sprite = controller.activeCar[earningOfflineTycoonBoosts.level[levelOfflineEarning] - 1];
        txtLevelOfflineEarn.text = (carsLevel[earningOfflineTycoonBoosts.level[levelOfflineEarning] - 1] >= 3 ? "3" :
            carsLevel[earningOfflineTycoonBoosts.level[levelOfflineEarning] - 1].ToString()) + "/3";
        txtOfflineEarnOld.text = levelOfflineEarning >= 1 ? ((Mathf.RoundToInt((earningOfflineTycoonBoosts.incEarn[levelOfflineEarning - 1] - 1f) * 1000)) / 10f).ToString() + "%" : "0%";
        txtOfflineEarnNew.text = "+" + ((Mathf.RoundToInt((earningOfflineTycoonBoosts.incEarn[levelOfflineEarning] - 1f) * 1000)) / 10f).ToString() + "%";
        //imgSlideGreenOfflineEarning.fillAmount = (earningOfflineTycoonBoosts.incEarn[levelOfflineEarning] - 1f) / 0.325f;
        if (levelOfflineEarning > 12)
        {
            txtOfflineEarnNew.gameObject.SetActive(false);
            imgPeykanOfflineEarn.gameObject.SetActive(false);
            btnOfflineEarning.gameObject.SetActive(false);
            lblNewOffline.SetActive(false);
            //Debug.Log("FULL ");
        }
        else {
            if (carsLevel[earningOfflineTycoonBoosts.level[levelOfflineEarning] - 1] >= 3)
            {
                //Debug.Log("Get Gift" + earningOfflineTycoonBoosts.level[ObscuredPrefs.GetInt("offlineEarnTycoonLevel", 0)]);
                btnOfflineEarning.interactable = true;
                lblNewOffline.SetActive(true);
            }
        }
    }
    public void GetGiftOfflineEran()
    {
        btnOfflineEarning.interactable = false;
        ObscuredPrefs.SetFloat("offlineEarnTycoonBoosts", earningOfflineTycoonBoosts.incEarn[ObscuredPrefs.GetInt("offlineEarnTycoonLevel", 0)]);
        ObscuredPrefs.SetInt("offlineEarnTycoonLevel", ObscuredPrefs.GetInt("offlineEarnTycoonLevel", 0) + 1);
        //txtOfflineEarning.text = "Level Offline Earn " + (ObscuredPrefs.GetInt("offlineEarnTycoonLevel", 0) + 1);
        PrintLevelsCarsInPark();
    }
    private void CheckExchangeRateDecline(int[] carsLevel)
    {
        lblNewExchangeDecline.SetActive(false);
        int levelExchangeDecline = ObscuredPrefs.GetInt("exchangeDeclineTycoonLevel", 0);
        txtExchangeDecline.text = "سطح " + (levelExchangeDecline + 1);
        //Debug.Log("CheckExchangeRateDecline LEVEL CAR :" + exchangeDeclineTycoonBoosts.level[levelExchangeDecline]);
        imgCarExchangeDecline.sprite = controller.activeCar[exchangeDeclineTycoonBoosts.level[levelExchangeDecline] - 1];
        txtLevelExchangeDecline.text = (carsLevel[exchangeDeclineTycoonBoosts.level[levelExchangeDecline] - 1] >= 3 ? "3" :
            carsLevel[exchangeDeclineTycoonBoosts.level[levelExchangeDecline] - 1].ToString()) + "/3";
        txtExchangeDeclineOld.text = levelExchangeDecline >= 1 ? (Mathf.RoundToInt((exchangeDeclineTycoonBoosts.rateDecline[levelExchangeDecline - 1]) * 100)).ToString() + "%" : "0%";
        txtExchangeDeclineNew.text = "+" + (Mathf.RoundToInt(exchangeDeclineTycoonBoosts.rateDecline[levelExchangeDecline] * 100f)).ToString() + "%";
        //imgSlideGreenOfflineEarning.fillAmount = (earningOfflineTycoonBoosts.incEarn[levelOfflineEarning] - 1f) / 0.325f;
        if (levelExchangeDecline > 12)
        {
            txtExchangeDeclineNew.gameObject.SetActive(false);
            imgPeykanExchangeDecline.gameObject.SetActive(false);
            btnExchangeDecline.gameObject.SetActive(false);
            //Debug.Log("FULL ");
            lblNewExchangeDecline.SetActive(false);
        }
        else {
            if (carsLevel[exchangeDeclineTycoonBoosts.level[levelExchangeDecline] - 1] >= 3)
            {
                //Debug.Log("Get Gift" + earningOfflineTycoonBoosts.level[ObscuredPrefs.GetInt("offlineEarnTycoonLevel", 0)]);
                btnExchangeDecline.interactable = true;
                lblNewExchangeDecline.SetActive(true);
            }
        }
    }
    public void GetGiftExchangeRateDecline()
    {
        btnExchangeDecline.interactable = false;
        ObscuredPrefs.SetFloat("exchangeDeclineTycoon", exchangeDeclineTycoonBoosts.rateDecline[ObscuredPrefs.GetInt("ExchangeDeclineTycoonLevel", 0)]);
        ObscuredPrefs.SetInt("exchangeDeclineTycoonLevel", ObscuredPrefs.GetInt("exchangeDeclineTycoonLevel", 0) + 1);
        //txtExchangeDecline.text = "Level Offline Earn " + (ObscuredPrefs.GetInt("exchangeDeclineTycoonLevel", 0) + 1);
        PrintLevelsCarsInPark();
    }
    public void SpawnNewPlaceVIP()
    {//ایجاد پارکینگ جدید

        ParkingPlaceVIP place = (ParkingPlaceVIP)Instantiate(placeVIP, Vector3.zero, Quaternion.identity);
        place.transform.SetParent(transform);
        place.transform.localScale = Vector3.one * 0.6f;
        place.controller = controller;
        placesPosition.Add(place.gameObject);
        VipPlace = true;
    }
    private void DeleteVipPlace()
    {
        if (VipPlace == true)
        {
            //Debug.Log("delete Vip Place");
            Destroy(placesPosition[placesPosition.Count - 1]);
            placesPosition.RemoveAt(placesPosition.Count - 1);
            VipPlace = false;
        }
    }
    public void SpawnNewPlace()
    {//ایجاد پارکینگ جدید
        DeleteVipPlace();
        ParkingPlace place = (ParkingPlace)Instantiate(placePrefab, Vector3.zero, Quaternion.identity);
        if (ObscuredPrefs.GetInt("helpStep", 0) != 22)
        {
            controller.guideManager.parkPlace.Add(place);
        }
        //place.gameObject.name = "Place" + Random.RandomRange(0, 100000);
        place.transform.SetParent(transform);
        place.transform.localScale = Vector3.one * 0.6f;
        places.Add(place);//به لیست پارکینگ ها اضافه ش میکنیم
        placesPosition.Add(place.gameObject);
        int numPlacesVIP = ObscuredPrefs.GetInt("num_of_places_vip", 0);
        if (numPlacesVIP == 0 && places.Count > 4 && VipPlace == false)
        {
            //Debug.Log("Spawn New PlaceVIP>>" + numPlacesVIP);
            SpawnNewPlaceVIP();
        }
    }

    public ParkingPlace GetPlace(int index)
    {//با توجه به شماره ، پارکینگ مورد نظر را برمیگرداند
        if (index < places.Count)
            return places[index];
        return null;
    }
    public void UpdatePlacePosition()
    {//موقعیت پارکینگ ها را تعیین می کند
        int num = placesPosition.Count;
        //Debug.Log("Tedad Parkings : " + placesPosition.Count);
        int count = 0;
        for (int i = 0; i < parkPosManage.Length; i++)
        {
            if (num == parkPosManage[i].Tedad)
            {
                int rows = parkPosManage[i].RowColumns.Length;
                for (int r = 0; r < rows; r++)//rows=parkPosManage[i].RowColumns.Length
                {
                    int column = parkPosManage[i].RowColumns[r];
                    for (int c = 0; c < column; c++)
                    {
                        Vector3 pos = new Vector3((c - (column - 1f) / 2) * 1.3f, (-(r - (rows - 1f) / 2)) * 1.1f);
                        placesPosition[count].transform.localPosition = pos /** 1.2f*/;
                        count++;
                    }
                }
            }
        }
        //int cols = num < 11 ? 2 : 3;//اگر تعداد مکان ها کمتر از 11تا بود دو ستون باشه و اگرکه بیشتر شد 3 تا ستون بشه
        //int rows = num / cols;//تعداد ردیف ها هم باتوجه به تعداد پارکینگ ها و تعداد ستون ها ساخته می شود
        //if (cols * rows < num) rows++;//اگر تعداد پارکینگ ها کمتر از تعداد باشد یکی به تعداد سطر ها اضافه می کند
        //int count = 0;
        //for (int r = 0; r < rows; r++)
        //{//سطر به سطر جلو می رود
        //    int column = (r == rows - 1) ? num % cols : cols;//در مواقعی که کامل ستون های یک سطر را پر نمیکند تعداد پارکینگها پس تعداد ستون های اون سطر را کمتر در نظر می گیرد
        //    if (column == 0) column = cols;
        //    for (int c = 0; c < cols; c++)
        //    {//ستون به ستون جلو می رود
        //        Vector3 pos = new Vector3(c - (column - 1f) / 2, -(r - (rows - 1f) / 2));
        //        Debug.Log("pos: " + pos + " r: " + r + " c: " + c);
        //        if (count == num)
        //        {
        //            break;
        //        }
        //        placesPosition[count].transform.localPosition = pos * 1.3f;
        //        Debug.Log("placesPosition>: " + placesPosition[count].transform.localPosition);
        //        count++;
        //    }
        //}
    }
    public ParkingPlace GetEmptyPlace()
    {//مکان های خالی را برمی گرداند
        foreach (ParkingPlace pl in places)
        {
            if (pl.IsEmpty()) return pl;
        }
        return null;
    }
    public ParkingPlace GetNearestPlace(Vector2 position)
    {
        foreach (ParkingPlace pl in places)
        {
            if (Vector3.Distance(pl.transform.position, position) < 0.5f) return pl;//اگر هرکدام از پارکینگ ها فاصله ای کمتر از نیم را با ماشین داشت .آن جایگاه پارکینگ را برمیگرداند
        }
        return null;
    }
    public void OpenGiftBoxes()//همه پارکینگ ها را در نظر میگیرد و هر کدوم که باکس داره بصورت اتوماتیک باز می کند
    {
        foreach (ParkingPlace pl in places)
        {
            pl.CheckAutoOpenGift();
        }
    }

    public void DisableCarInPark()
    {
        foreach (ParkingPlace pl in places)
        {
            if (!pl.IsEmpty())
            {
                pl.transform.GetChild(2).GetComponent<Collider2D>().enabled = false;
            }
        }
        if (VipPlace == true)
        {
            placesPosition[placesPosition.Count - 1].GetComponent<Collider2D>().enabled = false;
        }
        try
        {
            controller.videoAds.specialOffer.specialOfferObj.GetComponent<Collider2D>().enabled = false;
        }
        catch (System.Exception)
        {
        }

    }

    public void EnableCarInPark()
    {
        foreach (ParkingPlace pl in places)
        {
            if (!pl.IsEmpty())
            {
                pl.transform.GetChild(2).GetComponent<Collider2D>().enabled = true;
            }
        }
        if (VipPlace == true)
        {
            placesPosition[placesPosition.Count - 1].GetComponent<Collider2D>().enabled = true;
        }
        try
        {
            controller.videoAds.specialOffer.specialOfferObj.GetComponent<Collider2D>().enabled = true;
        }
        catch (System.Exception)
        {
        }
    }
}
[System.Serializable]
public class ParkingPosManage
{
    public int Tedad;
    public int[] RowColumns;
}
[System.Serializable]
public class CarSpeedTycoonBoosts
{
    public int[] level;
    public float[] incSpeed;
}
[System.Serializable]
public class EarningOfflineTycoonBoosts
{
    public int[] level;
    public float[] incEarn;
}
[System.Serializable]
public class ExchangeDeclineTycoonBoosts
{
    public int[] level;
    public float[] rateDecline;
}
