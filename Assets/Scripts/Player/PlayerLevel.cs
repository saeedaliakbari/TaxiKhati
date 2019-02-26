using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerLevel : MonoBehaviour
{
    public Text txtLevel;
    public Image imgProgress;
    public Levels[] levelsInfo;
    public GameObject btnRank, btnQuest, btnWheel, btnTimeBoost, btnBoosters;
    public Controller controller;
    // Use this for initialization
    void Start()
    {
        SetEleman();
        CheckLevel(PlayerPrefs.GetInt("Level", 1));
    }
    public void UpdateProgress()
    {
        if (PlayerPrefs.GetInt("Xp", 0) > levelsInfo[PlayerPrefs.GetInt("Level", 1) - 1].maxXp)
        {
            controller.ShowLevelBonus(PlayerPrefs.GetInt("Level", 1) + 1);
            PlayerPrefs.SetInt("Xp", PlayerPrefs.GetInt("Xp", 0) - levelsInfo[PlayerPrefs.GetInt("Level", 1) - 1].maxXp);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            PlayerPrefs.SetInt("mainAchiv11", PlayerPrefs.GetInt("Level", 1));
            PlayerPrefs.SetInt("mainAchiv12", PlayerPrefs.GetInt("Level", 1));
            PlayerPrefs.SetInt("mainAchiv13", PlayerPrefs.GetInt("Level", 1));
            CheckLevel(PlayerPrefs.GetInt("Level", 1));
        }
        SetEleman();
    }
    private void SetEleman()
    {

        txtLevel.text = PlayerPrefs.GetInt("Level", 1).ToString();
        float slider = float.Parse(PlayerPrefs.GetInt("Xp", 0).ToString()) / float.Parse(levelsInfo[PlayerPrefs.GetInt("Level", 1) - 1].maxXp.ToString());
        //Debug.Log("XP: " + PlayerPrefs.GetInt("Xp", 0) + "MaxXP:" + levelsInfo[PlayerPrefs.GetInt("Level", 1) - 1].maxXp + ">>" + slider);
        imgProgress.fillAmount = slider;
    }
    private void CheckLevel(int level)
    {
        if (level > 6)
        {
            btnRank.SetActive(true);
        }
        if (level > 7)
        {
            btnQuest.SetActive(true);
        }
        if (level > 8)
        {
            btnWheel.SetActive(true);
        }
        if (level > 9)
        {
            btnTimeBoost.SetActive(true);
        }
        if (level > 10)
        {
            btnBoosters.SetActive(true);
        }
    }
}
[System.Serializable]
public class Levels
{
    public int level;
    public int maxXp;
    public int parkingPlus;
    public int linePlus;
    public int gem;
    public float coin;
}
