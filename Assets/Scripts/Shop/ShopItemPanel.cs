using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    public Controller controller;
    private double earnPerSec;
    public TrimNumberText txtPack1, txtPack2, txtPack3;
    public void OpenPanel()
    {
        earnPerSec = controller.slotManager.earnPerSec;
        //Debug.Log("earnPerSec: " + earnPerSec);
        txtPack1.text = (earnPerSec * 4 * 60 * 60).ToString("0.##");
        txtPack2.text = (earnPerSec * 24 * 60 * 60).ToString("0.##");
        txtPack3.text = (earnPerSec * 24 * 4 * 60 * 60).ToString("0.##");
    }
    public void BtnTimeBoosts(int Gem)
    {
        if (ObscuredPrefs.GetDouble("gem") >= Gem)
        {
            //Debug.Log("Buy Time");
            int hours = 4;
            if (Gem == 225)
            {
                hours = 24;
            }
            else if (Gem == 400)
            {
                hours = 96;
            }
            ////Debug.Log("Plus Coin : " + runSlotManager.earnPerSec * houers * 60 * 60 + ">>" + controller.txtCoin.text);
            double timeWrap = earnPerSec * hours * 60 * 60;
            ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + timeWrap);
            ObscuredPrefs.SetDouble("coinTotal", ObscuredPrefs.GetDouble("coinTotal", 0) + timeWrap);
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") - Gem);
            controller.SetText();
            controller.txtError.text = Manager.ChangeNumber(timeWrap) + "سکه اضافه شد";
            controller.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 1.5f, () =>
            {
                controller.txtError.gameObject.SetActive(false);
            });
        }
        else
        {
            //controller.txtError.text = "به مقدار کافی الماس ندارید";
            controller.panelNoGem.SetActive(true);
            controller.parkingManager.DisableCarInPark();
            //Timer.Schedule(this, 3f, () =>
            //{
            //    controller.txtError.gameObject.SetActive(false);
            //});
        }
    }
}
