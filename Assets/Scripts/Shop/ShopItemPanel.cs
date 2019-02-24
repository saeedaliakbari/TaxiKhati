using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    private float earnPerSec;
    public TrimNumberText txtPack1, txtPack2, txtPack3;
    public void OpenPanel()
    {
        earnPerSec = Controller.instance.slotManager.earnPerSec;
        txtPack1.text = (earnPerSec * 4 * 60 * 60).ToString();
        txtPack2.text = (earnPerSec * 24 * 60 * 60).ToString();
        txtPack3.text = (earnPerSec * 24 * 4 * 60 * 60).ToString();
    }
    public void BtnTimeBoosts(int Gem)
    {
        if (PlayerPrefs.GetFloat("gem") >= Gem)
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
            PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + (earnPerSec * houers * 60 * 60));
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") - Gem);
            Controller.instance.SetText();
            Controller.instance.txtError.text = Manager.ChangeNumber(earnPerSec * houers * 60 * 60) + "سکه اضافه شد";
            Controller.instance.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 1.5f, () =>
            {
                Controller.instance.txtError.gameObject.SetActive(false);
            });
        }
        else
        {
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") + Gem);
            Controller.instance.txtError.text = "به مقدار کافی جم ندارید";
            Controller.instance.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                Controller.instance.txtError.gameObject.SetActive(false);
            });
        }
    }
}
