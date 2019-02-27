using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapsellSDK;
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

    public GameObject panelWait, panelError;

    public Text txtPanelError;

    public ZoneVideo zoneOfflineEarning, zoneShopCar, zoneCarUp, zoneGiftPanel, zoneWheelOfFurtune, zoneSpeedX2, zoneShopClose;
    // Use this for initialization
    void Start()
    {
        //ObscuredPrefs.SetDouble("coin", 93021943);
        Tapsell.initialize(sdkToken);
        LoadAd(zoneOfflineEarning);
    }
    #region Double Offline Earning
    public void BtnDoubleOfflineEarn()
    {
        StartCoroutine(IEDoubleOfflineEarn());
    }
    IEnumerator IEDoubleOfflineEarn()
    {
        if (PlayerPrefs.GetInt(zoneOfflineEarning.zoneId) == 0)
        {
            LoadAd(zoneOfflineEarning);
            //panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            //panelWait.SetActive(false);
        }
        if (PlayerPrefs.GetInt(zoneOfflineEarning.zoneId) == 1)
        {
            PlayerPrefs.SetInt(zoneOfflineEarning.zoneId, 0);
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
                        LoadAd(zoneOfflineEarning);
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
        if (PlayerPrefs.GetInt(zoneGiftPanel.zoneId) == 0)
        {
            LoadAd(zoneGiftPanel);
            //panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            //panelWait.SetActive(false);
        }
        if (PlayerPrefs.GetInt(zoneGiftPanel.zoneId) == 1)
        {
            PlayerPrefs.SetInt(zoneGiftPanel.zoneId, 0);
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
                        LoadAd(zoneGiftPanel);
                    }
                }
            );
        }
        else
        {
            Debug.Log("Error");
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
        if (PlayerPrefs.GetInt(zoneShopCar.zoneId) == 0)
        {
            LoadAd(zoneShopCar);
            //panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            //panelWait.SetActive(false);
        }
        if (PlayerPrefs.GetInt(zoneShopCar.zoneId) == 1)
        {
            PlayerPrefs.SetInt(zoneShopCar.zoneId, 0);
            ShowAd(zoneShopCar);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    if (result.completed && result.rewarded)
                    {
                        PlayerPrefs.SetInt("mergeCarForVideo", 1);
                        GiftShopCar(index - 1);
                    }
                    else
                    {
                        LoadAd(zoneShopCar);
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
        if (PlayerPrefs.GetInt(zoneCarUp.zoneId) == 0)
        {
            LoadAd(zoneCarUp);
            //panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            //panelWait.SetActive(false);
        }
        if (PlayerPrefs.GetInt(zoneCarUp.zoneId) == 1)
        {
            PlayerPrefs.SetInt(zoneCarUp.zoneId, 0);
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
                        LoadAd(zoneShopCar);
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
        if (PlayerPrefs.GetInt("VideoWheel", 3) > 0)
        {
            StartCoroutine(IEBtnVideoWheel());
            //wheelFortuneScript.GiftWheelWithVideo();
        }
        else
        {
            Debug.Log("Bayad Ta Zaman Baz Shodan Video Sabr Konid");
        }

    }
    IEnumerator IEBtnVideoWheel()
    {
        if (PlayerPrefs.GetInt(zoneWheelOfFurtune.zoneId) == 0)
        {
            LoadAd(zoneWheelOfFurtune);
            //panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            //panelWait.SetActive(false);
        }
        if (PlayerPrefs.GetInt(zoneWheelOfFurtune.zoneId) == 1)
        {
            PlayerPrefs.SetInt(zoneWheelOfFurtune.zoneId, 0);
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
                        LoadAd(zoneShopCar);
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
    #endregion
    #region Btn Speeed X2
    public void BtnSpeeedX2()
    {
        StartCoroutine(IEBtnSpeeedX2());
    }
    IEnumerator IEBtnSpeeedX2()
    {
        if (PlayerPrefs.GetInt(zoneSpeedX2.zoneId) == 0)
        {
            LoadAd(zoneSpeedX2);
            //panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            //panelWait.SetActive(false);
        }
        if (PlayerPrefs.GetInt(zoneSpeedX2.zoneId) == 1)
        {
            PlayerPrefs.SetInt(zoneSpeedX2.zoneId, 0);
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
                        LoadAd(zoneSpeedX2);
                    }
                }
            );
        }
        else
        {
            Debug.Log("Error");
        }
    }
    private void GiftSpeedX2()
    {
        float timeValue = Mathf.Max(0, (float)(Manager.GetActionTime("speed_x2") - Manager.GetCurrentTime()));
        double nowtime = Math.Round(Manager.GetCurrentTime());
        double plusTime = Math.Round(150 + timeValue + Manager.GetCurrentTime());
        Manager.SetActionTime("speed_x2", plusTime);
    }
    #endregion
    #region Close Shop Car
    public void BtnCloseShopCar()
    {
        StartCoroutine(IEBtnCloseShopCar());
    }
    IEnumerator IEBtnCloseShopCar()
    {
        if (PlayerPrefs.GetInt(zoneShopClose.zoneId) == 0)
        {
            LoadAd(zoneShopClose);
            //panelWait.SetActive(true);
            Debug.Log("Wait");
            yield return new WaitForSeconds(8f);
            Debug.Log("Wait Done");
            //panelWait.SetActive(false);
        }
        if (PlayerPrefs.GetInt(zoneShopClose.zoneId) == 1)
        {
            PlayerPrefs.SetInt(zoneShopClose.zoneId, 0);
            ShowAd(zoneShopClose);
            Tapsell.setRewardListener(
                (TapsellAdFinishedResult result) =>
                {
                    GiftBtnCloseShopCar();
                    LoadAd(zoneShopClose);
                }
            );
        }
        else
        {
            Debug.Log("Error");
        }
    }
    private void GiftBtnCloseShopCar()
    {
        Debug.Log("Gift" + PlayerPrefs.GetInt("countCloseShop", 1));
        PlayerPrefs.SetInt("countCloseShop", 1);
        PlayerPrefs.SetInt("countShowAd", PlayerPrefs.GetInt("countShowAd", 1) + 1);
        Debug.Log("Gift End" + PlayerPrefs.GetInt("countCloseShop", 1));
        if (PlayerPrefs.GetInt("countShowAd", 1) > 5)
        {
            panelNoAds.SetActive(true);
            PlayerPrefs.SetInt("countShowAd", 1);
        }
    }
    public void LooadAdCloseShop()//داخل باز کردن فروشگاه ماشین انجام می شود
    {
        if (PlayerPrefs.GetInt(zoneShopClose.zoneId) == 0 && PlayerPrefs.GetInt("removeAds", 0) == 0)
        {
            LoadAd(zoneShopClose);
        }
    }
    #endregion
    #region Main Methods
    public void LoadAd(ZoneVideo zone)//درخواست تبلیغ
    {
        Tapsell.requestAd(zone.zoneId, zone.cached,
            (TapsellAd result) =>
            {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                PlayerPrefs.SetInt(zone.zoneId, 1);
                zone.ad = result;
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id :" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + PlayerPrefs.GetInt(zone.zoneId));
            },

            (string zoneId) =>
            {
                // onNoAdAvailable
                PlayerPrefs.SetInt(zone.zoneId, 0);
                Debug.Log("No Ad Available");
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + PlayerPrefs.GetInt(zone.zoneId));
            },

            (TapsellError error) =>
            {
                // onError
                PlayerPrefs.SetInt(zone.zoneId, 0);
                Debug.Log(error.error);
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + PlayerPrefs.GetInt(zone.zoneId));
            },

            (string zoneId) =>
            {
                // onNoNetwork
                PlayerPrefs.SetInt(zone.zoneId, 0);
                Debug.Log("No Network: " + zoneId);
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                Debug.Log(zone.zoneName + ": " + PlayerPrefs.GetInt(zone.zoneId));
            },

            (TapsellAd result) =>
            {
                //onExpiring
                Debug.Log("Expiring");
                PlayerPrefs.SetInt(zone.zoneId, 0);
                zone.ad = null;
                Debug.Log("End Load ad : " + zone.ad == null ? "NULL" : "id:" + zone.ad.adId);
                LoadAd(zone);

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
