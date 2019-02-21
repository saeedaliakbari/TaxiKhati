using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OfflineEraning : MonoBehaviour
{
    public Controller controller;
    public TrimNumberText valueTxt;
    [HideInInspector]
    public TrimNumberText txtCoin;
    public Button btnDouble, btnThird;
    private float value;
    [HideInInspector]
    public bool doubleCoin = false;
    private bool thirdCoin = false;
    public void ShowEarning(int time, float offlineEarningRate)
    {
        btnDouble.interactable = true;
        btnThird.interactable = true;
        if (time > 345600)//تایم بیشتر از 4 روز نباشد
        {
            time = 345600;
        }
        value = Controller.instance.slotManager.EarningPerSec * time * offlineEarningRate / 100f;
        value = value * PlayerPrefs.GetFloat("offlineEarnTycoonBoosts", 1) * PlayerPrefs.GetFloat("offliceEarnVip", 1);
        valueTxt.text = value.ToString();
    }

    public void ClaimClick()
    {
        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + value);
        txtCoin.text = PlayerPrefs.GetFloat("coin", 5000).ToString();
        Controller.instance.CloseOffEarning();
    }
    public void ThirdClick()
    {
        if (PlayerPrefs.GetFloat("gem") >= 5)
        {
            btnDouble.interactable = false;
            btnThird.interactable = false;
            PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") - 5);
            thirdCoin = true;
            controller.SetText();
            AnimValueChange();
        }
    }
    public void AnimValueChange()
    {
        if (doubleCoin)
        {
            value *= 2;
            valueTxt.text = value.ToString();
            doubleCoin = false;
        }
        else if (thirdCoin)
        {
            value *= 3;
            valueTxt.text = value.ToString();
            thirdCoin = false;
        }
    }
}