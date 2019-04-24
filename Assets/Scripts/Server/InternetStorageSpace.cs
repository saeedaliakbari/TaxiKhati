using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InternetStorageSpace : MonoBehaviour
{
    public GameObject panelAutoRetrive, guideManager, panelLockGuide, panelWait, panelSave;
    public Text txtLevelPanelAutoRetrive, txtBtnSavePanelSave;
    public InputField inputPhonePanelSave, inputPassPanelSave;
    public Image imgLastCarPanelAutoRetrice;
    public Controller controller;

    [HideInInspector]
    public bool isNetConnect, isValidUserAndPass = false /*در قسمت لود اطلاعات از سرور استفاده شده است*/  , isPhoneNumberExist = false;// درقسمت بروزرسانی اطلاعات پروفایل استفاده شده است
    #region urls
    private const string url = "https://balootvas.ir/balootvas/TaxiKhati/storage.php?f=";
    private const string urlLoadData = url + "loadData";
    private const string urlSaveData = url + "setData";
    private const string urlUpdateInfo = url + "updateInfo";
    private const string urlCheckAccount = url + "checkAccount";
    private const string urlLoadDataRetrive = url + "loadDataRetrive";
    #endregion
    private UserData LoadUserData { get; set; }
    private UserData autoRetrive { get; set; }
    void Awake()
    {
        if (ObscuredPrefs.GetString("deviceUniqueIdentifier", "") == "")
            ObscuredPrefs.SetString("deviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);// کد منحصر به فرد برای هر دستگاه
        Debug.Log("Uniq: " + ObscuredPrefs.GetString("deviceUniqueIdentifier"));
        if (PlayerPrefs.GetInt("isAccount", -1) == -1)
        {
            StartCoroutine(IEcheckAccount());
        }
        SaveData();//هردفعه که اومد داخل سین اصلی ذخیره کن
    }
    //بررسی اتصال به اینترنت
    public IEnumerator IECheckNet()
    {
        WWW www = new WWW("https://balootvas.ir/");
        yield return www;
        if (www.isDone && www.bytesDownloaded > 0)
        {
            isNetConnect = true;
            Debug.Log("if checkNet=True");
        }
        else if (www.isDone && www.bytesDownloaded == 0)
        {
            isNetConnect = false;
            Debug.Log("if checkNet=False");
        }
    }
    #region Load Data
    //برای نمایش دادن اطلاعات قبل از لودشدن
    public IEnumerator IEInfoRetrive(string phone, string pass)
    {
        var postData = new WWWForm();
        postData.AddField("phone", phone);
        postData.AddField("pass", pass);
        WWW www = new WWW(urlLoadDataRetrive, postData);
        yield return www;
        if (www.error == null)
        {
            if (www.text.Trim() == "4")
            {
                isValidUserAndPass = false;
            }
            else
            {
                isValidUserAndPass = true;
                //درصورتی که لود موفقیت آمیز بود یوزر و پسورد ذخیره شده و هنگام ذخیره سازی از همین یوزر و پسورد استفاده خواهد شد
                ObscuredPrefs.SetString("phoneNumber", phone);
                ObscuredPrefs.SetString("pass", pass);
                LoadUserData = JsonMapper.ToObject<UserData>(www.text.Trim());
            }
        }
    }
    // لود اطلاعات بازی از سرور
    public IEnumerator IEloadData(string phone = "", string pass = "")
    {
        var postData = new WWWForm();
        postData.AddField("phone", phone);
        postData.AddField("pass", pass);
        postData.AddField("deviceUniqueIdentifier", ObscuredPrefs.GetString("deviceUniqueIdentifier"));
        WWW www = new WWW(urlLoadData, postData);
        yield return www;
        if (www.error == null)
        {
            if (www.text.Trim() == "2")
            {
                isValidUserAndPass = false;
            }
            else
            {
                ObscuredPrefs.DeleteAll();
                isValidUserAndPass = true;
                //درصورتی که لود موفقیت آمیز بود یوزر و پسورد ذخیره شده و هنگام ذخیره سازی از همین یوزر و پسورد استفاده خواهد شد
                ObscuredPrefs.SetString("phoneNumber", phone);
                ObscuredPrefs.SetString("pass", pass);
                LoadUserData = JsonMapper.ToObject<UserData>(www.text.Trim());
                SetDataToPlayerPrfs(LoadUserData);
                PlayerPrefs.SetInt("isAccount", 1);
                // درصورتی که اولین لود بعد از نصب باشد مقدار دهی می شود
                if (PlayerPrefs.GetInt("isFirstLoadedAfterInstallation", 0) == 0)
                    PlayerPrefs.SetInt("isFirstLoadedAfterInstallation", 1);
                // درصورتی که کاربر بازیابی اتوماتیک را تایید کرده باشد مقدار دهی می شود
                if (PlayerPrefs.GetInt("isDontRetrieveAndAutoRetrieve", 0) == 0)
                    PlayerPrefs.SetInt("isDontRetrieveAndAutoRetrieve", 1);
            }
        }
    }
    #endregion
    #region Update Profile
    // بروزرسانی اطلاعات پروفایل کاربر
    public IEnumerator IEupdateUserInfo(UpdateUserInfo updateUserInfo)
    {
        var postData = new WWWForm();
        postData.AddField("phone", updateUserInfo.profilePhoneNumber);
        postData.AddField("pass", updateUserInfo.profilePass);
        postData.AddField("gender", updateUserInfo.gender == 0 ? PlayerPrefs.GetInt("gender") : updateUserInfo.gender);
        postData.AddField("age", updateUserInfo.age == 0 ? PlayerPrefs.GetInt("age") : updateUserInfo.age);
        postData.AddField("province", updateUserInfo.province == 0 ? PlayerPrefs.GetInt("province") : updateUserInfo.province);
        postData.AddField("deviceUniqueIdentifier", ObscuredPrefs.GetString("deviceUniqueIdentifier"));
        WWW www = new WWW(urlUpdateInfo, postData);
        yield return www;
        if (www.isDone)
        {
            if (www.text.Trim() == "4")//شماره تلفن قبلا ثبت شده است
            {
                isPhoneNumberExist = true;
            }
            else if (www.text.Trim() == "1" || www.text.Trim() == "0") // باموفقیت آپدیت شد
            {
                isPhoneNumberExist = false;
                if (updateUserInfo.profilePhoneNumber != string.Empty && updateUserInfo.profilePass != string.Empty)
                {
                    PlayerPrefs.SetString("profilePhoneNumber", updateUserInfo.profilePhoneNumber);
                    PlayerPrefs.SetString("profilePass", updateUserInfo.profilePass);
                }
                if (updateUserInfo.age > 0 && updateUserInfo.age > 0 && updateUserInfo.age > 0)
                {
                    PlayerPrefs.SetInt("age", updateUserInfo.age);
                    PlayerPrefs.SetInt("gender", updateUserInfo.gender);
                    PlayerPrefs.SetInt("province", updateUserInfo.province);
                }
                SaveData();// ذخیره اطلاعات پلیرپرفس سرور
            }
        }
    }
    #endregion
    #region Save In Server
    public void BtnSaveSetting()
    {
        panelSave.SetActive(true);
        if (ObscuredPrefs.GetString("phoneNumber", "") == "" && ObscuredPrefs.GetString("pass", "") == "")
        {
            inputPassPanelSave.interactable = true;
            inputPhonePanelSave.interactable = true;
            txtBtnSavePanelSave.text = "تایید";
            txtBtnSavePanelSave.fontSize = 45;
        }
        else
        {
            inputPhonePanelSave.interactable = false;
            inputPhonePanelSave.text = ObscuredPrefs.GetString("phoneNumber", "");
            inputPassPanelSave.interactable = false;
            inputPassPanelSave.text = ObscuredPrefs.GetString("pass", "");
            txtBtnSavePanelSave.text = "ذخیره سازی";
            txtBtnSavePanelSave.fontSize = 30;
        }
    }
    public void BtnSaveInPanelSave()
    {
        if (inputPhonePanelSave.text == "")
        {
            inputPhonePanelSave.ActivateInputField();
        }
        else if (inputPassPanelSave.text == "")
        {
            inputPassPanelSave.ActivateInputField();
        }
        else
        {
            ObscuredPrefs.SetString("phoneNumber", inputPhonePanelSave.text);
            ObscuredPrefs.SetString("pass", inputPassPanelSave.text);
            panelWait.SetActive(true);
            StartCoroutine(IEsaveData(true));
        }
    }
    public void SaveData()
    {
        // درصورتی که یک بار لود شده باشد و یا اکانتی نداشته باشد یا یک دفعه ذخیره شده باشد 
        Debug.Log("Save Data " + PlayerPrefs.GetInt("isFirstLoadedAfterInstallation", 0) + "/" + PlayerPrefs.GetInt("isAccount", -1) + "/" + PlayerPrefs.GetInt("isDontRetrieveAndAutoRetrieve", 0) + "/" + PlayerPrefs.GetInt("isFirstSave", 0) + "/" + (ObscuredPrefs.GetInt("helpStep", 0) >= 22) + "&&" + (PlayerPrefs.GetInt("isFirstLoadedAfterInstallation", 0) == 1 || PlayerPrefs.GetInt("isAccount", -1) == 0 ||
            PlayerPrefs.GetInt("isDontRetrieveAndAutoRetrieve", 0) == 1 || PlayerPrefs.GetInt("isFirstSave", 0) == 1));
        if ((PlayerPrefs.GetInt("isFirstLoadedAfterInstallation", 0) == 1 || PlayerPrefs.GetInt("isAccount", -1) != -1 ||
            PlayerPrefs.GetInt("isDontRetrieveAndAutoRetrieve", 0) == 1 || PlayerPrefs.GetInt("isFirstSave", 0) == 1) && (ObscuredPrefs.GetInt("helpStep", 0) >= 22))
        {
            StartCoroutine(IEsaveData(false));
        }
        else
        {
            Debug.Log("No Save Data");
        }
    }
    // ذخیره کردن اطلاعات بازی در سرور
    private IEnumerator IEsaveData(bool optional)
    {
        LoadUserData = new UserData()
        {
            UserInfo = new UpdateUserInfo(),
            GameData = new GameData()
            {
                UserInfoData = new UserInfoData(),
                AchivmentData = new AchivmentData(),
                CarsData = new CarsData()
            }
        };
        GetDataFromPlayerPrfs(LoadUserData);
        var jsonGameData = JsonMapper.ToJson(LoadUserData);
        var postData = new WWWForm();
        postData.AddField("phone", ObscuredPrefs.GetString("phoneNumber", ""));
        postData.AddField("pass", ObscuredPrefs.GetString("pass", ""));
        postData.AddField("deviceUniqueIdentifier", ObscuredPrefs.GetString("deviceUniqueIdentifier"));
        postData.AddField("gameData", jsonGameData);
        Debug.Log(jsonGameData);
        WWW www = new WWW(urlSaveData, postData);
        yield return www;
        if (www.error == null)
        {
            if (www.text.Trim() == "2")
            {
                Debug.LogError("Not Save" + System.DateTime.Now);
            }
            else
            {
                if (PlayerPrefs.GetInt("isFirstSave", 0) == 0)
                {
                    PlayerPrefs.SetInt("isFirstSave", 1);
                }
                Debug.Log("Save All");
                if (optional)
                    BtnSaveSetting();
            }
        }
        else
            Debug.LogError(www.error);
        panelWait.SetActive(false);
    }
    #endregion
    #region Auto Retrive
    // چک کردن اکانت کاربردر اولین باری که وارد بازی می شود کاربر برای بازیابی بصورت خودکار
    public IEnumerator IEcheckAccount()
    {
        var postData = new WWWForm();
        postData.AddField("deviceUniqueIdentifier", ObscuredPrefs.GetString("deviceUniqueIdentifier"));
        WWW www = new WWW(urlCheckAccount, postData);
        yield return www;
        var Status = www.text.Trim().Substring(www.text.Trim().Length - 1, 1);
        var gameinfo = JsonMapper.ToObject<UserData>(www.text.Trim().Substring(0, www.text.Trim().Length - 1));
        Debug.Log("Status  :" + Status.ToString());
        if (www.error == null)
        {
            if (Status == "1")  // اکانت وجود دارد
            {
                PlayerPrefs.SetInt("isAccount", 1);
                panelAutoRetrive.SetActive(true);
                guideManager.SetActive(false);
                panelLockGuide.SetActive(false);
                int level = gameinfo.GameData.UserInfoData.Level;
                txtLevelPanelAutoRetrive.text = level == -1 || level == 0 ? "1" : level.ToString();
                imgLastCarPanelAutoRetrice.sprite = controller.activeCar[gameinfo.GameData.CarsData.unlocked_car - 1];
                autoRetrive = gameinfo;
                //پنل بازیابی اتوماتیک باید باز شود
            }
            else if (Status == "0")
            { // اکانت وجود ندارد
                PlayerPrefs.SetInt("isAccount", 0);
                //پنل وارد کردن کد معرف باز شود
                SaveData();
            }
            else
                PlayerPrefs.SetInt("isAccount", -1); //نامشخص
        }
        else
        {
            PlayerPrefs.SetInt("isAccount", -1); //نامشخص
        }
    }
    public void AutoRetrive()
    {
        panelWait.SetActive(true);
        SetDataToPlayerPrfs(autoRetrive);
        panelWait.SetActive(false);
        SceneManager.LoadScene(0);
    }
    #endregion


    #region PlayerPrefs
    static void SetDataToPlayerPrfs(object propValue)
    {
        DumpObjectTree(propValue, true);
    }
    static void GetDataFromPlayerPrfs(object propValue)
    {
        DumpObjectTree(propValue, false);
    }
    static void DumpObjectTree(object propValue, bool setValue)
    {
        //Debug.Log("Start ");
        if (propValue == null)
            return;
        var childProps = propValue.GetType().GetProperties();
        foreach (var prop in childProps)
        {
            var name = prop.Name;
            var value = prop.GetValue(propValue, null);
            //Debug.Log(propValue + " //name :" + name.ToString() + " value : " + value);
            #region Set Value
            if (setValue)
            {
                if (prop.PropertyType.Name == "String")
                {
                    if (value == null)
                    {
                        continue;
                    }
                    else
                    {
                        ObscuredPrefs.SetString(name, value.ToString());
                        continue;
                    }
                }
                else if (prop.PropertyType.Name == "Boolean")
                {
                    ObscuredPrefs.SetBool(name, (bool)value);
                }
                else if (prop.PropertyType.Name == "Int32")
                {
                    int val;
                    var isNumeric = int.TryParse(value.ToString(), out val);
                    if (isNumeric && val > -1)
                        ObscuredPrefs.SetInt(name, val);
                }
                else if (prop.PropertyType.Name == "Double")
                {
                    double val;
                    var isNumeric = double.TryParse(value.ToString(), out val);
                    if (isNumeric && val > 0)
                        ObscuredPrefs.SetDouble(name, val);
                }
                else if (prop.PropertyType.Name == "Single")
                {
                    float val;
                    var isNumeric = float.TryParse(value.ToString(), out val);
                    if (isNumeric && val > 0)
                        ObscuredPrefs.SetFloat(name, val);
                }
            }
            #endregion
            #region Get Value
            else
            {
                PropertyInfo propertyToSet = propValue.GetType().GetProperty(name);
                if (prop.PropertyType.Name == "Int32")
                {
                    if (name == "gender")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, -1), null);
                    else if (name == "age")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, -1), null);
                    else if (name == "province")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, -1), null);
                    else if (name == "VideoWheel")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, 3), null);
                    else if (name == "num_of_places")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, 4), null);
                    else if (name == "num_of_slot")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, 2), null);
                    else if (name == "maxXp")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, 118000), null);
                    else if (name == "Level")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, 1), null);
                    else if (name == "unlocked_car")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, 1), null);
                    else
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetInt(name, 0), null);
                }
                else if (prop.PropertyType.Name == "Boolean")
                {
                    propertyToSet.SetValue(propValue, ObscuredPrefs.GetBool(name, false), null);
                }
                else if (prop.PropertyType.Name == "Double")
                {
                    if (name == "coinTotal")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetDouble(name, 21000), null);
                    else if (name == "coin")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetDouble(name, 21000), null);
                    else if (name == "gem")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetDouble(name, 5), null);
                    else
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetDouble(name, 0), null);
                }
                else if (prop.PropertyType.Name == "String")
                {
                    if (name.Contains("TimeVideoWheel"))
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetString(name, "1992,11,30,00,00,00"), null);
                    else if (name == "saved_list_cars")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetString(name, "{\"listCars\":[],\"listBoxes\":[]}"), null);
                    else
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetString(name, ""), null);
                }
                else if (prop.PropertyType.Name == "Single")
                {
                    if (name == "carsSpeedTycoon")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetFloat(name, 1), null);
                    else if (name == "offlineEarnTycoonBoosts")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetFloat(name, 1), null);
                    else if (name == "incomeLine")
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetFloat(name, 1), null);
                    else
                        propertyToSet.SetValue(propValue, ObscuredPrefs.GetFloat(name, 0), null);
                }
            }
            #endregion
            DumpObjectTree(value, setValue);
        }
    }
    #endregion
}
public class UserData
{
    public UpdateUserInfo UserInfo { get; set; }
    public GameData GameData { get; set; }
}
public class UpdateUserInfo
{
    public string profilePhoneNumber { get; set; }
    public string profilePass { get; set; }
    public int gender { get; set; }
    public int age { get; set; }
    public int province { get; set; }
}
public class GameData
{
    #region Help
    public int buyed_car { get; set; }
    public int helpStep { get; set; }
    public int returned_car { get; set; }
    #endregion
    #region Itmes
    public float incomeLine { get; set; }
    public float offCar { get; set; }
    public int offShopCarLevel { get; set; }
    public int upIncomeLevel { get; set; }
    #endregion
    #region Time
    public double offline_earning_time { get; set; }
    public bool setTimer { get; set; }
    public int todayDate { get; set; }
    public bool set_x { get; set; }
    public double speed_x2_time { get; set; }
    public double speed_2x_for_150s_time { get; set; }
    public double earning_5x_for_1m_time { get; set; }
    public double earning_5x_for_1m_special_time { get; set; }
    #endregion
    #region Tycoon Boosts
    public int carSpeedTycoonLevel { get; set; }
    public float carsSpeedTycoon { get; set; }
    public int exchangeDeclineTycoonLevel { get; set; }
    public float exchangeDeclineTycoon { get; set; }
    public int offlineEarnTycoonLevel { get; set; }
    public float offlineEarnTycoonBoosts { get; set; }
    #endregion
    #region Video
    public string TimeVideoWheel1 { get; set; }
    public string TimeVideoWheel2 { get; set; }
    public string TimeVideoWheel3 { get; set; }
    public int VideoWheel { get; set; }
    #endregion
    public UserInfoData UserInfoData { get; set; }
    public AchivmentData AchivmentData { get; set; }
    public CarsData CarsData { get; set; }
}
public class AchivmentData
{
    #region mainAchiv
    public int mainAchiv1 { get; set; }
    public int mainAchiv2 { get; set; }
    public int mainAchiv3 { get; set; }
    public int mainAchiv4 { get; set; }
    public int mainAchiv5 { get; set; }
    public int mainAchiv6 { get; set; }
    public int mainAchiv7 { get; set; }
    public int mainAchiv8 { get; set; }
    public int mainAchiv9 { get; set; }
    public int mainAchiv10 { get; set; }
    public int mainAchiv11 { get; set; }
    public int mainAchiv12 { get; set; }
    public int mainAchiv13 { get; set; }
    public int mainAchiv14 { get; set; }
    public int mainAchiv15 { get; set; }
    public int mainAchiv16 { get; set; }
    #endregion
    #region mainAchivGet
    public int mainAchivGet1 { get; set; }
    public int mainAchivGet2 { get; set; }
    public int mainAchivGet3 { get; set; }
    public int mainAchivGet4 { get; set; }
    public int mainAchivGet5 { get; set; }
    public int mainAchivGet6 { get; set; }
    public int mainAchivGet7 { get; set; }
    public int mainAchivGet8 { get; set; }
    public int mainAchivGet9 { get; set; }
    public int mainAchivGet10 { get; set; }
    public int mainAchivGet11 { get; set; }
    public int mainAchivGet12 { get; set; }
    public int mainAchivGet13 { get; set; }
    public int mainAchivGet14 { get; set; }
    public int mainAchivGet15 { get; set; }
    public int mainAchivGet16 { get; set; }
    #endregion
}
public class CarsData
{
    public int curr_car_index { get; set; }
    public string saved_list_cars { get; set; }
    public int unlocked_car { get; set; }
    #region Car Price
    public double car_price_0 { get; set; }
    public double car_price_1 { get; set; }
    public double car_price_2 { get; set; }
    public double car_price_3 { get; set; }
    public double car_price_4 { get; set; }
    public double car_price_5 { get; set; }
    public double car_price_6 { get; set; }
    public double car_price_7 { get; set; }
    public double car_price_8 { get; set; }
    public double car_price_9 { get; set; }
    public double car_price_10 { get; set; }
    public double car_price_11 { get; set; }
    public double car_price_12 { get; set; }
    public double car_price_13 { get; set; }
    public double car_price_14 { get; set; }
    public double car_price_15 { get; set; }
    public double car_price_16 { get; set; }
    public double car_price_17 { get; set; }
    public double car_price_18 { get; set; }
    public double car_price_19 { get; set; }
    public double car_price_20 { get; set; }
    public double car_price_21 { get; set; }
    public double car_price_22 { get; set; }
    public double car_price_23 { get; set; }
    public double car_price_24 { get; set; }
    public double car_price_25 { get; set; }
    public double car_price_26 { get; set; }
    public double car_price_27 { get; set; }
    public double car_price_28 { get; set; }
    public double car_price_29 { get; set; }
    public double car_price_30 { get; set; }
    public double car_price_31 { get; set; }
    public double car_price_32 { get; set; }
    public double car_price_33 { get; set; }
    public double car_price_34 { get; set; }
    public double car_price_35 { get; set; }
    public double car_price_36 { get; set; }
    public double car_price_37 { get; set; }
    public double car_price_38 { get; set; }
    public double car_price_39 { get; set; }
    public double car_price_40 { get; set; }
    public double car_price_41 { get; set; }
    public double car_price_42 { get; set; }
    public double car_price_43 { get; set; }
    public double car_price_44 { get; set; }
    public double car_price_45 { get; set; }
    public double car_price_46 { get; set; }
    public double car_price_47 { get; set; }
    public double car_price_48 { get; set; }
    public double car_price_49 { get; set; }
    #endregion
}
public class UserInfoData
{
    public int userid { get; set; }
    public string username { get; set; }
    public double coinTotal { get; set; }
    public double coin { get; set; }
    public double gem { get; set; }
    public int Level { get; set; }
    public int Xp { get; set; }
    public int maxXp { get; set; }
    public double token { get; set; }
    public bool join_insta { get; set; }
    public bool join_tele { get; set; }
    public int num_of_places { get; set; }
    public int num_of_slot { get; set; }
    public bool UnlockedBoosters { get; set; }
    public bool UnlockedQuest { get; set; }
    public bool UnlockedRank { get; set; }
    public bool UnlockedTimeBoost { get; set; }
    public bool UnlockedWheel { get; set; }
}

