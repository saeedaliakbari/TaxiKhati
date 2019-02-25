using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BazaarPlugin;
using CodeStage.AntiCheat.ObscuredTypes;

public class IAPCafeBazar : MonoBehaviour
{
    public ObscuredString RSA;
    public ObscuredString[] skus;
    public int[] gem;
    public Controller controller;
    // Use this for initialization
    void Start()
    {
        BazaarIAB.init(RSA);//در صورت موفقیت رخداد
                            //billingSupportedEvent
                            //فراخوانی می شود و در صورت عدم موفقیت رخداد
                            //billingNotSupportedEvent
                            //فراخوانی خواهد شد.
                            //BazaarIAB.unbindService();//زمانی که کارتان با پرداخت درون برنامه ای تمام شد این تابع را فراخوانی کنید
                            //BazaarIAB.areSubscriptionsSupported();//برای بررسی این که خرید اشتراک های ماهانه و سالانه پشتیبانی می شود یا خیر از این تابع استفاده کنید

    }

    public void BtnPurchase(string sku)
    {

        Debug.Log("btn Purchase: " + sku);
        ObscuredPrefs.SetString("developerPayload", Random.Range(10000, 99999).ToString() + Random.Range(10000, 99999).ToString());
        BazaarIAB.purchaseProduct(sku, ObscuredPrefs.GetString("developerPayload"));
        //purchaseSucceededEvent
        //است که زمانی که خرید موفقیت آمیز بود فراخوانی می شود
        //یا زمانی که تلاش می‌کنید محصولی که قبلا خریده‌اید ولی مصرف نکرده‌اید را دوباره بخرید.اگر هم خرید ناموفق باشد رخداد 
        //purchaseFailedEvent 
        //فراخوانی خواهد شد.خریدهایی که انجام می‌شوند به داخل 
        //Inventory
        //خواهند رفت که اگر محصول مصرفی باشد باید بعد موفقیت فرآیند خرید آن را با استفاده از تابع 
        //consumeProduct 
        //مصرف کنید.
    }
    public void BtnPurchaseEshterak(string sku)
    {
        ObscuredPrefs.SetString("developerPayload", ObscuredPrefs.GetString("id", "saeedaliakbari") + Random.Range(10000, 99999).ToString());
        BazaarIAB.purchaseProduct(sku, ObscuredPrefs.GetString("developerPayload"));
    }
}
