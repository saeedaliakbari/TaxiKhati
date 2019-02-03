using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineEraning : MonoBehaviour
{
    public TrimNumberText valueTxt;
    [HideInInspector]
    public TrimNumberText txtCoin;
    public GameObject btnDouble;
    private int value;
    [HideInInspector]
    public bool doubleCoin = false;
    public void ShowEarning(int time, float offlineEarningRate)
    {
        value = (int)(Controller.instance.slotManager.EarningPerSec * time * offlineEarningRate / 100f);
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