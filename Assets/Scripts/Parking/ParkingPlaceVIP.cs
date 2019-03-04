using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingPlaceVIP : MonoBehaviour
{
    [HideInInspector]
    public Controller controller;
    private void OnMouseUp()//کشیدن را تمام کرد
    {
        controller.panelMessage.SetActive(true);
        controller.btnGoToVipPanelMessage.SetActive(true);
        controller.txtPanelMessage.text = "براي فعالسازي پارکینگ لطفا اشتراکـ را تهیه فرمایید";
    }
}
