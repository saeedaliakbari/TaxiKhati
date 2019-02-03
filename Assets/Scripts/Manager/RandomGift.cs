using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGift : MonoBehaviour
{
    public GameObject panelRandomGift;
    // Use this for initialization
    void Start()
    {
        //Debug.Log("LeveL: " + PlayerPrefs.GetInt("Level", 1));
        if (PlayerPrefs.GetInt("Level", 1) >= 6)
        {
            Debug.Log("Panel Random Gift Active");
            //    panelRandomGift.SetActive(true);
        }
    }
}
