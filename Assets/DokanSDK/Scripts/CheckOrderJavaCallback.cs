using UnityEngine;
using DokanSDK;

public class CheckOrderJavaCallback : AndroidJavaProxy
{
    private Dokan.CheckOrderCallback checkOrderCallback;
    private Dokan.CheckOrderErrorCallback checkOrderErrorCallback;
    public CheckOrderJavaCallback(Dokan.CheckOrderCallback checkOrderCallback, Dokan.CheckOrderErrorCallback checkOrderErrorCallback)
        : base("com.rahnema.dokan.sdk.callback.ICheckOrderCallback")
    {
        this.checkOrderCallback = checkOrderCallback;
        this.checkOrderErrorCallback = checkOrderErrorCallback;
    }

    public void onResponse(string orderInformationJson)
    {
        try
        {
            checkOrderCallback(JsonUtility.FromJson<OrderInfo>(orderInformationJson));
        }
        catch
        {
            checkOrderErrorCallback();
        }
    }
}
