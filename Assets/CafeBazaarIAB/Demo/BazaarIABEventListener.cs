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
#if UNITY_ANDROID

    void OnEnable()
    {
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
        BazaarIAB.enableLogging(true);//فعال سازی اطلاعات فراخوانی توابع در زمان اشکال زدایی
        Debug.Log("billingSupportedEvent");
        str = new string[iapCafeBazar.skus.Length];
        for (int i = 0; i < iapCafeBazar.skus.Length; i++)
        {
            str[i] = iapCafeBazar.skus[i];
        }
        BazaarIAB.querySkuDetails(str);//برای گرفتن اطلاعات محصولاتی که در پنل پرداخت درون برنامه ای تعریف کرده اید مثل قیمت، عنوان و … باید از این تابع استفاده کنید. برای استفاده از این تابع نیازی نیست که کاربر حتما در برنامه‌ی بازار لاگین کرده باشد.
        BazaarIAB.queryInventory(str);
    }
    void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
        BazaarIAB.init(iapCafeBazar.RSA);
    }
    #endregion
    #region Inventory
    void queryInventorySucceededEvent(List<BazaarPurchase> purchases, List<BazaarSkuInfo> skus)
    {
        PlayerPrefs.SetInt("num_of_places_vip", 0);
        PlayerPrefs.SetInt("num_of_slot_vip", 0);
        PlayerPrefs.SetFloat("offliceEarnVip", 1);
        PlayerPrefs.SetInt("removeAds", 0);
        PlayerPrefs.SetInt("gemPerDay", 0);
        PlayerPrefs.SetFloat("speedVip", 1);
        //Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
        for (int i = 0; i < purchases.Count; ++i)
        {
            if (purchases[i].ProductId == iapCafeBazar.skus[6])
            {
                //بقیه اطلاعات مربوط به اشتراک وارد شود
                PlayerPrefs.SetInt("num_of_places_vip", 2);//added 2 parking
                PlayerPrefs.SetInt("num_of_slot_vip", 2);//added 2 line
                iapCafeBazar.controller.parkingManager.SpawnPlaces();
                iapCafeBazar.controller.slotManager.InitSlots();
                PlayerPrefs.SetFloat("offliceEarnVip", 1.2f);//added 20% offline earning
                PlayerPrefs.SetInt("removeAds", 1);//remove ads in shop
                PlayerPrefs.SetInt("gemPerDay", 1);//10 gem per day
                PlayerPrefs.SetFloat("speedVip", 1.5f);//added 50% speed
                iapCafeBazar.controller.slotManager.UpdateEarningSpeedText();
                iapCafeBazar.controller.GiftDaily();
            }
            else if (purchases[i].ProductId == iapCafeBazar.skus[7])
            {
                PlayerPrefs.SetInt("removeAds", 1);//remove ads in shop
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
        for (int i = 0; i < skus.Count - 2; ++i)
        {
            //txtTitle[i].text = skus[i].Title;
            //txtPrice[i].text = skus[i].Price;
            Debug.Log("i>" + i + ":" + skus[i].Title + ">" + skus[i].Price);
        }

    }

    private void querySkuDetailsFailedEvent(string error)
    {
        Debug.Log("querySkuDetailsFailedEvent: " + error);
        BazaarIAB.querySkuDetails(str);
    }
    #endregion
    #region Baray Chand Kharid
    private void queryPurchasesSucceededEvent(List<BazaarPurchase> purchases)
    {
        Debug.Log(string.Format("queryPurchasesSucceededEvent. total purchases: {0}", purchases.Count));

        for (int i = 0; i < purchases.Count; ++i)
        {
            Debug.Log(purchases[i].ToString());
        }
    }

    private void queryPurchasesFailedEvent(string error)
    {
        Debug.Log("queryPurchasesFailedEvent: " + error);
    }
    #endregion
    #region Purchase
    void purchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("purchaseSucceededEvent: "/* + purchase*/);
        Debug.Log("developerPayload" + purchase.DeveloperPayload);
        Debug.Log("Order ID: " + purchase.OrderId);
        Debug.Log("Time Kharid : " + purchase.PurchaseTime);
        Debug.Log("Token Yekta Kharid: " + purchase.PurchaseToken);
        Debug.Log("Type; " + purchase.Type);
        Debug.Log("PurchaseState; " + purchase.PurchaseState);
        if (purchase.DeveloperPayload == ObscuredPrefs.GetString("developerPayload"))
        {
            ObscuredPrefs.SetString("developerPayload", "");
            Debug.Log("developerPayload is Ok");
            if (purchase.PurchaseState == BazaarPurchase.BazaarPurchaseState.Purchased)
            {
                if (purchase.ProductId == iapCafeBazar.skus[6])
                {
                    PlayerPrefs.SetInt("num_of_places_vip", 2);
                    iapCafeBazar.controller.panelMessage.SetActive(true);
                    iapCafeBazar.controller.txtPanelMessage.text = "پارکینگ به خطوط شما اضافه شد";
                    iapCafeBazar.controller.parkingManager.SpawnNewPlace();
                    iapCafeBazar.controller.parkingManager.SpawnNewPlace();
                    iapCafeBazar.controller.parkingManager.UpdatePlacePosition();
                    PlayerPrefs.SetInt("num_of_slot_vip", 2);//added 2 line
                    iapCafeBazar.controller.slotManager.SpawnASlot();
                    iapCafeBazar.controller.slotManager.SpawnASlot();
                    iapCafeBazar.controller.slotManager.UpdatePosition();
                    PlayerPrefs.SetFloat("offliceEarnVip", 1.2f);//added 20% offline earning
                    PlayerPrefs.SetInt("removeAds", 1);//remove ads in shop
                    PlayerPrefs.SetInt("gemPerDay", 1);//10 gem per day
                    PlayerPrefs.SetFloat("speedVip", 1.5f);//added 50% speed
                    iapCafeBazar.controller.slotManager.UpdateEarningSpeedText();
                    iapCafeBazar.controller.GiftDaily();
                }
                else if (purchase.ProductId == iapCafeBazar.skus[7])
                {
                    PlayerPrefs.SetInt("removeAds", 1);//remove ads in shop
                    iapCafeBazar.controller.panelMessage.SetActive(true);
                    iapCafeBazar.controller.txtPanelMessage.text = "تبلیغات بنری بازی حذف شد";
                    iapCafeBazar.controller.videoAds.panelNoAds.SetActive(false);
                }
                else {
                    iapCafeBazar.controller.panelMessage.SetActive(true);
                    iapCafeBazar.controller.txtPanelMessage.text = "لطفا منتظر بمانید";
                    BazaarIAB.queryInventory(new string[] { purchase.ProductId });
                }
            }
            else if (purchase.PurchaseState == BazaarPurchase.BazaarPurchaseState.Canceled)
            {
                Debug.Log("purchase is Canceled");
                iapCafeBazar.controller.panelMessage.SetActive(true);
                iapCafeBazar.controller.txtPanelMessage.text = "عملیات توسط شما لغو شد";
            }
            else
            {
                Debug.Log("purchase is 2 Refunded");
                iapCafeBazar.controller.panelMessage.SetActive(true);
                iapCafeBazar.controller.txtPanelMessage.text = "خطا در عملیات پرداخت";
            }
        }
    }

    void purchaseFailedEvent(string error)
    {
        Debug.Log("purchaseFailedEvent: " + error);
        iapCafeBazar.controller.txtPanelMessage.text = "خطا در پرداخت ";
        iapCafeBazar.controller.panelMessage.SetActive(true);
    }
    #endregion
    #region Consume Purchase
    void consumePurchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);
        for (int i = 0; i < iapCafeBazar.skus.Length; i++)
        {
            if (i != 1)//Eshteraki nemitavanim masraf konim
            {
                if (purchase.ProductId == iapCafeBazar.skus[i])
                {
                    ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") + iapCafeBazar.gem[i]);
                    iapCafeBazar.controller.txtPanelMessage.text = "تبریک\n" + iapCafeBazar.gem[i] + " جم اضافه شد";
                    iapCafeBazar.controller.panelMessage.SetActive(true);
                    iapCafeBazar.controller.SetText();
                }
            }
        }
    }

    void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
        BazaarIAB.queryInventory(str);
    }
    #endregion
#endif

}


