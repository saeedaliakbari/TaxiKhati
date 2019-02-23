using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrimNumberText : Text
{
    public void UpdateText()
    {
        float number;
        bool success = float.TryParse(GetComponent<Text>().text.Trim(), out number);
        if (success)
        {
            if (number >= 1000000000000000000000000f)
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000f).ToString("0.0") + " AD";
            }
            else if (number >= 1000000000000000000000f)
            {
                GetComponent<Text>().text = (number / 1000000000000000000000f).ToString("0.0") + " AC";
            }
            else if (number >= 1000000000000000000f)
            {
                GetComponent<Text>().text = (number / 1000000000000000000f).ToString("0.0") + " AB";
            }
            else if (number >= 1000000000000000)
            {
                GetComponent<Text>().text = (number / 1000000000000000).ToString("0.0") + " AA";
            }
            else if (number >= 1000000000000)
            {
                GetComponent<Text>().text = (number / 1000000000000).ToString("0.0") + " T";
            }
            else if (number >= 1000000000)
            {
                GetComponent<Text>().text = (number / 1000000000).ToString("0.0") + " B";
            }
            else if (number >= 1000000)
            {
                GetComponent<Text>().text = (number / 1000000).ToString("0.0") + " M";
            }
            else if (number >= 1000)
            {
                GetComponent<Text>().text = (number / 1000).ToString("0.0") + " K";
            }
        }
    }

    public override string text
    {
        get
        {
            return base.text;
        }
        set
        {
            base.text = value;
            UpdateText();
        }
    }
}
