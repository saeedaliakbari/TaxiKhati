using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchivmentPrefab : MonoBehaviour
{
    private int id, max, gem;
    private string stringPrefs;
    public Text txtTitle, txtGem, txtSlider;
    public Button btnGet;
    public Color inActvie;
    public Image imgBack, imgSlider;
    public GameObject objSlider, objGemIcon, objGemTxt;
    [HideInInspector]
    public Controller controller;
    public void Setup(Achivment achv, string stringPrefs, List<GameObject> listAchvGet, List<GameObject> listAchvDone, List<GameObject> listAchv)
    {
        id = achv.id;
        max = achv.max;
        gem = achv.rewardGem;
        this.stringPrefs = stringPrefs;
        txtTitle.text = achv.title;
        txtGem.text = achv.rewardGem.ToString();
        Debug.Log(id+ ">gem: "+gem +"txtGem>"+txtGem.text+ " > " + PlayerPrefs.GetInt(stringPrefs + id, 0) + "/" + max + ">>" + PlayerPrefs.GetInt(stringPrefs + id, 0) / max);
        imgSlider.fillAmount = (float)PlayerPrefs.GetInt(stringPrefs + id, 0) / max;
        txtSlider.text = (PlayerPrefs.GetInt(stringPrefs + id, 0) >= max ? max : PlayerPrefs.GetInt(stringPrefs + id, 0)) + "/" + max;
        if (PlayerPrefs.GetInt(stringPrefs + "Get" + id, 0) == 1)//geted
        {
            imgBack.color = inActvie;
            btnGet.gameObject.SetActive(false);
            objGemIcon.SetActive(false);
            objGemTxt.SetActive(false);
            objSlider.SetActive(false);
            listAchvGet.Add(gameObject);
        }
        else if (PlayerPrefs.GetInt(stringPrefs + id, 0) >= max)//unlock
        {
            btnGet.interactable = true;
            listAchvDone.Add(gameObject);
        }
        else//lock
        {
            btnGet.interactable = false;
            listAchv.Add(gameObject);

        }
    }
    public void GetGift()
    {
        imgBack.color = inActvie;
        Debug.Log("GEM> " + ObscuredPrefs.GetDouble("gem", 0));
        ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 0) + gem);
        Debug.Log("GEM> " + ObscuredPrefs.GetDouble("gem", 0));
        PlayerPrefs.SetInt(stringPrefs + "Get" + id, 1);
        //txtGem.text = ObscuredPrefs.GetDouble("gem", 0).ToString();
        controller.SetText();
        btnGet.gameObject.SetActive(false);
        objGemIcon.SetActive(false);
        objGemTxt.SetActive(false);
        objSlider.SetActive(false);
    }
}
