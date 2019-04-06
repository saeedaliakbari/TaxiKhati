using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineGiftCar : MonoBehaviour
{
    public Controller controller;
    private int giftNumber;
    // Use this for initialization
    public void offGiftCar(int numEmptyPark)
    {
        int time = 0;
        giftNumber = 0;
        if (Manager.GetActionTime("offline_earning") == 0)
        {
            time = 0;
        }
        else
        {
            time = (int)(Manager.GetCurrentTime() - Manager.GetActionTime("offline_earning"));
        }
        giftNumber = time / 20 - 1;
        Debug.Log("gift number: " + giftNumber);
        if (giftNumber > numEmptyPark)
        {
            giftNumber = numEmptyPark;
        }
        //while (controller.panelSplash.activeSelf) ;
        for (int i = 0; i < giftNumber; i++)
        {
            Debug.Log("insert " + i + " gift");
            ParkingPlace parkPlace = controller.parkingManager.GetEmptyPlace();
            int taxiLvl = ObscuredPrefs.GetInt("unlocked_car", 1);
            int index = taxiLvl - UnityEngine.Random.Range(controller.taxiDefferenceLvl[taxiLvl - 1].min, controller.taxiDefferenceLvl[taxiLvl - 1].max);
            index = index > 0 ? index : 1;
            Debug.Log("index Car : " + index);
            controller.SpawnABox(index - 1, parkPlace, 0, 8f);
        }
        giftNumber = 0;
    }
}

