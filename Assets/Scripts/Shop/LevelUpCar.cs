using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUpCar : MonoBehaviour
{
    public Controller controller;
    public GameObject[] objParticle;
    public Text txtGem;
    [HideInInspector]
    public int price = 0;
    //[HideInInspector]
    //public ParkingPlace parkPlace = null;
    public void btnClose()
    {
        StartCoroutine(controller.SpawnBoxLevelUpCar(controller.videoAds.indexOld, controller.parkingManager.GetEmptyPlace(), 2, 3f));
        controller.videoAds.panelTaxiUpVideo.SetActive(false);
    }
    public void buyWithGem()
    {
        if (ObscuredPrefs.GetDouble("gem", 0) >= price)
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 0) - price);
            controller.SetText();
            StartCoroutine(controller.SpawnBoxLevelUpCar(controller.videoAds.indexNew, controller.parkingManager.GetEmptyPlace(), 2, 3f));
            controller.videoAds.panelTaxiUpVideo.SetActive(false);
            controller.parkingManager.EnableCarInPark();
        }
        else
        {
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "مقدار کافی الماس ندارید";
            DiableParitcle();
        }
    }
    private void DiableParitcle()
    {
        for (int i = 0; i < objParticle.Length; i++)
        {
            objParticle[i].SetActive(false);
        }
    }
    public void EnableParticle()
    {
        for (int i = 0; i < objParticle.Length; i++)
        {
            objParticle[i].SetActive(true);
        }
    }
}
