using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsGet : MonoBehaviour {

    public GameObject myGameObject;
    public int num,lvl;
    public bool isShown = true;
    public GameObject[] toolstBoxes = new GameObject[2];
    public Sprite[] lineSpeedSprite = new Sprite[9],shopOffSprite=new Sprite[9];
    // Use this for initialization
 
    private void OnEnable()
    {
        for (int i = 0; i < toolstBoxes.Length; i++)
        {
            toolstBoxes[i].SetActive(false);
        }
        toolstBoxes[num].SetActive(true);
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
