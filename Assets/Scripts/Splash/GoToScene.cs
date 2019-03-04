﻿using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
//using ArabicSupport;

public class GoToScene : MonoBehaviour
{
    public string[] tipTxtArray = new string[11];
    public Text tipTxt;
    public GameObject goToScenePanel;

    IEnumerator goToScene(string scene)
    {
        goToScenePanel.gameObject.SetActive(true);
        //int randNum = Mathf.FloorToInt(Random.Range(1000, 5000));
        //randNum = randNum % 14;
        //tipTxt.text = ArabicFixer.Fix("راهنما: " + tipTxtArray[randNum], false, true);
        yield return new WaitForSeconds(1f);

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        //StartCoroutine("showTipTxt");

        while (!ao.isDone)
        {
            // Loading completed
            //StopCoroutine("showTipTxt");
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator showTipTxt()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            int randNum = Mathf.FloorToInt(Random.Range(1000, 5000));
            randNum = randNum % 11;
            //tipTxt.text = ArabicFixer.Fix("راهنما: " + tipTxtArray[randNum], false, true);
        }
    }

    public void go(string scene)
    {
        StartCoroutine(goToScene(scene));
    }

    public void goToDizisara()
    {
        if (PlayerPrefs.GetInt("isSeenCoreHelp", 0) == 0)
        {
            go("CoreDizi");
        }
        else
        {
            go("KitchenUpgradeDizi");
        }

    }

}