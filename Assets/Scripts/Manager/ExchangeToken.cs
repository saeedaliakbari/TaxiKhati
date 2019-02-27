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
    public Text txtRate;
    private float rate;
    private double coin, tokenValue;
    // Use this for initialization
    public Button btnExchange;
    public void SetTxtTokenValue()
    {
        rate = (1 - PlayerPrefs.GetFloat("exchangeDeclineTycoon", 0)) * 10000;
        txtRate.text = "(" + rate.ToString();
        try
        {
            coin = System.Math.Floor(double.Parse(inputTokenValue.text) * rate);
            txtCoinValue.text = coin.ToString("0.##");
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
        tokenValue = double.Parse(inputTokenValue.text);
        ObscuredPrefs.SetDouble("token", ObscuredPrefs.GetDouble("token", 0) + tokenValue);
        ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) - (tokenValue * rate));
        controller.SetText();
    }
    public void BtnPlus(int i)
    {
        try
        {
            //double coin = System.Math.Floor((double.Parse(inputTokenValue.text) + i) * rate);
            //if (coin <= ObscuredPrefs.GetDouble("coin", 5000))
            //{
            double token = double.Parse(inputTokenValue.text);
            Debug.Log("token: " + (token + 1));
            if (token + i > 1)
            {
                inputTokenValue.text = (token + i).ToString();
            }
            else
            {
                inputTokenValue.text = "1";
            }

            //}
        }
        catch (System.Exception)
        {
            inputTokenValue.text = "1";
        }
    }
    public void MaxValue()
    {
        double token = System.Math.Floor(ObscuredPrefs.GetDouble("coin", 5000) / rate);
        Debug.Log("Token : " + token);
        inputTokenValue.text = token.ToString();
        Debug.Log("Token : " + inputTokenValue.text);
        txtCoinValue.text = (token * rate).ToString("0.##");
        Debug.Log(token * rate);

    }
    public void MinValue()
    {
        inputTokenValue.text = "1";
    }
}
