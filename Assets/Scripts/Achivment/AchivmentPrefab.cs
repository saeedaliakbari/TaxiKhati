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
    public void Setup(Achivment achv, string stringPrefs, List<GameObject> listAchvGet, List<GameObject> listAchvDone, List<GameObject> listAchv)
    {
        id = achv.id;
        max = achv.max;
        gem = achv.rewardGem;
        this.stringPrefs = stringPrefs;
        txtTitle.text = achv.title;
        txtGem.text = gem.ToString();
        Debug.Log(id + ">" + PlayerPrefs.GetInt(stringPrefs + id, 0) + "/" + max + ">>" + PlayerPrefs.GetInt(stringPrefs + id, 0) / max);
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
        PlayerPrefs.SetFloat("gem", PlayerPrefs.GetFloat("gem", 0) + gem);
        PlayerPrefs.SetInt(stringPrefs + "Get" + id, 1);
        //txtGem.text = PlayerPrefs.GetFloat("gem", 0).ToString();
        Controller.instance.SetText();
        btnGet.gameObject.SetActive(false);
        objGemIcon.SetActive(false);
        objGemTxt.SetActive(false);
        objSlider.SetActive(false);
    }
}
