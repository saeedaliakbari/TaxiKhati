using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchivmentPrefab : MonoBehaviour
{
    private int id, max, gem;
    private string stringPrefs;
    public Text txtTitle, txtGem;
    public GameObject objBtnGet;
    public Color inActvie;
    public Image imgBack;
    public void Setup(Achivment achv, string stringPrefs, List<GameObject> listAchvGet, List<GameObject> listAchvDone, List<GameObject> listAchv)
    {
        id = achv.id;
        max = achv.max;
        gem = achv.rewardGem;
        this.stringPrefs = stringPrefs;
        txtTitle.text = achv.title;
        txtGem.text = gem.ToString();
        if (PlayerPrefs.GetInt(stringPrefs + "Get" + id, 0) == 1)//geted
        {
            imgBack.color = inActvie;
            objBtnGet.SetActive(false);
            listAchvGet.Add(gameObject);
        }
        else if (PlayerPrefs.GetInt(stringPrefs + id, 0) >= max)//unlock
        {
            objBtnGet.SetActive(true);
            listAchvDone.Add(gameObject);
        }
        else//lock
        {
            objBtnGet.SetActive(false);
            listAchv.Add(gameObject);

        }
    }
    public void GetGift()
    {
        PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem", 0) + gem);
        PlayerPrefs.SetInt(stringPrefs + "Get" + id, 1);
        txtGem.text = PlayerPrefs.GetFloat("gem", 0).ToString();
        objBtnGet.SetActive(false);
    }
}
