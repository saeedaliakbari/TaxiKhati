using System;
using UnityEngine;

namespace DokanSDK
{
    public class Dokan
    {
        public delegate void PurchaseOnCompleteCallback(string token, int status);
        public delegate void PurchaseCancelCallback();
        public delegate void CheckOrderCallback(OrderInfo orderInfo);
        public delegate void CheckOrderErrorCallback();
        public static void Init()
        {
            try
            {
#if !UNITY_EDITOR
                DokanHelper().CallStatic("initialize", GetContext());
#endif
            }
            catch (Exception e)
            {
                Debug.LogError("*** DokanApi.Init failed: error<" + e.Message + ">");
            }
        }
        public static void StartPurchase(PurchaseOnCompleteCallback onCompleteCallback, PurchaseCancelCallback cancelCallback)
        {
#if UNITY_EDITOR
            if (RandomSuccess())
            {
                onCompleteCallback("Sample Token", 123456);
            }
            else
            {
                cancelCallback();
            }
#else
            DokanHelper().CallStatic("startPurchase", GetContext(), new PurchaseJavaCallback(onCompleteCallback, cancelCallback));
#endif
        }
        public static void CheckOrder(string token, CheckOrderCallback callback, CheckOrderErrorCallback errorCallback)
        {
#if UNITY_EDITOR
            if (RandomSuccess())
            {
                OrderInfo orderInfo = new OrderInfo
                {
                    coin = 100,
                    price = 1000,
                    productName = "Sample Product Name",
                    simOperator = "Sample Sim Operator",
                    status = "Success"
                };
                callback(orderInfo);
            }
            else
            {
                errorCallback();
            }
#else
            DokanHelper().CallStatic("checkOrder", GetContext(), token, new CheckOrderJavaCallback(callback, errorCallback));
#endif
        }
        private static AndroidJavaObject GetContext()
        {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }
        private static AndroidJavaObject DokanHelper()
        {
            return new AndroidJavaObject("com.rahnema.dokan.sdk.helper.DokanHelper");
        }
        private static bool RandomSuccess()
        {
            return UnityEngine.Random.Range(0, 5) < 4;
        }
        public static bool IsAvailable()
        {
            return GetAndroidVersion() >= 20;
        }
        public static int GetAndroidVersion()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var version = new
AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }
#elif UNITY_EDITOR
            return 21;
#endif
        }
    }
}
