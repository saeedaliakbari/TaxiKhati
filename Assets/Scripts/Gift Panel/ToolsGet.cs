using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsGet : MonoBehaviour {

    public GameObject myGameObject;
    private int num,lvl;
    public bool isShown = true;
    
    public GameObject[] toolstBoxes = new GameObject[2];
    public Image[] changImages;
    public Text[] changeTexts;

    public Sprite[] lineSpeedSprite, shopOffSprite;
    
    public string[] lineSpeedStr={"+5%","+10%","+20%","+40%","+70%","+110%","+160%","+225%","+500%","+1000%"}, shopOffStr = { "ﻒﯿﻔﺨﺗ 5%", "ﻒﯿﻔﺨﺗ 15%", "ﻒﯿﻔﺨﺗ 25%", "ﻒﯿﻔﺨﺗ 35%", "ﻒﯿﻔﺨﺗ 50%", "ﻒﯿﻔﺨﺗ 55%", "ﻒﯿﻔﺨﺗ 60%", "ﻒﯿﻔﺨﺗ 65%", "ﻒﯿﻔﺨﺗ 75%", "ﻒﯿﻔﺨﺗ 85%" };
    // Use this for initialization
 
    private void OnEnable()
    {
        if (num==0)
        {
            toolstBoxes[num].SetActive(true);
            toolstBoxes[1].SetActive(false);
            changImages[num].sprite = lineSpeedSprite[lvl];
            changeTexts[num].text = lineSpeedStr[lvl];
        }
        else
        {
            toolstBoxes[0].SetActive(false);
            toolstBoxes[num].SetActive(true);
            changImages[num].sprite = shopOffSprite[lvl];
            changeTexts[num].text = shopOffStr[lvl];
        }
        StartCoroutine(waitForDeactive());
        isShown = true;
    }
    // Update is called once per frame
    private IEnumerator waitForDeactive()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }

    public void changeNum(int number,int level)
    {
        isShown = false;
        num = number;
        lvl = level;
    }

    public void openPanel()
    {
        if (!isShown)
        {
            myGameObject.SetActive(true);
        }
    }
}
