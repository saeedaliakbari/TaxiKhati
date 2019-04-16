using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class Earning5X : MonoBehaviour
{

    public GameObject coinRainParticle;
    // Use this for initialization
    void Start()
    {
        if (Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m") || Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m_special"))
        {
            coinRainParticle.SetActive(true);
            if (Manager.GetActionTime("5x_earning_for_1m") > Manager.GetActionTime("5x_earning_for_1m_special"))
            {
                StartCoroutine(NextCheck(Manager.GetActionTime("5x_earning_for_1m") - Manager.GetCurrentTime()));
            }
            else
            {
                StartCoroutine(NextCheck(Manager.GetActionTime("5x_earning_for_1m_special") - Manager.GetCurrentTime()));
            }
        }
        else
        {
            coinRainParticle.SetActive(false);
        }
    }

    public void check()
    {
        Start();
    }
    public IEnumerator NextCheck(double nextCheeck)
    {
        yield return new WaitForSeconds((float)nextCheeck);
        Start();
    }
}
