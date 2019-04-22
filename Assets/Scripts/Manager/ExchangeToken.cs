using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExchangeToken : MonoBehaviour
{
    public Controller controller;
    public InputField inputTokenValue;
    public TrimNumberText txtCoinValue, txtTokenValue;
    public Text txtRate;
    private float rate;
    private double coin, tokenValue;
    // Use this for initialization
    public Button btnExchange;
    public void SetTxtTokenValue()
    {
        rate = (1 - ObscuredPrefs.GetFloat("exchangeDeclineTycoon", 0)) * 10000;
        txtRate.text = "(" + rate.ToString();
        try
        {
            if (inputTokenValue.text.Split(' ').Length == 2)
            {
                tokenValue = Convertor(double.Parse(inputTokenValue.text.Split(' ')[0]), inputTokenValue.text.Split(' ')[1]);
            }
            else
            {
                tokenValue = Convertor(double.Parse(inputTokenValue.text), "o");
            }
            double maxToken = System.Math.Floor(ObscuredPrefs.GetDouble("coin", 5000) / rate);
            if (maxToken < tokenValue)
            {
                tokenValue = maxToken;
            }
            coin = System.Math.Floor(tokenValue * rate);
            txtCoinValue.text = coin.ToString("0.##");
            //Debug.Log("coin: " + coin + " >>" + txtCoinValue.text);
            if (coin > ObscuredPrefs.GetDouble("coin", 5000))
            {
                btnExchange.interactable = false;
            }
            else
            {
                if (coin == 0)
                {
                    btnExchange.interactable = false;
                }
                else
                {
                    btnExchange.interactable = true;
                }
            }
        }
        catch (System.Exception)
        {
            txtCoinValue.text = "0";
        }
    }
    public void BtnExchangeToken()
    {
        if (inputTokenValue.text.Split(' ').Length == 2)
        {
            tokenValue = Convertor(double.Parse(inputTokenValue.text.Split(' ')[0]), inputTokenValue.text.Split(' ')[1]);
        }
        else
        {
            tokenValue = Convertor(double.Parse(inputTokenValue.text), "o");
        }
        double maxToken = System.Math.Floor(ObscuredPrefs.GetDouble("coin", 5000) / rate);
        if (maxToken < tokenValue)
        {
            tokenValue = maxToken;
        }
        //Debug.Log("token value: " + tokenValue + ">>>" + inputTokenValue.text);
        ObscuredPrefs.SetDouble("token", ObscuredPrefs.GetDouble("token", 0) + tokenValue);
        ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) - (tokenValue * rate));
        SetTxtTokenValue();
        MinValue();
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

            //Debug.Log("token: " + (token + 1));
            if (token + i > 1)
            {
                inputTokenValue.text = (token + i).ToString();
            }
            else
            {
                inputTokenValue.text = "1";
            }
            SetTxtTokenValue();
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
        txtTokenValue.text = token.ToString();
        inputTokenValue.text = txtTokenValue.text;
        //Debug.Log("token :" + token + " >>>" + inputTokenValue.text + ">>>" + txtTokenValue.text + "/" + ObscuredPrefs.GetDouble("coin", 5000));
        //Debug.Log(">>" + inputTokenValue.text.Split(' ')[0] + "/" + inputTokenValue.text.Split(' ')[1]);
        if (inputTokenValue.text.Split(' ').Length == 2)
        {
            txtCoinValue.text = (Convertor(double.Parse(inputTokenValue.text.Split(' ')[0]), inputTokenValue.text.Split(' ')[1]) * rate).ToString("0.##");
        }
        else
        {
            txtCoinValue.text = (Convertor(double.Parse(inputTokenValue.text), "o") * rate).ToString("0.##");
        }

        //Debug.Log(token * rate);
    }
    public void MinValue()
    {
        inputTokenValue.text = "1";
    }

    private double Convertor(double dIn, string sIn)
    {
        double power = 1;
        if (sIn == "o")
        {
            power = 1;
        }
        else if (sIn == "K")
        {
            power = 1000d;
        }
        else if (sIn == "M")
        {
            power = 1000000d;
        }
        else if (sIn == "B")
        {
            power = 1000000000d;
        }
        else if (sIn == "T")
        {
            power = 1000000000000d;
        }
        else if (sIn == "aa")
        {
            power = 1000000000000000d;
        }
        else if (sIn == "ab")
        {
            power = 1000000000000000000d;
        }
        else if (sIn == "ac")
        {
            power = 1000000000000000000d;
        }
        else if (sIn == "ad")
        {
            power = 1000000000000000000000000d;
        }
        else if (sIn == "ae")
        {
            power = 1000000000000000000000000000d;
        }
        else if (sIn == "af")
        {
            power = 1000000000000000000000000000000d;
        }
        else if (sIn == "ag")
        {
            power = 1000000000000000000000000000000000d;
        }
        else if (sIn == "ah")
        {
            power = 1000000000000000000000000000000000000d;
        }
        else if (sIn == "ai")
        {
            power = 1000000000000000000000000000000000000000d;
        }
        else if (sIn == "aj")
        {
            power = 1000000000000000000000000000000000000000000d;
        }
        else if (sIn == "ak")
        {
            power = 1000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "al")
        {
            power = 1000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "am")
        {
            power = 1000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "an")
        {
            power = 1000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "ao")
        {
            power = 1000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "ap")
        {
            power = 1000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "aq")
        {
            power = 1000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "ar")
        {
            power = 1000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "as")
        {
            power = 1000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "at")
        {
            power = 1000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "au")
        {
            power = 1000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "av")
        {
            power = 1000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "aw")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "ax")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "ay")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "az")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "ba")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "bb")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "bc")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "bd")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "be")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "bf")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "bg")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        else if (sIn == "bh")
        {
            power = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;
        }
        Debug.Log("power : " + power + "din : " + dIn);

        return power * dIn;
    }
}
