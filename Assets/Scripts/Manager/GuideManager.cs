using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class GuideManager : MonoBehaviour
{
    public GameObject panelLockGuide, btnBuyCar, buyGuide, mergeGuide, startDriveGuide,
        ClosePanelMergeGuide, btnCloseMerge, returnPanelGuide, openBoxGuide,
        mergeGuide2, imgDriver, imgPanelText, txtStep10, btnStep10,
        btnStep, btnStep2, txtStep1, txtStep2,
        txtStep11, btnStep11,
        openPanelExChange, token,
        btnShop, openShopGuide,
        panelShopCar, coin, playerLevel, btnOther, otherItemGuide,
        btnBuyShopItem, buyItemGuide, btnStep19, txtStep19;
    public ShopPanel shopPanel;
    public ShopItemEarn shopItemEarn;
    public ShopItemOff shopItemOff;
    public Controller controller;
    [HideInInspector]
    public List<ParkingPlace> parkPlace = new List<ParkingPlace>();
    public void PlusStep()
    {
        ObscuredPrefs.SetInt("helpStep", ObscuredPrefs.GetInt("helpStep", 0) + 1);
        GameAnalytics.NewDesignEvent("Help Step", ObscuredPrefs.GetInt("helpStep", 0));
    }
    public void InActiveBuyCar()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 3)
        {
            btnBuyCar.SetActive(false);
            buyGuide.SetActive(false);
            mergeGuide.SetActive(true);
            controller.parkingManager.EnableCarInPark();
        }
        else
        {
            controller.parkingManager.DisableCarInPark();
        }
    }
    public void MergeStep()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 4)
        {
            mergeGuide.SetActive(false);
            PlusStep();
            StartCoroutine(IEMergePanel());
        }
    }
    IEnumerator IEMergePanel()
    {
        yield return new WaitForSeconds(2f);
        ClosePanelMergeGuide.SetActive(true);
        btnCloseMerge.SetActive(true);
    }
    public void StartDrive()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 6)
        {
            startDriveGuide.SetActive(false);
            controller.colliderCarHelp.enabled = false;
            StartCoroutine(IEReturn());
            PlusStep();
        }
        else if (ObscuredPrefs.GetInt("helpStep", 0) == 11)
        {
            startDriveGuide.SetActive(false);
            imgDriver.SetActive(true);
            imgPanelText.SetActive(true);
            txtStep10.SetActive(true);
            btnStep10.SetActive(true);
            PlusStep();
        }
    }
    IEnumerator IEReturn()
    {
        yield return new WaitForSeconds(4.1f);
        controller.colliderCarHelp.enabled = true;
        returnPanelGuide.SetActive(true);
    }
    public void ReturnCar()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 7)
        {
            returnPanelGuide.SetActive(false);
            controller.SpawnABox(1, parkPlace[3], 0, 3f);
            openBoxGuide.SetActive(true);
            PlusStep();
        }
    }
    public void OpenGiftBox()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 8)
        {
            openBoxGuide.SetActive(false);
            mergeGuide2.SetActive(true);
            PlusStep();
        }
    }
    public void MergeStep2()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 9)
        {
            mergeGuide2.SetActive(false);
            panelLockGuide.SetActive(false);
            PlusStep();
        }
    }
    public void Merge2Done()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 10)
        {
            panelLockGuide.SetActive(true);
            mergeGuide2.SetActive(false);
            startDriveGuide.SetActive(true);
            PlusStep();
        }
    }

    public void Step(int level)
    {
        switch (level)
        {
            case 0:
                panelLockGuide.SetActive(true);
                break;
            case 1:
                btnStep.SetActive(false);
                btnStep2.SetActive(true);
                txtStep1.SetActive(false);
                txtStep2.SetActive(true);
                break;
            case 2:
                imgPanelText.SetActive(false);
                imgDriver.SetActive(false);
                txtStep2.SetActive(false);
                txtStep10.SetActive(true);
                btnStep2.SetActive(false);
                buyGuide.SetActive(true);
                btnBuyCar.SetActive(true);
                break;
            case 3:
                controller.parkingManager.DisableCarInPark();
                break;
            case 4:
                btnBuyCar.SetActive(false);
                buyGuide.SetActive(false);
                mergeGuide.SetActive(true);
                controller.parkingManager.EnableCarInPark();
                break;
            case 5:
                mergeGuide.SetActive(false);
                ObscuredPrefs.SetInt("helpStep", 6);
                startDriveGuide.SetActive(true);
                break;
            case 6:
                startDriveGuide.SetActive(false);
                returnPanelGuide.SetActive(true);
                ObscuredPrefs.SetInt("helpStep", 7);
                break;
            case 7:
                startDriveGuide.SetActive(false);
                returnPanelGuide.SetActive(true);
                break;
            case 8:
                returnPanelGuide.SetActive(false);
                ObscuredPrefs.SetInt("helpStep", 9);
                mergeGuide2.SetActive(true);
                break;
            case 9:
                mergeGuide2.SetActive(true);
                break;
            case 10:
                ObscuredPrefs.SetInt("helpStep", 11);
                mergeGuide2.SetActive(false);
                startDriveGuide.SetActive(true);
                break;
            case 11:
                mergeGuide2.SetActive(false);
                startDriveGuide.SetActive(true);
                break;
            case 12:
                ObscuredPrefs.SetInt("helpStep", 12);
                startDriveGuide.SetActive(false);
                imgDriver.SetActive(true);
                imgPanelText.SetActive(true);
                txtStep10.SetActive(true);
                btnStep10.SetActive(true);
                break;
            case 13:
                ObscuredPrefs.SetInt("helpStep", 13);
                txtStep10.SetActive(false);
                btnStep10.SetActive(false);
                txtStep11.SetActive(true);
                btnStep11.SetActive(true);
                break;
            case 14:
                ObscuredPrefs.SetInt("helpStep", 14);
                imgPanelText.SetActive(false);
                imgDriver.SetActive(false);
                btnStep11.SetActive(false);
                txtStep11.SetActive(false);
                gameObject.SetActive(true);
                openPanelExChange.SetActive(true);
                token.SetActive(true);
                break;
            case 15:
                ObscuredPrefs.SetInt("helpStep", 14);
                imgPanelText.SetActive(false);
                imgDriver.SetActive(false);
                btnStep11.SetActive(false);
                txtStep11.SetActive(false);
                gameObject.SetActive(true);
                openPanelExChange.SetActive(true);
                token.SetActive(true);
                break;
            case 16:
                openPanelExChange.SetActive(false);
                token.SetActive(false);
                ObscuredPrefs.SetInt("helpStep", 16);
                btnShop.SetActive(true);
                openShopGuide.SetActive(true);
                break;
            case 17:
                ObscuredPrefs.SetInt("helpStep", 17);
                panelShopCar.SetActive(true);
                shopPanel.UpdateCarItems();
                coin.SetActive(true);
                playerLevel.SetActive(false);
                controller.parkingManager.DisableCarInPark();
                shopPanel.OpenPanel();
                btnShop.SetActive(false);
                openShopGuide.SetActive(false);
                btnOther.SetActive(true);
                otherItemGuide.SetActive(true);
                break;
            case 18:
                ObscuredPrefs.SetInt("helpStep", 17);
                panelShopCar.SetActive(true);
                shopPanel.UpdateCarItems();
                coin.SetActive(true);
                playerLevel.SetActive(false);
                controller.parkingManager.DisableCarInPark();
                shopPanel.OpenPanel();
                btnShop.SetActive(false);
                openShopGuide.SetActive(false);
                btnOther.SetActive(true);
                otherItemGuide.SetActive(true);
                break;
            case 19:
                ObscuredPrefs.SetInt("helpStep", 20);
                panelShopCar.SetActive(false);
                coin.SetActive(false);
                playerLevel.SetActive(true);
                btnStep19.SetActive(true);
                imgDriver.SetActive(true);
                txtStep19.SetActive(true);
                btnOther.SetActive(false);
                otherItemGuide.SetActive(false);
                break;
            case 20:
                ObscuredPrefs.SetInt("helpStep", 21);
                panelShopCar.SetActive(false);
                coin.SetActive(false);
                playerLevel.SetActive(true);
                imgPanelText.SetActive(true);
                btnStep19.SetActive(true);
                imgDriver.SetActive(true);
                txtStep19.SetActive(true);
                btnOther.SetActive(false);
                otherItemGuide.SetActive(false);
                controller.parkingManager.EnableCarInPark();
                break;

        }
    }

}
