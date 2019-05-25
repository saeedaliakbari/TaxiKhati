using UnityEngine;
using DokanSDK;

public class PurchaseJavaCallback : AndroidJavaProxy
{
    private Dokan.PurchaseOnCompleteCallback callback;
    private Dokan.PurchaseCancelCallback calcelCallback;
    public PurchaseJavaCallback(Dokan.PurchaseOnCompleteCallback callback, Dokan.PurchaseCancelCallback calcelCallback)
        : base("com.rahnema.dokan.sdk.callback.PurchaseCallback")
    {
        this.callback = callback;
        this.calcelCallback = calcelCallback;
    }

    public void onComplete(string token, int status)
    {
        callback(token, status);
    }

    public void onCancel()
    {
        calcelCallback();
    }
}
