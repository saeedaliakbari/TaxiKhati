using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceCoin : MonoBehaviour
{
    public Text txtCoin;
    public void UpdateCoinBalance()
    {
        //استفاده نشده
        //float coin = float.Parse(PlayerPrefs.GetString("Coin"));
        string coin = PlayerPrefs.GetString("Coin", "5000");
        if (coin.Length > 27)
        {
            txtCoin.text = (float.Parse(coin) / 1000000000000000000000000f).ToString("0") + "AD";
        }
        else if (coin.Length > 24)
        {
            txtCoin.text = (float.Parse(coin) / 1000000000000000000000f).ToString("0") + "AC";
        }
        else if (coin.Length > 21)
        {
            txtCoin.text = (float.Parse(coin) / 1000000000000000000f).ToString("0") + "AB";
        }
        else if (coin.Length > 18)
        {
            txtCoin.text = (float.Parse(coin) / 1000000000000000f).ToString("0") + "AA";
        }
        else if (coin.Length > 15)
        {
            txtCoin.text = (float.Parse(coin) / 1000000000000f).ToString("0") + "T";
        }
        else if (coin.Length > 12)
        {
            txtCoin.text = (float.Parse(coin) / 1000000000f).ToString("0") + "B";
        }
        else if (coin.Length > 9)
        {
            txtCoin.text = (float.Parse(coin) / 1000000f).ToString("0") + "M";
        }
        else if (coin.Length > 6)
        {
            txtCoin.text = (float.Parse(coin) / 1000f).ToString("0") + "K";
        }
        else
        {
            txtCoin.text = coin.ToString();
        }
    }
}
