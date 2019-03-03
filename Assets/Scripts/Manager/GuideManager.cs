using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
public class GuideManager : MonoBehaviour
{
    public GameObject panelLockGuide, btnBuyCar, buyGuide, mergeGuide, startDriveGuide,
        ClosePanelMergeGuide, btnCloseMerge, returnPanelGuide;
    public Controller controller;
    //[HideInInspector]
    public List<ParkingPlace> parkPlace = new List<ParkingPlace>();
    void Start()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) != 13)
        {
            ObscuredPrefs.SetInt("helpStep", 0);
            panelLockGuide.SetActive(true);
        }
        else
        {
            Destroy(panelLockGuide);
            Destroy(gameObject);
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
            StartCoroutine(IEReturn());
            PlusStep();
        }
    }
    IEnumerator IEReturn()
    {
        yield return new WaitForSeconds(4.1f);
        returnPanelGuide.SetActive(true);
    }
    public void ReturnCar()
    {
        if (ObscuredPrefs.GetInt("helpStep", 0) == 7)
        {
            returnPanelGuide.SetActive(false);
            controller.SpawnABox(1, parkPlace[3], 0);

            PlusStep();
        }
    }
}
