using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CafeIntent : MonoBehaviour
{
    public Controller controller;
    public GameObject panelComment, panelUpdate, btnClosePanelUpdate, panelSendMessage;
    public InputField inputComment;
    private string strLinkGetInfo = "https://balootvas.ir/balootvas/TaxiKhati/getinfo.php";
    private string strLinkSendComment = "https://balootvas.ir/balootvas/TaxiKhati/insertComment.php";
    private string version;
    private int bundle;
    private int forceUpdate;
    private int bundleCodeVersion = 0;
    void Start()
    {
#if UNITY_EDITOR
        //Debug.Log("UNITY_EDITOR bundleCodeVersion:" + UnityEditor.PlayerSettings.Android.bundleVersionCode);
        bundleCodeVersion = UnityEditor.PlayerSettings.Android.bundleVersionCode;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        var pInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", Application.identifier, 0);
        bundleCodeVersion = pInfo.Get<int>("versionCode");
        Debug.Log("UNITY_ANDROID bundleCodeVersion:" + bundleCodeVersion);
#endif
        StartCoroutine(IEGetApp());
    }
    public void btnAre()
    {
        PlayerPrefs.SetInt("comment", 1);
        CafeIntent ci = new CafeIntent();
        ci.Like("ir.balootgames.taxi");
        PlayerPrefs.SetInt("comment", 1);
    }
    public void btnSendComment()
    {
        controller.panelWait.SetActive(true);
        StartCoroutine(IESendComment());
    }

    public void OpenAPP(string PackageName)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
        intentObject.Call<AndroidJavaObject>("setData", uriClass.CallStatic<AndroidJavaObject>("parse", "bazaar://details?id=" + PackageName));
        intentObject.Call<AndroidJavaObject>("setPackage", "com.farsitel.bazaar");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);
    }


    //like directly youre app in cafe store
    public void Like(string PackageName)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_EDIT"));
        intentObject.Call<AndroidJavaObject>("setData", uriClass.CallStatic<AndroidJavaObject>("parse", "bazaar://details?id=" + PackageName));
        intentObject.Call<AndroidJavaObject>("setPackage", "com.farsitel.bazaar");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);

    }

    IEnumerator IEGetApp()
    {
        WWWForm wwwForm = new WWWForm();
        WWW www = new WWW(strLinkGetInfo, wwwForm);
        yield return www;
        if (www.error == null)
        {
            if (www.isDone)
            {
                JsonData jsonBooks = JsonMapper.ToObject(www.text);
                bundle = int.Parse(jsonBooks[0][1].ToString());
                version = jsonBooks[0][2].ToString();
                forceUpdate = int.Parse(jsonBooks[0][3].ToString());
                //Debug.Log("bundle: " + bundle + " forceUpdate: " + forceUpdate);
                if (bundle > bundleCodeVersion)
                {
                    panelUpdate.SetActive(true);
                    controller.parkingManager.DisableCarInPark();
                    if (forceUpdate == 1)
                    {
                        btnClosePanelUpdate.SetActive(false);
                    }
                    else
                    {
                        btnClosePanelUpdate.SetActive(true);
                    }
                }
            }
        }
    }
    IEnumerator IESendComment()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("comment", inputComment.text);
        WWW www = new WWW(strLinkSendComment, wwwForm);
        yield return www;
        controller.panelWait.SetActive(false);
        if (www.error == null)
        {
            if (www.isDone)
            {
                panelSendMessage.SetActive(false);
                controller.parkingManager.EnableCarInPark();
                PlayerPrefs.SetInt("comment", 1);

            }
        }
        else
        {
            //Debug.Log(www.error.ToString());
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "خطا در ارسال نظر\nلطفا اینترنت خود را چک نمایید";
        }
    }
}
