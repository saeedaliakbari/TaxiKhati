using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftPanel : MonoBehaviour
{
    public int num;
    public GameObject [] giftBoxes=new GameObject[5];
	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        for (int i = 0; i < giftBoxes.Length; i++)
        {
            giftBoxes[i].SetActive(false);
        }
        giftBoxes[num].SetActive(true);
        StartCoroutine(waitForDeactive());
    }
    // Update is called once per frame
    private IEnumerator waitForDeactive()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
