﻿using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
//using ArabicSupport;

public class GoToScene : MonoBehaviour
{
    IEnumerator goToScene(string scene)
    {
        yield return new WaitForSeconds(0.5f);
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;
        while (!ao.isDone)
        {
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void go(string scene)
    {
        StartCoroutine(goToScene(scene));
    }
}