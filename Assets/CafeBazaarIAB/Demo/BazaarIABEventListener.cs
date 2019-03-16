using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BazaarPlugin;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class BazaarIABEventListener : MonoBehaviour
{
    //public Text[] txtPrice, txtTitle;
    public IAPCafeBazar iapCafeBazar;
    private string[] str;
    private string strInsertBuy = "https://balootvas.ir/balootvas/TaxiKhati/insertBuy.php";
    //void Start()
    //{
    //    StartCoroutine(IEBuyDate(iapCafeBazar.skus[1]));
    //}
#if UNITY_ANDROID

    void OnEnable()
    {
        //Debug.Log("ViP Start()");

        //ObscuredPrefs.SetInt("num_of_places_vip", 2);//added 2 parking
        //ObscuredPrefs.SetInt("num_of_slot_vip", 2);//added 2 line
        //iapCafeBazar.controller.parkingManager.SpawnPlacesVIP();
        //iapCafeBazar.controller.slotManager.InitSlotsVIP();
        //ObscuredPrefs.SetFloat("offliceEarnVip", 1.2f);//added 20% offline earning
        //ObscuredPrefs.SetInt("removeAds", 1);//remove ads in shop
        //ObscuredPrefs.SetInt("gemPerDay", 1);//10 gem per day
        //ObscuredPrefs.SetFloat("speedVip", 1.5f);//added 50% speed
        //iapCafeBazar.controller.slotManager.UpdateEarningSpeedText();
        //iapCafeBazar.controller.GiftDaily();


        // Listen to all events for illustration purposes
        IABEventManager.billingSupportedEvent += billingSupportedEvent;
        IABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        IABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        IABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        IABEventManager.querySkuDetailsSucceededEvent += querySkuDetailsSucceededEvent;
        IABEventManager.querySkuDetailsFailedEvent += querySkuDetailsFailedEvent;
        IABEventManager.queryPurchasesSucceededEvent += queryPurchasesSucceededEvent;
        IABEventManager.queryPurchasesFailedEvent += queryPurchasesFailedEvent;
        IABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        IABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        IABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        IABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }
    void OnDisable()
    {
        // Remove all event handlers
        IABEventManager.billingSupportedEvent -= billingSupportedEvent;
        IABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        IABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        IABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        IABEventManager.querySkuDetailsSucceededEvent -= querySkuDetailsSucceededEvent;
        IABEventManager.querySkuDetailsFailedEvent -= querySkuDetailsFailedEvent;
        IABEventManager.queryPurchasesSucceededEvent -= queryPurchasesSucceededEvent;
        IABEventManager.queryPurchasesFailedEvent -= queryPurchasesFailedEvent;
        IABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        IABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        IABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        IABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
    }
    #region Init
    void billingSupportedEvent()
    {
        BazaarIAB.enableLogging(false);//فعال سازی اطلاعات فراخوانی توابع در زمان اشکال زدایی
        //Debug.Log("billingSupportedEvent");
        str = new string[iapCafeBazar.skus.Length];
        for (int i = 0; i < iapCafeBazar.skus.Length; i++)
        {
            str[i] = iapCafeBazar.skus[i];
        }
        //BazaarIAB.querySkuDetails(str);//برای گرفتن اطلاعات محصولاتی که در پنل پرداخت درون برنامه ای تعریف کرده اید مثل قیمت، عنوان و … باید از این تابع استفاده کنید. برای استفاده از این تابع نیازی نیست که کاربر حتما در برنامه‌ی بازار لاگین کرده باشد.
        StartCoroutine(IECheckNet());
        //BazaarIAB.queryInventory(str);
    }
    IEnumerator IECheckNet()
    {
        WWW www = new WWW("https://balootvas.ir/");
        yield return www;
        if (www.error == null)
        {
            if (www.isDone)
            {
                BazaarIAB.queryInventory(str);
            }
        }
    }
    void billingNotSupportedEvent(string error)
    {
        //Debug.Log("billingNotSupportedEvent: " + error);
        BazaarIAB.init(iapCafeBazar.RSA);
    }
    #endregion
    #region Inventory
    void queryInventorySucceededEvent(List<BazaarPurchase> purchases, List<BazaarSkuInfo> skus)
    {
        ObscuredPrefs.SetInt("num_of_places_vip", 0);
        ObscuredPrefs.SetInt("num_of_slot_vip", 0);
        ObscuredPrefs.SetFloat("offliceEarnVip", 0);
        ObscuredPrefs.SetInt("removeAds", 0);
        ObscuredPrefs.SetInt("gemPerDay", 0);
        ObscuredPrefs.SetFloat("speedVip", 0);
        //Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
        for (int i = 0; i < purchases.Count; ++i)
        {
            if (purchases[i].ProductId == iapCafeBazar.skus[6])
            {
                //بقیه اطلاعات مربوط به اشتراک وارد شود
                ObscuredPrefs.SetInt("num_of_places_vip", 2);//added 2 parking
                ObscuredPrefs.SetInt("num_of_slot_vip", 2);//added 2 line
                iapCafeBazar.controller.parkingManager.SpawnPlacesVIP();
                iapCafeBazar.controller.slotManager.InitSlotsVIP();
                ObscuredPrefs.SetFloat("offliceEarnVip", 0.2f);//added 20% offline earning
                ObscuredPrefs.SetInt("removeAds", 1);//remove ads in shop
                ObscuredPrefs.SetInt("gemPerDay", 1);//10 gem per day
                ObscuredPrefs.SetFloat("speedVip", 0.5f);//added 50% speed
                iapCafeBazar.controller.slotManager.UpdateEarningSpeedText();
                iapCafeBazar.controller.GiftDaily();
                iapCafeBazar.controller.btnVip.SetActive(false);
            }
            else if (purchases[i].ProductId == iapCafeBazar.skus[7])
            {
                ObscuredPrefs.SetInt("removeAds", 1);//remove ads in shop
            }
            else
            {
                BazaarIAB.consumeProduct(purchases[i].ProductId);
            }
        }
    }

    void queryInventoryFailedEvent(string error)
    {
        //Debug.Log("queryInventoryFailedEvent: " + error);
        BazaarIAB.queryInventory(str);
    }
    #endregion
    #region SKU Details
    private void querySkuDetailsSucceededEvent(List<BazaarSkuInfo> skus)
    {
        //Debug.Log(string.Format("querySkuDetailsSucceededEvent. total skus: {0}", skus.Count));
        //for (int i = 0; i < skus.Count - 2; ++i)
        //{
        //    //txtTitle[i].text = skus[i].Title;
        //    //txtPrice[i].text = skus[i].Price;
        //    Debug.Log("i>" + i + ":" + skus[i].Title + ">" + skus[i].Price);
        //}

    }

    private void querySkuDetailsFailedEvent(string error)
    {
        //Debug.Log("querySkuDetailsFailedEvent: " + error);
        BazaarIAB.querySkuDetails(str);
    }
    #endregion
    #region Baray Chand Kharid
    private void queryPurchasesSucceededEvent(List<BazaarPurchase> purchases)
    {
        //Debug.Log(string.Format("queryPurchasesSucceededEvent. total purchases: {0}", purchases.Count));

        //for (int i = 0; i < purchases.Count; ++i)
        //{
        //    Debug.Log(purchases[i].ToString());
        //}
    }

    private void queryPurchasesFailedEvent(string error)
    {
        //Debug.Log("queryPurchasesFailedEvent: " + error);
    }
    #endregion
    #region Purchase
    void purchaseSucceededEvent(BazaarPurchase purchase)
    {
        //Debug.Log("purchaseSucceededEvent: "/* + purchase*/);
        //Debug.Log("developerPayload" + purchase.DeveloperPayload);
        //Debug.Log("Order ID: " + purchase.OrderId);
        //Debug.Log("Time Kharid : " + purchase.PurchaseTime);
        //Debug.Log("Token Yekta Kharid: " + purchase.PurchaseToken);
        //Debug.Log("Type; " + purchase.Type);
        //Debug.Log("PurchaseState; " + purchase.PurchaseState);
        if (purchase.DeveloperPayload == ObscuredPrefs.GetString("developerPayload"))
        {
            ObscuredPrefs.SetString("developerPayload", "");
            //Debug.Log("developerPayload is Ok");
            if (purchase.PurchaseState == BazaarPurchase.BazaarPurchaseState.Purchased)
            {
                if (purchase.ProductId == iapCafeBazar.skus[6])
                {
                    ObscuredPrefs.SetInt("num_of_places_vip", 2);
                    iapCafeBazar.controller.panelMessage.SetActive(true);
                    iapCafeBazar.controller.txtPanelMessage.text = "پارکینگ به خطوط شما اضافه شد";
                    iapCafeBazar.controller.parkingManager.SpawnNewPlace();
                    iapCafeBazar.controller.parkingManager.SpawnNewPlace();
                    iapCafeBazar.controller.parkingManager.UpdatePlacePosition();
                    ObscuredPrefs.SetInt("num_of_slot_vip", 2);//added 2 line
                    iapCafeBazar.controller.slotManager.SpawnASlot();
                    iapCafeBazar.controller.slotManager.SpawnASlot();
                    iapCafeBazar.controller.slotManager.UpdatePosition();
                    ObscuredPrefs.SetFloat("offliceEarnVip", 0.2f);//added 20% offline earning
                    ObscuredPrefs.SetInt("removeAds", 1);//remove ads in shop
                    ObscuredPrefs.SetInt("gemPerDay", 1);//10 gem per day
                    ObscuredPrefs.SetFloat("speedVip", 0.5f);//added 50% speed
                    iapCafeBazar.controller.slotManager.UpdateEarningSpeedText();
                    iapCafeBazar.controller.GiftDaily();
                    iapCafeBazar.controller.btnVip.SetActive(false);
                }
                else if (purchase.ProductId == iapCafeBazar.skus[7])
                {
                    ObscuredPrefs.SetInt("removeAds", 1);//remove ads in shop
                    iapCafeBazar.controller.panelMessage.SetActive(true);
                    iapCafeBazar.controller.txtPanelMessage.text = "تبلیغات بنري بازي حذف شد";
                    iapCafeBazar.controller.videoAds.panelNoAds.SetActive(false);
                    iapCafeBazar.controller.parkingManager.EnableCarInPark();
                }
                else {
                    iapCafeBazar.controller.panelWait.SetActive(true);
                    BazaarIAB.queryInventory(new string[] { purchase.ProductId });
                }
                StartCoroutine(IEBuyDate(purchase.ProductId));
            }
            else if (purchase.PurchaseState == BazaarPurchase.BazaarPurchaseState.Canceled)
            {
                //Debug.Log("purchase is Canceled");
                iapCafeBazar.controller.panelMessage.SetActive(true);
                iapCafeBazar.controller.txtPanelMessage.text = "عملیات توسط شما لغو شد";
            }
            else
            {
                //Debug.Log("purchase is 2 Refunded");
                iapCafeBazar.controller.panelMessage.SetActive(true);
                iapCafeBazar.controller.txtPanelMessage.text = "خطا در عملیات پرداخت";
            }
        }
    }

    void purchaseFailedEvent(string error)
    {
        //Debug.Log("purchaseFailedEvent: " + error);
        iapCafeBazar.controller.txtPanelMessage.text = "خطا در پرداخت ";
        iapCafeBazar.controller.panelMessage.SetActive(true);
    }
    #endregion
    #region Consume Purchase
    void consumePurchaseSucceededEvent(BazaarPurchase purchase)
    {
        //Debug.Log("consumePurchaseSucceededEvent: " + purchase);
        for (int i = 0; i < iapCafeBazar.skus.Length; i++)
        {
            if (i < 6)//Eshteraki nemitavanim masraf konim
            {
                if (purchase.ProductId == iapCafeBazar.skus[i])
                {
                    ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") + iapCafeBazar.gem[i]);
                    iapCafeBazar.controller.panelWait.SetActive(false);
                    iapCafeBazar.controller.txtPanelMessage.text = "تبریک\n" + iapCafeBazar.gem[i] + " الماس اضافه شد";
                    iapCafeBazar.controller.panelMessage.SetActive(true);
                    iapCafeBazar.controller.SetText();
                }
            }
        }
    }

    void consumePurchaseFailedEvent(string error)
    {
        //Debug.Log("consumePurchaseFailedEvent: " + error);
        BazaarIAB.queryInventory(str);
    }
    #endregion
#endif
    #region Insert Purchase In DataBase
    IEnumerator IEBuyDate(string packName)
    {
        //Debug.Log("start" + PersianDate(new DateTime(2019, 11, 30)));
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("date", PersianDate(DateTime.Now));
        wwwForm.AddField("packNo", packName);
        WWW www = new WWW(strInsertBuy, wwwForm);
        yield return www;
        //Debug.Log("END INSRT");
    }
    string PersianDate(DateTime DateTime1)
    {
        System.Globalization.PersianCalendar PersianCalendar1 = new System.Globalization.PersianCalendar();
        return (PersianCalendar1.GetYear(DateTime1).ToString("0000") + "/" + PersianCalendar1.GetMonth(DateTime1).ToString("00") + "/" + PersianCalendar1.GetDayOfMonth(DateTime1).ToString("00"));
    }
    #endregion
}


