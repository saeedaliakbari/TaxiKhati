using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DokanSDK;
using CodeStage.AntiCheat.ObscuredTypes;

public class DokanApi : MonoBehaviour
{
    public Controller controller;
    public GameObject objSharj;
    // Use this for initialization
    void Start()
    {
        if (Dokan.IsAvailable())
        {
            objSharj.SetActive(true);
            Dokan.Init();
        }
        else
        {
            objSharj.SetActive(false);
        }
    }
    public void BtnSharj()
    {
        Dokan.StartPurchase(PurchaseOnCompleteCallback, PurchaseCancelCallback);
    }
    private void PurchaseOnCompleteCallback(string token, int status)
    {
        Debug.Log("Purchase successful token<" + token + "> status<" + status + ">");
        Dokan.CheckOrder(token, CheckOrderCallback, CheckOrderErrorCallback);
    }
    private void PurchaseCancelCallback()
    {
        Debug.Log("Purchase is canceled.");
        controller.panelMessage.SetActive(true);
        controller.txtPanelMessage.text = "خرید شارژ توسط کاربر لغو شد";
    }
    private void CheckOrderErrorCallback()
    {
        Debug.Log("Purchase Failed.");
        controller.panelMessage.SetActive(true);
        controller.txtPanelMessage.text = "خرید شارژ ناموفق بود";
    }
    private void CheckOrderCallback(OrderInfo orderInfo)
    {
        Debug.Log("CheckOrder: productName<" + orderInfo.productName + "> price<" + orderInfo.price + "> status<" + orderInfo.status + "> ");
        if (orderInfo.status == "COMPLETED")
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") + orderInfo.coin);
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "جایزه " + orderInfo.coin + " الماس اضافه شد";
            controller.SetText();
        }
    }
}
