﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GameAnalyticsSDK;

public class Splash : MonoBehaviour
{
    public GoToScene goTo;

    void Start()
    {
        //GameAnalytics.Initialize();
        goTo.go("Main");
    }

}