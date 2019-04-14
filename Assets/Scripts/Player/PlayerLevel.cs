using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerLevel : MonoBehaviour
{
    public Text txtLevel;
    public Image imgProgress;
    public List<Levels> levelsInfo;
    public GameObject btnRank, btnQuest, btnWheel, btnTimeBoost, btnBoosters;
    public GameObject rankParticle, questParticle, wheelParticle, timeBoostParticle, boostersParticle;
    public Controller controller;
    public Animator animPlayerLevel;
    public Move myMove;

    public GameObject moveObj, NewIteamPanel;
    private Coroutine lastRoutine = null;
    // Use this for initialization
    void Start()
    {
        SetEleman(ObscuredPrefs.GetInt("Xp", 0), 0);
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
            //Debug.Log("xptrailer" + xptrailer + " xp" + xp);
            if (ObscuredPrefs.GetInt("Level", 1) < 53)
            {
                if ((nowXp + xptrailer) > levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp)
                {
                    ObscuredPrefs.SetInt("Xp", ObscuredPrefs.GetInt("Xp", 0) - levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp);
                    nowXp = 0;
                    ObscuredPrefs.SetInt("Level", ObscuredPrefs.GetInt("Level", 1) + 1);
                    controller.ShowLevelBonus(ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.SetInt("mainAchiv11", ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.SetInt("mainAchiv12", ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.SetInt("mainAchiv13", ObscuredPrefs.GetInt("Level", 1));
                    controller.achivmentManager.CheckAchivments();
                    CheckLevel(ObscuredPrefs.GetInt("Level", 1), true);
                    float slider = (nowXp + xptrailer) / float.Parse(levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp.ToString());
                    imgProgress.fillAmount = slider;
                }

            }
            else
            {
                int maxXpOldLevel = ObscuredPrefs.GetInt("maxXp" + ObscuredPrefs.GetInt("Level", 1), 118000);
                //Debug.Log("maxXpOldLevel" + ObscuredPrefs.GetInt("Level", 1) + ">" + maxXpOldLevel);
                if ((nowXp + xptrailer) > maxXpOldLevel)
                {
                    ObscuredPrefs.SetInt("Xp", ObscuredPrefs.GetInt("Xp", 0) - maxXpOldLevel);
                    ObscuredPrefs.DeleteKey("maxXp" + ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.DeleteKey("gem" + ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.SetInt("Level", ObscuredPrefs.GetInt("Level", 1) + 1);
                    Debug.Log("new Level :" + ObscuredPrefs.GetInt("Level", 1));
                    controller.ShowLevelBonus(ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.SetInt("maxXp" + ObscuredPrefs.GetInt("Level", 1), maxXpOldLevel + 3000);
                    Debug.Log("maxXp newLevel" + ObscuredPrefs.GetInt("Level", 1) + ">" + ObscuredPrefs.GetInt("maxXp" + ObscuredPrefs.GetInt("Level", 1), 118000));
                    int levelNew = ObscuredPrefs.GetInt("Level", 1);
                    if ((levelNew - 50) % 3 == 0)
                    {
                        ObscuredPrefs.SetInt("gem" + levelNew, 18 + ((levelNew - 50) / 9) * 5);
                    }
                    else
                    {
                        ObscuredPrefs.SetInt("gem" + levelNew, 0);
                    }
                    ObscuredPrefs.SetInt("mainAchiv11", ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.SetInt("mainAchiv12", ObscuredPrefs.GetInt("Level", 1));
                    ObscuredPrefs.SetInt("mainAchiv13", ObscuredPrefs.GetInt("Level", 1));
                    controller.achivmentManager.CheckAchivments();
                    CheckLevel(ObscuredPrefs.GetInt("Level", 1), true);
                }
            }
            SetEleman(nowXp, xptrailer);
            txtLevel.text = ObscuredPrefs.GetInt("Level", 1).ToString();
            yield return new WaitForSeconds(0.05f);
            if (ObscuredPrefs.GetInt("Level", 1) < 25)
            {
                xptrailer++;
            }
            else
            {
                xptrailer += 10;
            }
        }
        SetEleman(ObscuredPrefs.GetInt("Xp", 0), 0);
    }
    private void SetEleman(int nowXp, int xpTrailer)
    {
        txtLevel.text = ObscuredPrefs.GetInt("Level", 1).ToString();
        if (ObscuredPrefs.GetInt("Level", 1) < 53)
        {
            float slider = (nowXp + xpTrailer) / float.Parse(levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp.ToString());
            ////Debug.Log("XP: " + ObscuredPrefs.GetInt("Xp", 0) + "MaxXP:" + levelsInfo[ObscuredPrefs.GetInt("Level", 1) - 1].maxXp + ">>" + slider);
            imgProgress.fillAmount = slider;
        }
        else
        {
            //Debug.Log("" + (nowXp + xpTrailer) +"/"+ ObscuredPrefs.GetInt("maxXp" + ObscuredPrefs.GetInt("Level", 1), 118000).ToString());
            float slider = (nowXp + xpTrailer) / float.Parse(ObscuredPrefs.GetInt("maxXp" + ObscuredPrefs.GetInt("Level", 1), 118000).ToString());
            //Debug.Log("slider : " + slider);
            imgProgress.fillAmount = slider;
        }
    }
    private void CheckLevel(int level, bool first)
    {
        if (!first)
        {
            if (level > 6)
            {
                btnRank.SetActive(true);
                if (ObscuredPrefs.GetBool("UnlockedRank", true))
                {
                    rankParticle.SetActive(true);
                }
            }
            if (level > 7)
            {
                btnQuest.SetActive(true);
                if (ObscuredPrefs.GetBool("UnlockedQuest", true))
                {
                    questParticle.SetActive(true);
                }
            }
            if (level > 8)
            {
                btnWheel.SetActive(true);
                if (ObscuredPrefs.GetBool("UnlockedWheel", true))
                {
                    wheelParticle.SetActive(true);
                }
            }

            if (level > 9)
            {
                btnTimeBoost.SetActive(true);
                if (ObscuredPrefs.GetBool("UnlockedTimeBoost", true))
                {
                    timeBoostParticle.SetActive(true);
                }
            }

            if (level > 10)
            {
                btnBoosters.SetActive(true);
                if (ObscuredPrefs.GetBool("UnlockedBoosters", true))
                {
                    boostersParticle.SetActive(true);
                }
            }
        }
        else
        {

            if (level == 7)
            {
                myMove.num = 0;
                myMove.TargetObj = btnRank;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                rankParticle.SetActive(true);
            }
            if (level == 8)
            {
                myMove.num = 1;
                myMove.TargetObj = btnQuest;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                questParticle.SetActive(true);
            }
            if (level == 9)
            {
                myMove.num = 2;
                myMove.TargetObj = btnWheel;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                wheelParticle.SetActive(true);
            }
            if (level == 10)
            {
                myMove.num = 3;
                myMove.TargetObj = btnTimeBoost;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                timeBoostParticle.SetActive(true);
            }
            if (level == 11)
            {
                myMove.num = 4;
                myMove.TargetObj = btnBoosters;
                NewIteamPanel.SetActive(true);
                moveObj.SetActive(true);
                boostersParticle.SetActive(true);
            }

        }
    }

    public void disableBtnParticle(int num)
    {
        if (num == 0)
        {
            ObscuredPrefs.SetBool("UnlockedRank", false);
            rankParticle.SetActive(false);

        }
        if (num == 1)
        {
            ObscuredPrefs.SetBool("UnlockedQuest", false);
            questParticle.SetActive(false);

        }
        if (num == 2)
        {
            ObscuredPrefs.SetBool("UnlockedWheel", false);
            wheelParticle.SetActive(false);

        }

        if (num == 3)
        {
            ObscuredPrefs.SetBool("UnlockedTimeBoost", false);
            timeBoostParticle.SetActive(false);
        }

        if (num == 4)
        {
            ObscuredPrefs.SetBool("UnlockedBoosters", false);
            boostersParticle.SetActive(false);
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
