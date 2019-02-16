using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineEraning : MonoBehaviour
{
    public TrimNumberText valueTxt;
    [HideInInspector]
    public TrimNumberText txtCoin;
    public GameObject btnDouble;
    private float value;
    [HideInInspector]
    public bool doubleCoin = false;
    public void ShowEarning(int time, float offlineEarningRate)
    {
        if (time>345600)//تایم بیشتر از 4 روز نباشد
        {
            time = 345600;
        }
        value = Controller.instance.slotManager.EarningPerSec * time * offlineEarningRate / 100f;
        value = value * PlayerPrefs.GetFloat("offlineEarnTycoonBoosts", 1)* PlayerPrefs.GetFloat("offliceEarnVip", 1);
        valueTxt.text = value.ToString();
    }

    public void ClaimClick()
    {
        if (doubleCoin)
        {
            value += value;
            valueTxt.text = value.ToString();
            doubleCoin = false;
        }
        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + value);
        txtCoin.text = PlayerPrefs.GetFloat("coin", 5000).ToString();
        Controller.instance.CloseOffEarning();
    }
}