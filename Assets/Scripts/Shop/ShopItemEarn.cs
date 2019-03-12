using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemEarn : MonoBehaviour
{
    public ShopItem[] itemsEarn;
    public RunSlotManager runSlotManager;
    public Controller controller;
    public float[] upIncome;
    public void OpenPanel()
    {
        //-1976.035
        int levelEarn = ObscuredPrefs.GetInt("upIncomeLevel", 0);
        for (int i = 0; i < levelEarn; i++)
        {
            itemsEarn[i].btnBuy.gameObject.SetActive(false);
            itemsEarn[i].imgActive.gameObject.SetActive(true);
            itemsEarn[i].imgLock.gameObject.SetActive(false);
            //Debug.Log("upIncomeLevel i= " + i + " > Is Faal");
        }
        for (int i = levelEarn + 1; i < itemsEarn.Length; i++)
        {
            itemsEarn[i].btnBuy.gameObject.SetActive(false);
            itemsEarn[i].imgActive.gameObject.SetActive(false);
            itemsEarn[i].imgLock.gameObject.SetActive(true);
            //Debug.Log("upIncomeLevel i= " + i + " > Is Lock");
        }
        itemsEarn[levelEarn].btnBuy.gameObject.SetActive(true);
        itemsEarn[levelEarn].imgActive.gameObject.SetActive(false);
        itemsEarn[levelEarn].imgLock.gameObject.SetActive(false);
        //Debug.Log("upIncomeLevel i= " + levelEarn + " > Is Buy");
    }
    public void BtnUpEarnLine(float price)
    {
        if (ObscuredPrefs.GetDouble("token") >= price)
        {
            ObscuredPrefs.SetDouble("token", ObscuredPrefs.GetDouble("token") - price);
            ObscuredPrefs.SetInt("upIncomeLevel", ObscuredPrefs.GetInt("upIncomeLevel", 0) + 1);
            int level = ObscuredPrefs.GetInt("upIncomeLevel", 0);
            ObscuredPrefs.SetFloat("incomeLine", upIncome[level - 1]);
            runSlotManager.UpdateEarningSpeedText();
            StartCoroutine(controller.IEEarningRatio());
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
