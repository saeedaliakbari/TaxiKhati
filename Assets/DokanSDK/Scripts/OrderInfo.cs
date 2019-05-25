using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

[Serializable]
public class OrderInfo {
    public string status;
    public int price;
    public int coin;
    public string productName;
    [FormerlySerializedAs("operator")]
    public string simOperator;
}
