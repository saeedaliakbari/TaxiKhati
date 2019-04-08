using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TapsellPlusSDK;
using UnityEngine;
public class VideoAdsTapsellPlus : MonoBehaviour
{
    public string apiKey;
    public ZoneVideoPlus zoneOfflineEarning, zoneShopCar, zoneCarUp, zoneGiftPanel, zoneWheelOfFurtune, zoneSpeedX2, zoneShopClose;
    public Controller controller;
    // Use this for initialization
    void Start()
    {
        TapsellPlus.initialize(apiKey);
    }
    #region Main Method
    public void LoadAd(ZoneVideoPlus zone, bool ErrorHandling)
    {
        TapsellPlus.requestRewardedVideo(zone.zoneId,
            (string adId) =>
            {
                //onAdReady
                // TapsellPlus.ShowAd(adId, errorAction, adClosedAction, rewardAction);
                ObscuredPrefs.SetInt(zone.zoneId, 1);
                zone.ad = adId;
            },
            (TapsellError error) =>
            {
                //onError
                Debug.Log("Error " + error.message);
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                controller.panelMessage.SetActive(true);
                controller.txtPanelMessage.text = "مشکلی در هنگام دریافت تبلیغ بوجود آمده است";
                controller.parkingManager.DisableCarInPark();
                controller.panelWait.SetActive(false);
                controller.parkingManager.EnableCarInPark();
            }
            //,
            //() =>
            //{
            //    // onNoAdAvailable
            //    Debug.Log("No Ad Available!");
            //    ObscuredPrefs.SetInt(zone.zoneId, 0);
            //    controller.panelMessage.SetActive(true);
            //    controller.txtPanelMessage.text = "تبلیغی براي نمایش وجود ندارد";
            //    controller.parkingManager.DisableCarInPark();
            //    controller.panelWait.SetActive(false);
            //    controller.parkingManager.EnableCarInPark();
            //},
            //() =>
            //{
            //    // onNoNetwork
            //    Debug.Log("No Network!");
            //    ObscuredPrefs.SetInt(zone.zoneId, 0);
            //    controller.panelMessage.SetActive(true);
            //    controller.txtPanelMessage.text = "اتصال به اینترنت قطع است";
            //    controller.parkingManager.DisableCarInPark();
            //    controller.panelWait.SetActive(false);
            //    controller.parkingManager.EnableCarInPark();
            //}
        );
    }
    public void ShowAd(ZoneVideoPlus zone)
    {
        TapsellPlus.showAd(zone.ad,
            (string zoneId) =>
            {
                Debug.Log("onOpenAd " + zoneId);
            },
            (string zoneId) =>
            {
                Debug.Log("onCloseAd " + zoneId);
            },
            (string zoneId) =>
            {
                Debug.Log("onReward " + zoneId);
            },
            (TapsellError error) =>
            {
                Debug.Log("onError " + error.message);
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
