using CodeStage.AntiCheat.ObscuredTypes;
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
    private double value;
    [HideInInspector]
    public bool doubleCoin = false;
    private bool thirdCoin = false;
    public void ShowEarning(int time, float offlineEarningRate)
    {
        btnDouble.interactable = true;
        btnThird.interactable = true;
        if (time > 21600)//تایم بیشتر از 4 روز نباشد
        {
            time = 21600;
        }
        value = controller.slotManager.EarningPerSec * time * offlineEarningRate / 100f;
        value = value * ObscuredPrefs.GetFloat("offlineEarnTycoonBoosts", 1) * ObscuredPrefs.GetFloat("offliceEarnVip", 1);
        valueTxt.text = value.ToString("0.##");
        if (value > 0)
        {
            gameObject.SetActive(true);//پنل آفلاین بدست آوردن سکه را فعال می کند
        }
    }

    public void ClaimClick()
    {
        ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + value);
        ObscuredPrefs.SetDouble("coinTotal", ObscuredPrefs.GetDouble("coinTotal", 0) + value);
        txtCoin.text = ObscuredPrefs.GetDouble("coin", 5000).ToString("0.##");
        controller.CloseOffEarning();
        controller.SetText();
    }
    public void ThirdClick()
    {
        if (ObscuredPrefs.GetDouble("gem") >= 5)
        {
            btnDouble.interactable = false;
            btnThird.interactable = false;
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") - 5);
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
            valueTxt.text = value.ToString("0.##");
            doubleCoin = false;
        }
        else if (thirdCoin)
        {
            value *= 3;
            valueTxt.text = value.ToString("0.##");
            thirdCoin = false;
        }
    }
}