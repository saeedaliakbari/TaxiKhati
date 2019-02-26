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
        Debug.Log("earnPerSec: " + earnPerSec);
        txtPack1.text = (earnPerSec * 4 * 60 * 60).ToString();
        txtPack2.text = (earnPerSec * 24 * 60 * 60).ToString();
        txtPack3.text = (earnPerSec * 24 * 4 * 60 * 60).ToString();
    }
    public void BtnTimeBoosts(int Gem)
    {
        if (ObscuredPrefs.GetDouble("gem") >= Gem)
        {
            Debug.Log("Buy Time");
            int houers = 4;
            if (Gem == 225)
            {
                houers = 24;
            }
            else if (Gem == 400)
            {
                houers = 48;
            }
            //Debug.Log("Plus Coin : " + runSlotManager.earnPerSec * houers * 60 * 60 + ">>" + controller.txtCoin.text);
            ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + (earnPerSec * houers * 60 * 60));
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") - Gem);
            controller.SetText();
            controller.txtError.text = Manager.ChangeNumber(earnPerSec * houers * 60 * 60) + "سکه اضافه شد";
            controller.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 1.5f, () =>
            {
                controller.txtError.gameObject.SetActive(false);
            });
        }
        else
        {
            controller.txtError.text = "به مقدار کافی جم ندارید";
            controller.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                controller.txtError.gameObject.SetActive(false);
            });
        }
    }
}
