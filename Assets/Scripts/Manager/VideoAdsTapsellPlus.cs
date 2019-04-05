using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using Tapsell;
using UnityEngine;
public class VideoAdsTapsellPlus : MonoBehaviour
{
    public string apiKey, signKey;
    public ZoneVideoPlus zoneOfflineEarning, zoneShopCar, zoneCarUp, zoneGiftPanel, zoneWheelOfFurtune, zoneSpeedX2, zoneShopClose;
    public Controller controller;
    // Use this for initialization
    void Start()
    {
        TapsellPlus.Initialize(apiKey, signKey);
    }
    #region Main Method
    public void LoadAd(ZoneVideoPlus zone, bool ErrorHandling)
    {
        TapsellPlus.RequestAd(zone.zoneId,
            (string adId) =>
            {
                //onAdReady
                // TapsellPlus.ShowAd(adId, errorAction, adClosedAction, rewardAction);
                ObscuredPrefs.SetInt(zone.zoneId, 1);
                zone.ad = adId;
            },
            (long code, string message) =>
            {
                //onError
                Debug.Log("code: " + code + ", message: " + message);
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                controller.panelMessage.SetActive(true);
                controller.txtPanelMessage.text = "مشکلی در هنگام دریافت تبلیغ بوجود آمده است";
                controller.parkingManager.DisableCarInPark();
                controller.panelWait.SetActive(false);
                controller.parkingManager.EnableCarInPark();
            },
            () =>
            {
                // onNoAdAvailable
                Debug.Log("No Ad Available!");
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                controller.panelMessage.SetActive(true);
                controller.txtPanelMessage.text = "تبلیغی براي نمایش وجود ندارد";
                controller.parkingManager.DisableCarInPark();
                controller.panelWait.SetActive(false);
                controller.parkingManager.EnableCarInPark();
            },
            () =>
            {
                // onNoNetwork
                Debug.Log("No Network!");
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                controller.panelMessage.SetActive(true);
                controller.txtPanelMessage.text = "اتصال به اینترنت قطع است";
                controller.parkingManager.DisableCarInPark();
                controller.panelWait.SetActive(false);
                controller.parkingManager.EnableCarInPark();
            }
        );
    }
    public void ShowAd(ZoneVideoPlus zone)
    {
        TapsellPlus.ShowAd(zone.ad,
            (long code, string message) =>
            {
                // Error happened during request ad or show ad
                Debug.Log("code: " + code + ", message: " + message);
            },
            (bool completed) =>
            {
                // Ad was closed.
                // completed indicates if the video completed before close or not.
                Debug.Log("Ad Closed, completed: " + completed);
            },
            (string id, string reward) =>
            {
                // The ad is rewarded.
                Debug.Log("The ad is rewarded. (" + "adId: " + id + ", reward: " + reward + ")");
            }
        );
    }
    #endregion
}
[System.Serializable]
public class ZoneVideoPlus
{
    public string zoneName;
    public string zoneId;
    public bool cached;
    [HideInInspector]
    public string ad = null;
}
