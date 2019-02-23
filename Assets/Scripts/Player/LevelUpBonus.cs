using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpBonus : MonoBehaviour
{
    public GameObject parkItem, lineItem, coinItem, gemItem;
    public Text txtLevel, txtPark, txtLine, txtCoin, txtGem;
    public PlayerLevel playerLevel;
    //public Controller controller;
    int newLevel;
    public void ShowLevelUpBonus(int newLevel)
    {
        this.newLevel = newLevel;
        txtCoin.text = "+" + playerLevel.levelsInfo[newLevel - 2].coin.ToString();
        txtGem.text = "+" + playerLevel.levelsInfo[newLevel - 2].gem.ToString();
        txtLine.text = "+" + playerLevel.levelsInfo[newLevel - 2].linePlus.ToString();
        txtPark.text = "+" + playerLevel.levelsInfo[newLevel - 2].parkingPlus.ToString();
        txtLevel.text = newLevel.ToString();

        lineItem.SetActive(playerLevel.levelsInfo[newLevel - 2].linePlus > 0);
        coinItem.SetActive(playerLevel.levelsInfo[newLevel - 2].coin > 0);
        gemItem.SetActive(playerLevel.levelsInfo[newLevel - 2].gem > 0);

        #region Set Position Item
        #region prakItem>0
        if (playerLevel.levelsInfo[newLevel - 2].parkingPlus > 0)
        {
            parkItem.SetActive(true);
            #region coin>0
            if (playerLevel.levelsInfo[newLevel - 2].coin > 0)
            {
                if (playerLevel.levelsInfo[newLevel - 2].gem > 0)
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //پارکینگ و سکه و جم و لاین
                        parkItem.transform.localPosition = new Vector3(-270, 0);
                        coinItem.transform.localPosition = new Vector3(-90, 0);
                        gemItem.transform.localPosition = new Vector3(90, 0);
                        lineItem.transform.localPosition = new Vector3(270, 0);
                    }
                    else
                    {
                        //پارکینگ و سکه وجم
                        parkItem.transform.localPosition = new Vector3(-230, 0);
                        coinItem.transform.localPosition = new Vector3(0, 0);
                        gemItem.transform.localPosition = new Vector3(230, 0);
                    }
                }
                else
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //پارکینگ و سکه و لاین
                        parkItem.transform.localPosition = new Vector3(-230, 0);
                        coinItem.transform.localPosition = new Vector3(0, 0);
                        lineItem.transform.localPosition = new Vector3(230, 0);
                    }
                    else
                    {
                        //پارکینگ و سکه
                        parkItem.transform.localPosition = new Vector3(-120, 0);
                        coinItem.transform.localPosition = new Vector3(120, 0);
                    }
                }
            }
            #endregion
            #region coin=0
            else
            {
                if (playerLevel.levelsInfo[newLevel - 2].gem > 0)
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //پارکینگ و جم و لاین
                        parkItem.transform.localPosition = new Vector3(-230, 0);
                        gemItem.transform.localPosition = new Vector3(0, 0);
                        lineItem.transform.localPosition = new Vector3(230, 0);
                    }
                    else
                    {
                        //پارکینگ و جم
                        parkItem.transform.localPosition = new Vector3(-120, 0);
                        gemItem.transform.localPosition = new Vector3(120, 0);
                    }
                }
                else
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //پارکینگ و لاین
                        parkItem.transform.localPosition = new Vector3(-120, 0);
                        lineItem.transform.localPosition = new Vector3(120, 0);
                    }
                    else
                    {
                        //پارکینگ
                        parkItem.transform.localPosition = new Vector3(0, 0);
                    }
                }
            }
            #endregion
        }
        #endregion
        #region parkItem=0
        else
        {
            parkItem.SetActive(false);
            #region coin>0
            if (playerLevel.levelsInfo[newLevel - 2].coin > 0)
            {
                if (playerLevel.levelsInfo[newLevel - 2].gem > 0)
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //سکه وجم و لابن
                        coinItem.transform.localPosition = new Vector3(-230, 0);
                        gemItem.transform.localPosition = new Vector3(0, 0);
                        lineItem.transform.localPosition = new Vector3(230, 0);
                    }
                    else
                    {
                        //سکه وجم
                        coinItem.transform.localPosition = new Vector3(-120, 0);
                        gemItem.transform.localPosition = new Vector3(120, 0);
                    }
                }
                else
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //سکه و لاین
                        coinItem.transform.localPosition = new Vector3(-120, 0);
                        lineItem.transform.localPosition = new Vector3(120, 0);
                    }
                    else
                    {
                        //سکه
                        coinItem.transform.localPosition = new Vector3(0, 0);
                    }
                }
            }
            #endregion
            #region coin=0
            else
            {
                if (playerLevel.levelsInfo[newLevel - 2].gem > 0)
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //جم و لاین
                        gemItem.transform.localPosition = new Vector3(-120, 0);
                        lineItem.transform.localPosition = new Vector3(120, 0);
                    }
                    else
                    {
                        //جم
                        gemItem.transform.localPosition = new Vector3(0, 0);
                    }
                }
                else
                {
                    if (playerLevel.levelsInfo[newLevel - 2].linePlus > 0)
                    {
                        //لاین
                        lineItem.transform.localPosition = new Vector3(0, 0);
                    }
                    else
                    {
                        //--
                    }
                }
            }
            #endregion
        }
        #endregion
        #endregion
    }
    public void ClaimClick()
    {
        PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin", 5000) + playerLevel.levelsInfo[newLevel - 2].coin);
        PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem") + playerLevel.levelsInfo[newLevel - 2].gem);
        Controller.instance.SetText();
        int linePlus = playerLevel.levelsInfo[newLevel - 2].linePlus;
        if (linePlus > 0)
        {
            while (linePlus > 0)
            {
                linePlus--;
                Controller.instance.slotManager.SpawnASlot();
                Controller.instance.slotManager.UpdatePosition();
                PlayerPrefs.SetInt("num_of_slot", PlayerPrefs.GetInt("num_of_slot", 2) + 1);
            }
        }
        int parkingPlus = playerLevel.levelsInfo[newLevel - 2].parkingPlus;
        if (parkingPlus > 0)
        {
            while (parkingPlus > 0)
            {
                parkingPlus--;
                Controller.instance.parkingManager.SpawnNewPlace();
                Controller.instance.parkingManager.UpdatePlacePosition();
                PlayerPrefs.SetInt("num_of_places", PlayerPrefs.GetInt("num_of_places", 4) + 1);
            }
        }
    }
}

