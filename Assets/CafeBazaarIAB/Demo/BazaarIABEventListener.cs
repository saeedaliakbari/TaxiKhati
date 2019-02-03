using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BazaarPlugin;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class BazaarIABEventListener : MonoBehaviour
{
    public Text[] txtPrice, txtTitle;
    public GameObject panelMessage;
    public Text txtGem;
    public Text txtMessage, txtPanelMessage;
    public IAPCafeBazar iapCafeBazar;
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
    void billingSupportedEvent()
    {
        BazaarIAB.enableLogging(true);//فعال سازی اطلاعات فراخوانی توابع در زمان اشکال زدایی
        Debug.Log("billingSupportedEvent");
        string[] str = new string[iapCafeBazar.skus.Length];
        for (int i = 0; i < iapCafeBazar.skus.Length; i++)
        {
            str[i] = iapCafeBazar.skus[i];
        }
        BazaarIAB.querySkuDetails(str);//برای گرفتن اطلاعات محصولاتی که در پنل پرداخت درون برنامه ای تعریف کرده اید مثل قیمت، عنوان و … باید از این تابع استفاده کنید. برای استفاده از این تابع نیازی نیست که کاربر حتما در برنامه‌ی بازار لاگین کرده باشد.
        Debug.LogError("Sefareshat");
        BazaarIAB.queryInventory(str);
    }
    void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
    }

    void queryInventorySucceededEvent(List<BazaarPurchase> purchases, List<BazaarSkuInfo> skus)
    {
        Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));

        for (int i = 0; i < purchases.Count; ++i)
        {
            Debug.Log(purchases[i].ToString());
        }

        Debug.Log("-----------------------------");

        for (int i = 0; i < skus.Count; ++i)
        {
            Debug.Log(skus[i].ToString());
        }
    }

    void queryInventoryFailedEvent(string error)
    {
        Debug.Log("queryInventoryFailedEvent: " + error);
    }

    private void querySkuDetailsSucceededEvent(List<BazaarSkuInfo> skus)
    {
        Debug.Log(string.Format("querySkuDetailsSucceededEvent. total skus: {0}", skus.Count));
        for (int i = 0; i < skus.Count; ++i)
        {
            txtTitle[i].text = skus[i].Title;
            txtPrice[i].text = skus[i].Price;
            Debug.Log(skus[i].ToString());
        }

    }

    private void querySkuDetailsFailedEvent(string error)
    {
        Debug.Log("querySkuDetailsFailedEvent: " + error);
    }

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

    void purchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("purchaseSucceededEvent: "/* + purchase*/);
        Debug.Log("developerPayload"+ purchase.DeveloperPayload);
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
                Debug.Log("purchase is Purchased OK");
                if (purchase.Type == "inapp")
                {
                    Debug.Log("inapp purchase");
                    txtMessage.text = "purchase is Purchased OK" + "inapp purchase" + " Add Gem: 10";
                    //BazaarIAB.consumeProduct(purchase.ProductId);
                    PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") + 10);
                    txtGem.text = PlayerPrefs.GetFloat("gem").ToString();
                }
                else
                {
                    txtMessage.text = "purchase is Purchased OK" + "subs purchase";
                    Debug.Log("subs purchase");
                }
            }
            else if (purchase.PurchaseState == BazaarPurchase.BazaarPurchaseState.Canceled)
            {
                Debug.Log("purchase is Canceled");
            }
            else
            {
                Debug.Log("purchase is 2 Refunded");
            }
            txtPanelMessage.text = purchase.PurchaseState.ToString();
            panelMessage.SetActive(true);
            BazaarIAB.consumeProduct(purchase.ProductId);
        }
    }

    void purchaseFailedEvent(string error)
    {
        Debug.Log("purchaseFailedEvent: " + error);
    }

    void consumePurchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);
    }

    void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
    }

#endif

}


