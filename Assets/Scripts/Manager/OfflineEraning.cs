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
    public Animator animPanel,CoinAnimator;
    private double value;
    public void ShowEarning(int time)
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 22)
        {
            btnDouble.interactable = true;
            btnThird.interactable = true;
            if (time > 21600)//تایم بیشتر از 6 ساعت نباشد
            {
                time = 21600;
            }
            float rate = ObscuredPrefs.GetFloat("offlineEarnTycoonBoosts", 1) + ObscuredPrefs.GetFloat("offliceEarnVip", 0);
            value = controller.slotManager.EarningPerSec * time;
            value = value * rate;
            valueTxt.text = value.ToString("0.##");
            if (value > 0)
            {
                gameObject.SetActive(true);//پنل آفلاین بدست آوردن سکه را فعال می کند
            }
        }
    }

    public void ClaimClick()
    {
        ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + value);
        ObscuredPrefs.SetDouble("coinTotal", ObscuredPrefs.GetDouble("coinTotal", 0) + value);
        txtCoin.text = ObscuredPrefs.GetDouble("coin", 5000).ToString("0.##");
        controller.CloseOffEarning();
        controller.SetText();
        CoinAnimator.Play("earn");
    }
    public void ThirdClick()
    {
        if (ObscuredPrefs.GetDouble("gem") >= 5)
        {
            btnDouble.interactable = false;
            btnThird.interactable = false;
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") - 5);
            controller.SetText();
            AnimValueChange(false);
            animPanel.Play("Close");
            controller.Close(gameObject);
        }
        else
        {
            controller.parkingManager.DisableCarInPark();
            controller.panelNoGem.SetActive(true);
            gameObject.SetActive(true);
        }
    }
    public void AnimValueChange(bool doubleCoin)
    {
        if (doubleCoin)
        {
            value *= 2;
            valueTxt.text = value.ToString("0.##");
        }
        else
        {
            value *= 3;
            valueTxt.text = value.ToString("0.##");
        }
        ClaimClick();

    }

    public void CounterCoin(double Earned,Text coinText)
    {
        StartCoroutine(counterCoinIEnumerator(Earned,coinText));
    }

    public IEnumerator counterCoinIEnumerator(double Earned,Text coinText)
    {
       yield return new WaitForSeconds(0f);
       for (int i = 0; i < 10; i++)
       {
           if (Earned<1000)
           {
               
           }
           else if (Earned < 100000)
           {
               
           }
           else if (Earned < 10000000)
           {

           }
           else if (Earned < 1000000000)
           {

           }
           else if (Earned < 100000000000)
           {

           }
           else if (Earned < 10000000000000)
           {

           }
           else if (Earned < 1000000000000000)
           {

           }
           else if (Earned < 100000000000000000)
           {

           }

        }
    }
}