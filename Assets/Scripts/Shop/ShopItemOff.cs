using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemOff : MonoBehaviour {
    public ShopItem[] itemsOff;
    public RunSlotManager runSlotManager;
    //public Controller controller;
    public float[] offShopCar;
    public void OpenPanel()
    {
        int levelOff = PlayerPrefs.GetInt("offShopCarLevel", 0);
        for (int i = 0; i < levelOff; i++)
        {
            itemsOff[i].btnBuy.gameObject.SetActive(false);
            itemsOff[i].imgActive.gameObject.SetActive(true);
            itemsOff[i].imgLock.gameObject.SetActive(false);
            //Debug.Log("offShopCarLevel i= " + i + " > Is Faal");
        }
        for (int i = levelOff + 1; i < itemsOff.Length; i++)
        {
            itemsOff[i].btnBuy.gameObject.SetActive(false);
            itemsOff[i].imgActive.gameObject.SetActive(false);
            itemsOff[i].imgLock.gameObject.SetActive(true);
            //Debug.Log("offShopCarLevel i= " + i + " > Is Lock");
        }
        itemsOff[levelOff].btnBuy.gameObject.SetActive(true);
        itemsOff[levelOff].imgActive.gameObject.SetActive(false);
        itemsOff[levelOff].imgLock.gameObject.SetActive(false);
        //Debug.Log("offShopCarLevel i= " + levelOff + " > Is Buy");
    }
    public void BtnOff(float price)
    {
        if (PlayerPrefs.GetFloat("token") >= price)
        {
            PlayerPrefs.SetFloat("token", PlayerPrefs.GetFloat("token") - price);
            PlayerPrefs.SetInt("offShopCarLevel", PlayerPrefs.GetInt("offShopCarLevel", 0) + 1);
            int level = PlayerPrefs.GetInt("upIncomeLevel", 0);
            PlayerPrefs.SetFloat("offCar", offShopCar[level - 1]);
            runSlotManager.UpdateEarningSpeedText();
            OpenPanel();
        }
        else
        {
            Controller.instance.txtError.text = "به مقدار کافی پول ندارید";
            Controller.instance.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                Controller.instance.txtError.gameObject.SetActive(false);
            });
        }
    }
}
