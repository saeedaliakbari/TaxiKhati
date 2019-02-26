using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconCoin : MonoBehaviour {
    public GameObject icon;
    public Text txtBalance;
    public bool isLeft;
    private int lastLentgh = 0;
    void FixedUpdate()
    {
        if (lastLentgh != txtBalance.text.Length)
        {
            lastLentgh = txtBalance.text.Length;
            UpdateIconPosition();
        }
    }
    public void UpdateIconPosition()
    {
        float length = txtBalance.text.Length * txtBalance.fontSize * 0.5f + 5;
        //Debug.Log(length);
        icon.transform.localPosition = new Vector3(isLeft ? -length / 2.5f : length / 2f, 0);
    }
	
}
