using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpBonus : MonoBehaviour
{
    public GameObject parkItem, lineItem, coinItem, gemItem, objLevelLeft, objLevelRight;
    public Text txtLevel, txtPark, txtLine, txtCoin, txtGem, txtLevelLeft, txtLevelCenter, txtLevelRight;
    public TrimNumberText txtCoinConvertor;
    public PlayerLevel playerLevel;
    public Controller controller;
    public Image imgSlider;
    public Animator FrameAnimator;
    int newLevel;
    public void ShowLevelUpBonus(int newLevel)
    {
        controller.internetStorageSpace.SaveData(false);
        if (newLevel > 53)
        {
            Debug.Log("newlevel: " + newLevel + ">>" + ObscuredPrefs.GetInt("gem" + newLevel));
            this.newLevel = newLevel;
            txtGem.text = "+" + ObscuredPrefs.GetInt("gem" + newLevel).ToString();
            txtLevel.text = newLevel.ToString();
            gemItem.SetActive(ObscuredPrefs.GetInt("gem" + newLevel) > 0);
            lineItem.SetActive(false);
            coinItem.SetActive(false);
            parkItem.SetActive(false);
            gemItem.transform.localPosition = new Vector3(0, 0);
            FrameAnimator.Play("G");
            SetSliderLevel();
        }
        else {
            this.newLevel = newLevel;
            txtCoinConvertor.text = playerLevel.levelsInfo[newLevel - 2].coin.ToString();
            txtCoin.text = "+" + txtCoinConvertor.text;
            txtGem.text = "+" + playerLevel.levelsInfo[newLevel - 2].gem.ToString();
            txtLine.text = "+" + playerLevel.levelsInfo[newLevel - 2].linePlus.ToString();
            txtPark.text = "+" + playerLevel.levelsInfo[newLevel - 2].parkingPlus.ToString();
            txtLevel.text = newLevel.ToString();

            lineItem.SetActive(playerLevel.levelsInfo[newLevel - 2].linePlus > 0);
            coinItem.SetActive(double.Parse(playerLevel.levelsInfo[newLevel - 2].coin) > 0);
            gemItem.SetActive(playerLevel.levelsInfo[newLevel - 2].gem > 0);
            SetSliderLevel();
            #region Set Position Item
            #region prakItem>0
            if (playerLevel.levelsInfo[newLevel - 2].parkingPlus > 0)
            {
                parkItem.SetActive(true);
                #region coin>0
                if (double.Parse(playerLevel.levelsInfo[newLevel - 2].coin) > 0)
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
                            FrameAnimator.Play("PLG");
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
                            FrameAnimator.Play("PL");
                        }
                        else
                        {
                            //پارکینگ
                            parkItem.transform.localPosition = new Vector3(0, 0);
                            FrameAnimator.Play("P");
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
                if (double.Parse(playerLevel.levelsInfo[newLevel - 2].coin) > 0)
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
                            FrameAnimator.Play("CG");
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
                            FrameAnimator.Play("C");
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
    }
    public void ClaimClick()
    {
        if (newLevel > 53)
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") + ObscuredPrefs.GetInt("gem" + newLevel));
            controller.SetText();
        }
        else {
            ObscuredPrefs.SetDouble("coin", ObscuredPrefs.GetDouble("coin", 5000) + double.Parse(playerLevel.levelsInfo[newLevel - 2].coin));
            ObscuredPrefs.SetDouble("coinTotal", ObscuredPrefs.GetDouble("coinTotal", 0) + double.Parse(playerLevel.levelsInfo[newLevel - 2].coin));
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") + playerLevel.levelsInfo[newLevel - 2].gem);
            controller.SetText();
            int linePlus = playerLevel.levelsInfo[newLevel - 2].linePlus;
            if (linePlus > 0)
            {
                while (linePlus > 0)
                {
                    linePlus--;
                    controller.slotManager.SpawnASlot();
                    controller.slotManager.UpdatePosition();
                    ObscuredPrefs.SetInt("num_of_slot", ObscuredPrefs.GetInt("num_of_slot", 2) + 1);
                }
            }
            int parkingPlus = playerLevel.levelsInfo[newLevel - 2].parkingPlus;
            if (parkingPlus > 0)
            {
                while (parkingPlus > 0)
                {
                    parkingPlus--;
                    controller.parkingManager.SpawnNewPlace();
                    controller.parkingManager.UpdatePlacePosition();
                    ObscuredPrefs.SetInt("num_of_places", ObscuredPrefs.GetInt("num_of_places", 4) + 1);
                }
            }
        }
    }

    private void SetSliderLevel()
    {
        //Debug..Log("New Level: " + newLevel);
        objLevelLeft.SetActive(true);
        if (newLevel <= 3)
        {
            objLevelLeft.SetActive(false);
            txtLevelCenter.text = "سطح 3";
            txtLevelRight.text = "سطح 5";
            imgSlider.fillAmount = (newLevel / 3f) * 0.5f;
        }
        else if (newLevel <= 5)
        {
            objLevelLeft.SetActive(false);
            txtLevelCenter.text = "سطح 3";
            txtLevelRight.text = "سطح 5";
            imgSlider.fillAmount = (((newLevel - 3) / 2f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 8)
        {
            txtLevelLeft.text = "سطح 5";
            txtLevelCenter.text = "سطح 8";
            txtLevelRight.text = "سطح 11";
            imgSlider.fillAmount = ((newLevel - 5) / 3f) * 0.5f;
        }
        else if (newLevel <= 11)
        {
            txtLevelLeft.text = "سطح 5";
            txtLevelCenter.text = "سطح 8";
            txtLevelRight.text = "سطح 11";
            imgSlider.fillAmount = (((newLevel - 8) / 3f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 14)
        {
            txtLevelLeft.text = "سطح 11";
            txtLevelCenter.text = "سطح 14";
            txtLevelRight.text = "سطح 17";
            imgSlider.fillAmount = ((newLevel - 11) / 3f) * 0.5f;
        }
        else if (newLevel <= 17)
        {
            txtLevelLeft.text = "سطح 11";
            txtLevelCenter.text = "سطح 14";
            txtLevelRight.text = "سطح 17";
            imgSlider.fillAmount = (((newLevel - 14) / 3f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 20)
        {
            txtLevelLeft.text = "سطح 17";
            txtLevelCenter.text = "سطح 20";
            txtLevelRight.text = "سطح 23";
            imgSlider.fillAmount = ((newLevel - 17) / 3f) * 0.5f;
        }
        else if (newLevel <= 23)
        {
            txtLevelLeft.text = "سطح 17";
            txtLevelCenter.text = "سطح 20";
            txtLevelRight.text = "سطح 23";
            imgSlider.fillAmount = (((newLevel - 20) / 3f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 26)
        {
            txtLevelLeft.text = "سطح 23";
            txtLevelCenter.text = "سطح 26";
            txtLevelRight.text = "سطح 29";
            imgSlider.fillAmount = ((newLevel - 23) / 3f) * 0.5f;
        }
        else if (newLevel <= 29)
        {
            txtLevelLeft.text = "سطح 23";
            txtLevelCenter.text = "سطح 26";
            txtLevelRight.text = "سطح 29";
            imgSlider.fillAmount = (((newLevel - 26) / 3f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 32)
        {
            txtLevelLeft.text = "سطح 29";
            txtLevelCenter.text = "سطح 32";
            txtLevelRight.text = "سطح 35";
            imgSlider.fillAmount = ((newLevel - 29) / 3f) * 0.5f;
        }
        else if (newLevel <= 35)
        {
            txtLevelLeft.text = "سطح 29";
            txtLevelCenter.text = "سطح 32";
            txtLevelRight.text = "سطح 35";
            imgSlider.fillAmount = (((newLevel - 32) / 3f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 38)
        {
            txtLevelLeft.text = "سطح 35";
            txtLevelCenter.text = "سطح 38";
            txtLevelRight.text = "سطح 41";
            imgSlider.fillAmount = ((newLevel - 35) / 3f) * 0.5f;
        }
        else if (newLevel <= 41)
        {
            txtLevelLeft.text = "سطح 35";
            txtLevelCenter.text = "سطح 38";
            txtLevelRight.text = "سطح 41";
            imgSlider.fillAmount = (((newLevel - 38) / 3f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 44)
        {
            txtLevelLeft.text = "سطح 41";
            txtLevelCenter.text = "سطح 44";
            txtLevelRight.text = "سطح 47";
            imgSlider.fillAmount = ((newLevel - 41) / 3f) * 0.5f;
        }
        else if (newLevel <= 47)
        {
            txtLevelLeft.text = "سطح 41";
            txtLevelCenter.text = "سطح 44";
            txtLevelRight.text = "سطح 47";
            imgSlider.fillAmount = (((newLevel - 44) / 3f) * 0.5f) + 0.5f;
        }
        else if (newLevel <= 50)
        {
            txtLevelLeft.text = "سطح 47";
            txtLevelCenter.text = "سطح 50";
            txtLevelRight.text = "سطح 53";
            imgSlider.fillAmount = ((newLevel - 47) / 3f) * 0.5f;
        }
        else if (newLevel <= 53)
        {
            txtLevelLeft.text = "سطح 47";
            txtLevelCenter.text = "سطح 50";
            txtLevelRight.text = "سطح 53";
            imgSlider.fillAmount = (((newLevel - 50) / 3f) * 0.5f) + 0.5f;
        }
        else
        {
            int levelGem = 53 + (((newLevel - 53) / 3) * 3);
            txtLevelLeft.text = "سطح " + (levelGem - 3);
            txtLevelCenter.text = "سطح " + (levelGem);
            txtLevelRight.text = "سطح " + (levelGem + 3);
            imgSlider.fillAmount = (newLevel - (levelGem - 3)) / 6f;
        }


    }
}

