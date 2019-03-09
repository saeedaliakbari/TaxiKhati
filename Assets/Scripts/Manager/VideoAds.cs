using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapsellSDK;
using CodeStage.AntiCheat.ObscuredTypes;

public class VideoAds : MonoBehaviour
{
    public Controller controller;
    public OfflineEraning offlineEarning;
    public ShopPanel shopPanel;
    public RandomGift randomGift;
    public WheelFortuneScript wheelFortuneScript;
    public SpecialOffer specialOffer;
    public GameObject panelTaxiUpVideo, panelNoAds;
    public Image imgNowCar, imgUpCar;
    [HideInInspector]
    public int indexCar = 0;

    private string sdkToken = "ntmnrcicifbdkgjlgqcnnqqcmkedhbbjdgmldpmnmhehcsjctdrfkoobmidbjkimkggbig";

    public GameObject panelError;

    public Text txtPanelError;

    public ZoneVideo zoneOfflineEarning, zoneShopCar, zoneCarUp, zoneGiftPanel, zoneWheelOfFurtune, zoneSpeedX2, zoneShopClose;
    // Use this for initialization
    void Start()
    {
        //ObscuredPrefs.SetDouble("coin", 93021943);
        Tapsell.initialize(sdkToken);
        LoadAd(zoneOfflineEarning, false);
        shopPanel.UpdateCarItems();
    }
    #region Double Offline Earning
    public void BtnDoubleOfflineEarn()
    {
        StartCoroutine(IEDoubleOfflineEarn());
    }
    IEnumerator IEDoubleOfflineEarn()
    {
        if (ObscuredPrefs.GetInt(zoneOfflineEarning.zoneId) == 0)
        {
            LoadAd(zoneOfflineEarning, true);
            controller.parkingManager.DisableCarInPark();
            controller.panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            controller.panelWait.SetActive(false);
            controller.parkingManager.EnableCarInPark();
        }
        if (ObscuredPrefs.GetInt(zoneOfflineEarning.zoneId) == 1)
        {
            ObscuredPrefs.SetInt(zoneOfflineEarning.zoneId, 0);
            ShowAd(zoneOfflineEarning);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    if (result.completed && result.rewarded)
                    {
                        GiftDoubleOfflineEarn();
                    }
                    else
                    {
                        LoadAd(zoneOfflineEarning, false);
                    }
                }
            );
        }
        else
        {

            Debug.Log("Error");
            //panelError.SetActive(true);
            //txtPanelError.text = "خطا در لود ویدئو";
        }
    }
    private void GiftDoubleOfflineEarn()
    {
        offlineEarning.doubleCoin = true;
        offlineEarning.AnimValueChange();
        offlineEarning.btnDouble.interactable = false;
        offlineEarning.btnThird.interactable = false;
    }
    #endregion
    #region Btn SpecialOffer
    public void BtnSpecialOffer()
    {
        //GiftGiftCar();
        specialOffer.btnGem.interactable = false;
        specialOffer.btnVideo.interactable = false;
        StartCoroutine(IEBtnSpecialOffer());
    }
    IEnumerator IEBtnSpecialOffer()
    {
        if (ObscuredPrefs.GetInt(zoneGiftPanel.zoneId) == 0)
        {
            LoadAd(zoneGiftPanel, true);
            controller.parkingManager.DisableCarInPark();
            controller.panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            controller.panelWait.SetActive(false);
            controller.parkingManager.EnableCarInPark();
        }
        if (ObscuredPrefs.GetInt(zoneGiftPanel.zoneId) == 1)
        {
            ObscuredPrefs.SetInt(zoneGiftPanel.zoneId, 0);
            ShowAd(zoneGiftPanel);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    if (result.completed && result.rewarded)
                    {
                        specialOffer.ManageGift();
                    }
                    else
                    {
                        LoadAd(zoneGiftPanel, false);
                    }
                }
            );
        }
        else
        {
            //controller.panelMessage.SetActive(true);
            //controller.txtPanelMessage.text = "در حال حاضر امکان نمایش ویدئو وجود ندارد";
            //controller.parkingManager.DisableCarInPark();
            //Debug.Log("Error");
            specialOffer.btnGem.interactable = true;
            specialOffer.btnVideo.interactable = true;
            //panelError.SetActive(true);
            //txtPanelError.text = "خطا در لود ویدئو";
        }
    }
    #endregion
    #region Btn Shop Car
    public void BtnShopCar(int index)
    {
        StartCoroutine(IEBtnShopCar(index));
    }
    IEnumerator IEBtnShopCar(int index)
    {
        if (ObscuredPrefs.GetInt(zoneShopCar.zoneId) == 0)
        {
            LoadAd(zoneShopCar, true);
            controller.parkingManager.DisableCarInPark();
            controller.panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            
        }
        if (ObscuredPrefs.GetInt(zoneShopCar.zoneId) == 1)
        {
            ObscuredPrefs.SetInt(zoneShopCar.zoneId, 0);
            ShowAd(zoneShopCar);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    if (result.completed && result.rewarded)
                    {
                        ObscuredPrefs.SetInt("mergeCarForVideo", 1);
                        GiftShopCar(index - 1);
                    }
                    else
                    {
                        LoadAd(zoneShopCar, false);
                    }
                }
            );
        }
        controller.panelWait.SetActive(false);
        controller.parkingManager.EnableCarInPark();
        //else
        //{
        //    controller.panelMessage.SetActive(true);
        //    controller.txtPanelMessage.text = "در حال حاضر امکان نمایش ویدئو وجود ندارد";
        //    controller.parkingManager.DisableCarInPark();
        //    Debug.Log("Error");
        //    //panelError.SetActive(true);
        //    //txtPanelError.text = "خطا در لود ویدئو";
        //}
    }
    private void GiftShopCar(int index)
    {
        controller.SpawnACarWithVideo(index);
        shopPanel.BuyCarClick(index);
    }
    #endregion
    #region Btn Car Up
    public void BtnCarUp()
    {
        StartCoroutine(IEBtnCarUp());
    }
    IEnumerator IEBtnCarUp()
    {
        if (ObscuredPrefs.GetInt(zoneCarUp.zoneId) == 0)
        {
            LoadAd(zoneCarUp, true);
            controller.parkingManager.DisableCarInPark();
            controller.panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            
        }
        if (ObscuredPrefs.GetInt(zoneCarUp.zoneId) == 1)
        {
            ObscuredPrefs.SetInt(zoneCarUp.zoneId, 0);
            ShowAd(zoneCarUp);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    if (result.completed && result.rewarded)
                    {
                        GiftCarUp(true);
                    }
                    else
                    {
                        GiftCarUp(false);
                        LoadAd(zoneShopCar, false);
                    }
                }
            );
        }
        controller.panelWait.SetActive(false);
        controller.parkingManager.EnableCarInPark();
        //else
        //{
        //    Debug.Log("Error");
        //    controller.panelMessage.SetActive(true);
        //    controller.txtPanelMessage.text = "در حال حاضر امکان نمایش ویدئو وجود ندارد";
        //    controller.parkingManager.DisableCarInPark();
        //    //panelError.SetActive(true);
        //    //txtPanelError.text = "خطا در لود ویدئو";
        //}
    }
    private void GiftCarUp(bool status)
    {
        if (status)
        {
            indexCar += 1;
        }
        controller.SpawnACarWithVideo(indexCar);
        panelTaxiUpVideo.SetActive(false);

    }
    #endregion
    #region Video Wheel Of Fourtune
    public void BtnVideoWheel()
    {
        if (ObscuredPrefs.GetInt("VideoWheel", 3) > 0)
        {
            StartCoroutine(IEBtnVideoWheel());
            //wheelFortuneScript.GiftWheelWithVideo();
        }
        else
        {
            controller.txtPanelMessage.text = "فرصت ویدئویي رایگان وجود ندارد";
            controller.panelMessage.SetActive(true);
            Debug.Log("Bayad Ta Zaman Baz Shodan Video Sabr Konid");
        }

    }
    IEnumerator IEBtnVideoWheel()
    {
        if (ObscuredPrefs.GetInt(zoneWheelOfFurtune.zoneId) == 0)
        {
            controller.parkingManager.DisableCarInPark();
            LoadAd(zoneWheelOfFurtune, true);
            controller.panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            
        }
        if (ObscuredPrefs.GetInt(zoneWheelOfFurtune.zoneId) == 1)
        {
            ObscuredPrefs.SetInt(zoneWheelOfFurtune.zoneId, 0);
            ShowAd(zoneWheelOfFurtune);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    if (result.completed && result.rewarded)
                    {
                        wheelFortuneScript.GiftWheelWithVideo();
                    }
                    else
                    {
                        LoadAd(zoneShopCar, false);
                    }
                }
            );
        }
        controller.panelWait.SetActive(false);
        controller.parkingManager.EnableCarInPark();
        //else
        //{
        //    Debug.Log("Error");
        //    controller.panelMessage.SetActive(true);
        //    controller.txtPanelMessage.text = "در حال حاضر امکان نمایش ویدئو وجود ندارد";
        //    controller.parkingManager.DisableCarInPark();
        //    //panelError.SetActive(true);
        //    //txtPanelError.text = "خطا در لود ویدئو";
        //}
    }
    #endregion
    #region Btn Speeed X2
    public void BtnSpeeedX2()
    {
        StartCoroutine(IEBtnSpeeedX2());
    }
    IEnumerator IEBtnSpeeedX2()
    {
        if (ObscuredPrefs.GetInt(zoneSpeedX2.zoneId) == 0)
        {
            LoadAd(zoneSpeedX2, true);
            controller.parkingManager.DisableCarInPark();
            controller.panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            
        }
        if (ObscuredPrefs.GetInt(zoneSpeedX2.zoneId) == 1)
        {
            ObscuredPrefs.SetInt(zoneSpeedX2.zoneId, 0);
            ShowAd(zoneSpeedX2);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    if (result.completed && result.rewarded)
                    {
                        GiftSpeedX2();
                    }
                    else
                    {
                        LoadAd(zoneSpeedX2, false);
                    }
                }
            );
        }
        controller.panelWait.SetActive(false);
        controller.parkingManager.EnableCarInPark();
        //else
        //{
        //    controller.panelMessage.SetActive(true);
        //    controller.txtPanelMessage.text = "در حال حاضر امکان نمایش ویدئو وجود ندارد";
        //    controller.parkingManager.DisableCarInPark();
        //    Debug.Log("Error");
        //}
    }
    private void GiftSpeedX2()
    {
        float timeValue = Mathf.Max(0, (float)(Manager.GetActionTime("speed_x2") - Manager.GetCurrentTime()));
        float plus = 150;
        if (timeValue + 150 > 1800)
        {
            plus = 1800 - timeValue;
        }
        Debug.Log("plus: " + plus + " time: " + timeValue);
        double nowtime = Math.Round(Manager.GetCurrentTime());
        double plusTime = Math.Round(plus + timeValue + Manager.GetCurrentTime());
        Debug.Log("New Time : " + plusTime);
        Manager.SetActionTime("speed_x2", plusTime);
        controller.UpdateTimeSpeed2X();
    }
    #endregion
    #region Close Shop Car
    public void BtnCloseShopCar()
    {
        StartCoroutine(IEBtnCloseShopCar());
    }
    IEnumerator IEBtnCloseShopCar()
    {
        Debug.Log("Start Ejra Tabligh");
        if (ObscuredPrefs.GetInt(zoneShopClose.zoneId) == 0)
        {
            Debug.Log("Load Tablogh");
            LoadAd(zoneShopClose, false);
            controller.parkingManager.DisableCarInPark();
            controller.panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
        }
        if (ObscuredPrefs.GetInt(zoneShopClose.zoneId) == 1)
        {
            Debug.Log("Ejra Tabligh");
            ObscuredPrefs.SetInt(zoneShopClose.zoneId, 0);
            ShowAd(zoneShopClose);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    GiftBtnCloseShopCar();
                    LoadAd(zoneShopClose, false);
                }
            );
        }
        controller.panelWait.SetActive(false);
        controller.parkingManager.EnableCarInPark();
        //else
        //{
        //    controller.panelMessage.SetActive(true);
        //    controller.txtPanelMessage.text = "در حال حاضر امکان نمایش تبلیغ وجود ندارد";
        //    controller.parkingManager.DisableCarInPark();
        //    Debug.Log("Error");
        //}
    }
    private void GiftBtnCloseShopCar()
    {
        Debug.Log("Gift" + ObscuredPrefs.GetInt("countCloseShop", 1));
        //ObscuredPrefs.SetInt("countCloseShop", 1);
        ObscuredPrefs.SetInt("countShowAd", ObscuredPrefs.GetInt("countShowAd", 1) + 1);
        Debug.Log("Gift End" + ObscuredPrefs.GetInt("countCloseShop", 1));
        if (ObscuredPrefs.GetInt("countShowAd", 1) > 5)
        {
            panelNoAds.SetActive(true);
            controller.parkingManager.DisableCarInPark();
            ObscuredPrefs.SetInt("countShowAd", 1);
        }
    }
    public void LooadAdCloseShop()//داخل باز کردن فروشگاه ماشین انجام می شود
    {
        if (ObscuredPrefs.GetInt(zoneShopClose.zoneId) == 0 && ObscuredPrefs.GetInt("removeAds", 0) == 0)
        {
            LoadAd(zoneShopClose, false);
        }
    }
    #endregion
    #region Main Methods
    public void LoadAd(ZoneVideo zone, bool ErrorHandling)//درخواست تبلیغ
    {
        Tapsell.requestAd(zone.zoneId, zone.cached,
            (TapsellAd result) =>
            {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                ObscuredPrefs.SetInt(zone.zoneId, 1);
                zone.ad = result;
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id :" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + ObscuredPrefs.GetInt(zone.zoneId));
            },

            (string zoneId) =>
            {
                // onNoAdAvailable
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                Debug.Log("No Ad Available");
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + ObscuredPrefs.GetInt(zone.zoneId));
                if (ErrorHandling)
                {
                    controller.panelMessage.SetActive(true);
                    controller.txtPanelMessage.text = "در حال حاضر تبلیغی برای نمایش وجود ندارد";
                    controller.parkingManager.DisableCarInPark();
                    controller.panelWait.SetActive(false);
                    controller.parkingManager.EnableCarInPark();
                }

            },

            (TapsellError error) =>
            {
                // onError
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                Debug.Log(error.error);
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + ObscuredPrefs.GetInt(zone.zoneId));
                if (ErrorHandling)
                {
                    controller.panelMessage.SetActive(true);
                    controller.txtPanelMessage.text = "مشکلی بوجودآمده لطفا مجددا تلاش نمایید";
                    controller.parkingManager.DisableCarInPark();
                    controller.panelWait.SetActive(false);
                    controller.parkingManager.EnableCarInPark();
                }
            },

            (string zoneId) =>
            {
                // onNoNetwork
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                Debug.Log("No Network: " + zoneId);
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + ObscuredPrefs.GetInt(zone.zoneId));
                if (ErrorHandling)
                {
                    controller.panelMessage.SetActive(true);
                    controller.txtPanelMessage.text = "مشکل در برقراري ارتباط با اینترنت";
                    controller.parkingManager.DisableCarInPark();
                    controller.panelWait.SetActive(false);
                    controller.parkingManager.EnableCarInPark();
                }
            },

            (TapsellAd result) =>
            {
                //onExpiring
                Debug.Log("Expiring");
                ObscuredPrefs.SetInt(zone.zoneId, 0);
                zone.ad = null;
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                LoadAd(zone, false);
                controller.panelWait.SetActive(false);
                controller.parkingManager.EnableCarInPark();
            }

        );

    }

    public void ShowAd(ZoneVideo zone)
    {
        Debug.Log("panel Error false");
        //panelError.SetActive(false);
        TapsellShowOptions options = new TapsellShowOptions();
        options.backDisabled = false;
        options.immersiveMode = false;
        options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_PORTRAIT;
        options.showDialog = true;
        Tapsell.showAd(zone.ad, options); ;
    }
    #endregion
}
[Serializable]
public class ZoneVideo
{
    public string zoneName;
    public string zoneId;
    public bool cached;
    [HideInInspector]
    public TapsellAd ad = null;
}
