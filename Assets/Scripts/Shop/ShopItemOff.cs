using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemOff : MonoBehaviour
{
    public ShopItem[] itemsOff;
    public RunSlotManager runSlotManager;
    public Controller controller;
    public float[] offShopCar;
    public void OpenPanel()
    {
        int levelOff = ObscuredPrefs.GetInt("offShopCarLevel", 0);
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
        if (levelOff < 10)
        {
            itemsOff[levelOff].btnBuy.gameObject.SetActive(true);
            itemsOff[levelOff].imgActive.gameObject.SetActive(false);
            itemsOff[levelOff].imgLock.gameObject.SetActive(false);
        }
        //Debug.Log("offShopCarLevel i= " + levelOff + " > Is Buy");
    }
    public void BtnOff(float price)
    {
        if (ObscuredPrefs.GetDouble("token") >= price)
        {
            ObscuredPrefs.SetDouble("token", ObscuredPrefs.GetDouble("token") - price);
            ObscuredPrefs.SetInt("offShopCarLevel", ObscuredPrefs.GetInt("offShopCarLevel", 0) + 1);
            int level = ObscuredPrefs.GetInt("offShopCarLevel", 0);
            Debug.Log("level off is update : " + level + " off= " + offShopCar[level - 1]);
            ObscuredPrefs.SetFloat("offCar", offShopCar[level - 1]);
            runSlotManager.UpdateEarningSpeedText();
            OpenPanel();
        }
        else
        {
            controller.txtError.text = "به مقدار کافی پول ندارید";
            controller.txtError.gameObject.SetActive(true);
            Timer.Schedule(this, 3f, () =>
            {
                controller.txtError.gameObject.SetActive(false);
            });
        }
    }
}
