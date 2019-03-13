using CodeStage.AntiCheat.ObscuredTypes;
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
    public Animator animPlayerLevel;
    public Move myMove;

    public GameObject moveObj, NewIteamPanel;
    private Coroutine lastRoutine = null;
    // Use this for initialization
    void Start()
    {
        SetEleman();
        CheckLevel(ObscuredPrefs.GetInt("Level", 1), false);
    }
    public void UpdateProgress(int xp)
    {
        if (lastRoutine != null)
        {
            StopCoroutine(lastRoutine);
        }
        lastRoutine = StartCoroutine(IEUpdateProgress(xp));
        //if (ObscuredPrefs.GetInt("Xp", 0) > levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp)
        //{
        //    controller.ShowLevelBonus(ObscuredPrefs.GetInt("Level", 1) + 1);
        //    ObscuredPrefs.SetInt("Xp", ObscuredPrefs.GetInt("Xp", 0) - levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp);
        //    ObscuredPrefs.SetInt("Level", ObscuredPrefs.GetInt("Level", 1) + 1);
        //    ObscuredPrefs.SetInt("mainAchiv11", ObscuredPrefs.GetInt("Level", 1));
        //    ObscuredPrefs.SetInt("mainAchiv12", ObscuredPrefs.GetInt("Level", 1));
        //    ObscuredPrefs.SetInt("mainAchiv13", ObscuredPrefs.GetInt("Level", 1));
        //    controller.achivmentManager.OpenPanel();
        //    CheckLevel(ObscuredPrefs.GetInt("Level", 1), true);
        //}
        //SetEleman();
    }
    IEnumerator IEUpdateProgress(int xp)
    {
        yield return new WaitForSeconds(1.1f);
        animPlayerLevel.Play("XPGain");
        int nowXp = ObscuredPrefs.GetInt("Xp", 0) - xp;
        int xptrailer = 0;
        while (xptrailer <= xp)
        {
            if ((nowXp + xptrailer) > levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp)
            {
                controller.ShowLevelBonus(ObscuredPrefs.GetInt("Level", 1) + 1);
                ObscuredPrefs.SetInt("Xp", ObscuredPrefs.GetInt("Xp", 0) - levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp);
                nowXp = 0;
                ObscuredPrefs.SetInt("Level", ObscuredPrefs.GetInt("Level", 1) + 1);
                ObscuredPrefs.SetInt("mainAchiv11", ObscuredPrefs.GetInt("Level", 1));
                ObscuredPrefs.SetInt("mainAchiv12", ObscuredPrefs.GetInt("Level", 1));
                ObscuredPrefs.SetInt("mainAchiv13", ObscuredPrefs.GetInt("Level", 1));
                controller.achivmentManager.CheckAchivments();
                CheckLevel(ObscuredPrefs.GetInt("Level", 1), true);
            }
            txtLevel.text = ObscuredPrefs.GetInt("Level", 1).ToString();
            float slider = (nowXp + xptrailer) / float.Parse(levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp.ToString());
            imgProgress.fillAmount = slider;
            Debug.Log("now xp : " + (nowXp + xptrailer));
            yield return new WaitForSeconds(0.05f);
            xptrailer++;
        }
        SetEleman();
    }
    private void SetEleman()
    {
        txtLevel.text = ObscuredPrefs.GetInt("Level", 1).ToString();
        float slider = float.Parse(ObscuredPrefs.GetInt("Xp", 0).ToString()) / float.Parse(levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp.ToString());
        //Debug.Log("XP: " + ObscuredPrefs.GetInt("Xp", 0) + "MaxXP:" + levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp + ">>" + slider);
        imgProgress.fillAmount = slider;
    }
    private void CheckLevel(int level, bool first)
    {
        if (!first)
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
        else
        {

            if (level == 7)
            {
                myMove.num = 0;
                //myMove.target = btnRank.transform;
                myMove.TargetObj = btnRank;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                //btnRank.SetActive(true);
            }
            if (level == 8)
            {
                myMove.num = 1;
                //myMove.target = btnQuest.transform;
                myMove.TargetObj = btnQuest;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                //btnQuest.SetActive(true);
            }
            if (level == 9)
            {
                myMove.num = 2;
                //myMove.target = btnWheel.transform;
                myMove.TargetObj = btnWheel;
                btnWheel.SetActive(true);
                NewIteamPanel.SetActive(true);
                //moveObj.SetActive(true);
            }
            if (level == 10)
            {
                myMove.num = 3;
                //myMove.target = btnTimeBoost.transform;
                myMove.TargetObj = btnTimeBoost;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                //btnTimeBoost.SetActive(true);
            }
            if (level == 11)
            {
                myMove.num = 4;
                //myMove.target = btnBoosters.transform;
                myMove.TargetObj = btnBoosters;
                //btnBoosters.SetActive(true);
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
            }

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
    public string coin;
}
