using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExchangeToken : MonoBehaviour
{
    public Controller controller;
    public InputField inputCoinValue;
    public TrimNumberText txtTokenValue;
    private float rate, tokenValue;

    // Use this for initialization
    
    public void SetTxtTokenValue()
    {
        rate = (1 - PlayerPrefs.GetFloat("exchangeDeclineTycoon", 0)) * 10000;
        try
        {
            txtTokenValue.text = Mathf.Floor(float.Parse(inputCoinValue.text) / rate).ToString();
        }
        catch (System.Exception)
        {
            txtTokenValue.text = "0";
        }
    }
    public void BtnExchangeToken()
    {
        tokenValue = Mathf.Floor(float.Parse(inputCoinValue.text) / rate);
        PlayerPrefs.SetFloat("token", PlayerPrefs.GetFloat("token", 0) + tokenValue);
        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 0) - (tokenValue * rate));
        controller.SetText();
    }
}
