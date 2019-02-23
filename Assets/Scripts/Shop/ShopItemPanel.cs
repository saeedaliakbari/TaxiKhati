using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    //public Controller controller;
    public RunSlotManager runSlotManager;
    public GameObject panelMessage;
    public Button itemShopCar;
    public Text txtPanelMessage, txtMessage, txtPriceShopCar, txtPriceUpIncome;
    [SerializeField]
    private float[] priceOffShopCar, offShopCar, priceUpIncome, upIncome;
    public void OpenPanel()
    {
        //PlayerPrefs.SetFloat("token", 1000000000);
        txtPriceShopCar.text = ((int)PlayerPrefs.GetFloat("priceShopCar", priceOffShopCar[0])).ToString() + " token";
        txtPriceUpIncome.text = ((int)PlayerPrefs.GetFloat("priceUpIncomeLine", priceUpIncome[0])).ToString() + " token";
        Debug.Log("Off NOW: " + PlayerPrefs.GetFloat("offCar", 1));
        Debug.Log("Income NOW: " + PlayerPrefs.GetFloat("incomeLine", 1));
        if (PlayerPrefs.GetFloat("offCar", 1) == 0.15f)
        {
            itemShopCar.interactable = false;
            txtPriceShopCar.text = "FULL";
        }
    }
    public void BtnOffShopCar()
    {
        if (PlayerPrefs.GetFloat("token") >= (int)PlayerPrefs.GetFloat("priceShopCar", priceOffShopCar[0]) && PlayerPrefs.GetFloat("offCar", 1) >= 0.15f)
        {
            Debug.Log("off is Start");
            PlayerPrefs.SetFloat("token", PlayerPrefs.GetFloat("token") - (int)PlayerPrefs.GetFloat("priceShopCar", priceOffShopCar[0]));
            PlayerPrefs.SetInt("offShopCarLevel", PlayerPrefs.GetInt("offShopCarLevel", 0) + 1);
            int level = PlayerPrefs.GetInt("offShopCarLevel", 0);
            PlayerPrefs.SetFloat("priceShopCar", priceOffShopCar[level]);
            PlayerPrefs.SetFloat("offCar", 1 - offShopCar[level - 1]);
            OpenPanel();
        }
        else
        {
            Controller.instance.txtError.text = "به مقدار کافی توکن ندارید";
            Controller.instance.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                Controller.instance.txtError.gameObject.SetActive(false);
            });
        }
    }
    public void BtnUpIncomeLine()
    {
        if (PlayerPrefs.GetFloat("token") >= (int)PlayerPrefs.GetFloat("priceUpIncomeLine", priceUpIncome[0]))
        {
            Debug.Log("up income line start:" + (int)PlayerPrefs.GetFloat("priceUpIncomeLine", priceUpIncome[0]));
            PlayerPrefs.SetFloat("token", PlayerPrefs.GetFloat("token") - (int)PlayerPrefs.GetFloat("priceUpIncomeLine", priceUpIncome[0]));
            PlayerPrefs.SetInt("upIncomeLevel", PlayerPrefs.GetInt("upIncomeLevel", 0) + 1);
            int level = PlayerPrefs.GetInt("upIncomeLevel", 0);
            PlayerPrefs.SetFloat("priceUpIncomeLine", priceUpIncome[level]);
            PlayerPrefs.SetFloat("incomeLine", upIncome[level - 1]);
            runSlotManager.UpdateEarningSpeedText();
            OpenPanel();
        }
        else
        {
            Controller.instance.txtError.text = "به مقدار کافی توکن ندارید";
            Controller.instance.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                Controller.instance.txtError.gameObject.SetActive(false);
            });
        }
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
            PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + (runSlotManager.earnPerSec * houers * 60 * 60));
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") - Gem);
            Controller.instance.SetText();
            Controller.instance.txtError.text = Manager.ChangeNumber(runSlotManager.earnPerSec * houers * 60 * 60) + "سکه اضافه شد";
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
