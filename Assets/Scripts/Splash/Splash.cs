﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class Splash : MonoBehaviour
{
    public GoToScene goToSecne;

    void Start()
    {
        GameAnalytics.Initialize();
        goToSecne.go("Main");
    }

}