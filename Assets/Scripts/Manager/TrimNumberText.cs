using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrimNumberText : Text
{
    //private static readonly int charA = (int)'a'/*('a')*/;

    //private static readonly Dictionary<int, string> units = new Dictionary<int, string>
    //{
    //    {0, ""},
    //    {1, "K"},
    //    {2, "M"},
    //    {3, "B"},
    //    {4, "T"}
    //};

    //public static string FormatNumber(double value)
    //{
    //    if (value < 1d)
    //    {
    //        return "0";
    //    }

    //    var n = (int)Math.Log(value, 1000);
    //    var m = value / Math.Pow(1000, n);
    //    var unit = "";

    //    if (n < units.Count)
    //    {
    //        unit = units[n];
    //    }
    //    else
    //    {
    //        var unitInt = n - units.Count;
    //        var secondUnit = unitInt % 26;
    //        var firstUnit = unitInt / 26;
    //        unit = char.ConvertFromUtf32(firstUnit + charA).ToString() + char.ConvertFromUtf32(secondUnit + charA).ToString();
    //    }

    //    // Math.Floor(m * 100) / 100) fixes rounding errors
    //    return (Math.Floor(m * 100) / 100).ToString("0.##") + unit;
    //}
    public void UpdateText()
    {
        double number;
        bool success = double.TryParse(GetComponent<Text>().text.Trim(), out number);
        if (number < 1000)
        {
            number = (int)number;
        }
        if (success)
        {
            if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//90
            {
                if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//111
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " bh";
                }
                else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//108
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " bg";
                }
                else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//105
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " bf";
                }
                else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//102
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " be";
                }
                else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//99
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " bd";
                }
                else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//96
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " bc";
                }
                else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//93
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " bb";
                }
                else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//90
                {
                    GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " ba";
                }
            }
            else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//87
            {
                GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " az";
            }
            else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//84
            {
                GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " ay";
            }
            else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//81
            {
                GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " ax";
            }
            else if (number >= 10000000000000000000000000000000000000000000000000000000000000000000000000000000000d)//78
            {
                GetComponent<Text>().text = (number / 10000000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " aw";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000000000000000000000000d)//75
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " av";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000000000000000000000d)//72
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " au";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000000000000000000d)//69
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " at";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000000000000000d)//66
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " as";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000000000000d)//63
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " ar";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000000000d)//60
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " aq";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000000d)//57
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " ap";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000000d)//54
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " ao";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000000d)//51
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000000d).ToString("0.##") + " an";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000000d)//48
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000000d).ToString("0.##") + " am";
            }
            else if (number >= 1000000000000000000000000000000000000000000000000d)//48
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000000d).ToString("0.##") + " al";
            }
            else if (number >= 1000000000000000000000000000000000000000000000d)//45
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000000d).ToString("0.##") + " ak";
            }
            else if (number >= 1000000000000000000000000000000000000000000d)//42
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000000d).ToString("0.##") + " aj";
            }
            else if (number >= 1000000000000000000000000000000000000000d)//39
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000000d).ToString("0.##") + " ai";
            }
            else if (number >= 1000000000000000000000000000000000000d)//36
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000000d).ToString("0.##") + " ah";
            }
            else if (number >= 1000000000000000000000000000000000d)//33
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000000d).ToString("0.##") + " ag";
            }
            else if (number >= 1000000000000000000000000000000d)//30
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000000d).ToString("0.##") + " af";
            }
            else if (number >= 1000000000000000000000000000d)//27
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000000d).ToString("0.##") + " ae";
            }
            else if (number >= 1000000000000000000000000d)//24
            {
                GetComponent<Text>().text = (number / 1000000000000000000000000d).ToString("0.##") + " ad";
            }
            else if (number >= 1000000000000000000000d)//21
            {
                GetComponent<Text>().text = (number / 1000000000000000000000d).ToString("0.##") + " ac";
            }
            else if (number >= 1000000000000000000d)//18
            {
                GetComponent<Text>().text = (number / 1000000000000000000d).ToString("0.##") + " ab";
            }
            else if (number >= 1000000000000000d)//15
            {
                GetComponent<Text>().text = (number / 1000000000000000d).ToString("0.##") + " aa";
            }
            else if (number >= 1000000000000d)//12
            {
                GetComponent<Text>().text = (number / 1000000000000d).ToString("0.##") + " T";
            }
            else if (number >= 1000000000d)
            {
                GetComponent<Text>().text = (number / 1000000000d).ToString("0.##") + " B";
            }
            else if (number >= 1000000d)
            {
                GetComponent<Text>().text = (number / 1000000d).ToString("0.##") + " M";
            }
            else if (number >= 1000d)
            {
                GetComponent<Text>().text = (number / 1000d).ToString("0.##") + " K";
            }
            //else
            //{
            //    GetComponent<Text>().text = (number / 1000d).ToString("0.##") + " ";
            //}

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
