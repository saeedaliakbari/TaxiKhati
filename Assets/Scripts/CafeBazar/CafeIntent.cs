using UnityEngine;

public class CafeIntent : MonoBehaviour
{
    public GameObject panelNazar;

    public void btnHatman()
    {
        PlayerPrefs.SetInt("isComment", 1);
        CafeIntent ci = new CafeIntent();
        ci.Like("com.glimgames.resturant");
        PlayerPrefs.SetInt("isComment", 1);
    }
    public void btnBadan()
    {
        panelNazar.gameObject.SetActive(false);
    }
    public void btnHichvaght()
    {
        PlayerPrefs.SetInt("isComment", 1);
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

}
