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
    public Image imgBack, imgSlider, imgTik;
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
        //Debug.Log(id + ">gem: " + gem + "txtGem>" + txtGem.text + " > " + ObscuredPrefs.GetInt(stringPrefs + id, 0) + "/" + max + ">>" + ObscuredPrefs.GetInt(stringPrefs + id, 0) / max);
        imgSlider.fillAmount = (float)ObscuredPrefs.GetInt(stringPrefs + id, 0) / max;
        txtSlider.text = (ObscuredPrefs.GetInt(stringPrefs + id, 0) >= max ? max : ObscuredPrefs.GetInt(stringPrefs + id, 0)) + "/" + max;
        if (ObscuredPrefs.GetInt(stringPrefs + "Get" + id, 0) == 1)//geted
        {
            imgBack.color = inActvie;
            btnGet.gameObject.SetActive(false);
            objGemIcon.SetActive(false);
            objGemTxt.SetActive(false);
            objSlider.SetActive(false);
            listAchvGet.Add(gameObject);
            imgTik.gameObject.SetActive(true);
        }
        else if (ObscuredPrefs.GetInt(stringPrefs + id, 0) >= max)//unlock
        {
            btnGet.interactable = true;
            controller.achivmentManager.lblNew.SetActive(true);
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
        imgTik.gameObject.SetActive(true);
        Debug.Log("GEM> " + ObscuredPrefs.GetDouble("gem", 0));
        ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 0) + gem);
        Debug.Log("GEM> " + ObscuredPrefs.GetDouble("gem", 0));
        ObscuredPrefs.SetInt(stringPrefs + "Get" + id, 1);
        //txtGem.text = ObscuredPrefs.GetDouble("gem", 0).ToString();
        controller.SetText();
        btnGet.gameObject.SetActive(false);
        objGemIcon.SetActive(false);
        objGemTxt.SetActive(false);
        objSlider.SetActive(false);
        controller.achivmentManager.OpenPanel();
    }
}
