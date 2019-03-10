using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXP : MonoBehaviour {

    public Transform target;//target hadaf mibashad
    public GameObject MoveTarget;
    public float speed;
    public GameObject TargetObj;
    [HideInInspector]
    public int num;

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.Arrived())
        {

            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.target.position, step);

            //this.transform.SetParent(target, true);
        }
        else if (this.Arrived())
        {
            speed = 0;
            this.gameObject.SetActive(false);
        }

    }
    public bool Arrived()
    {
        if (target.position == transform.position)
            return true;
        return false;
    }
}
