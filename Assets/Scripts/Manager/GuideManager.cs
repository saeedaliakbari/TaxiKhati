using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
public class GuideManager : MonoBehaviour
{
    public GameObject panelLockGuide, btnBuyCar, buyGuide, mergeGuide, startDriveGuide,
        ClosePanelMergeGuide, btnCloseMerge, returnPanelGuide, openBoxGuide,
        mergeGuide2, imgDriver, imgPanelText, txtStep10, btnStep10;
    public Controller controller;
    [HideInInspector]
    public List<ParkingPlace> parkPlace = new List<ParkingPlace>();
    public bool enableHelp = true;
    void Start()
    {
        if (enableHelp)
        {
            ObscuredPrefs.SetInt("helpStep", 0);
        }
        else
        {
            ObscuredPrefs.SetInt("helpStep", 22);
        }
    }
    public void PlusStep()
    {
        ObscuredPrefs.SetInt("helpStep", ObscuredPrefs.GetInt("helpStep", 0) + 1);
        Debug.Log("Help Step: " + ObscuredPrefs.GetInt("helpStep", 0));
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
            controller.SpawnABox(1, parkPlace[3], 0);
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
}
