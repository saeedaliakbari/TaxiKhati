using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExchangeToken : MonoBehaviour
{
    public Controller controller;
    public InputField inputTokenValue;
    public TrimNumberText txtCoinValue;
    private float rate, tokenValue;
    private float coin;
    // Use this for initialization
    public Button btnExchange;
    public void SetTxtTokenValue()
    {
        rate = (1 - PlayerPrefs.GetFloat("exchangeDeclineTycoon", 0)) * 10000;
        try
        {
            coin = Mathf.Floor(float.Parse(inputTokenValue.text) * rate);
            txtCoinValue.text = coin.ToString();
            if (coin > ObscuredPrefs.GetDouble("coin", 5000))
            {
                btnExchange.interactable = false;
            }
            else
            {
                btnExchange.interactable = true;
            }

        }
        catch (System.Exception)
        {
            txtCoinValue.text = "0";
        }

    }
    public void BtnExchangeToken()
    {
        tokenValue = Mathf.Floor(float.Parse(inputTokenValue.text) * rate);
       ObscuredPrefs.SetDouble("token",ObscuredPrefs.GetDouble("token", 0) + tokenValue);
        ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) - (tokenValue * rate));
        controller.SetText();
    }
    public void BtnPlus(int i)
    {
        try
        {
            inputTokenValue.text = (float.Parse(inputTokenValue.text) + i).ToString();
        }
        catch (System.Exception)
        {
            inputTokenValue.text = "1";
        }
    }
}
