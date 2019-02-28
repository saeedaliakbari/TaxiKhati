using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWait : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Hashtable hash = iTween.Hash("path", iTweenPath.GetPath("RoadWait"), "orienttopath", true, "speed", 150f, "easetype", iTween.EaseType.linear, "oncomplete", "OnComplete");//ماشین را به مکان مشخص شده حرکت می دهد
        iTween.MoveTo(gameObject, hash);
    }
    private void OnComplete()
    {
        Hashtable hash = iTween.Hash("path", iTweenPath.GetPath("RoadWait"), "orienttopath", true, "speed", 10f, "easetype", iTween.EaseType.linear, "oncomplete", "OnComplete");//ماشین را به مکان مشخص شده حرکت می دهد
        iTween.MoveTo(gameObject, hash);
    }

}
