using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXP : MonoBehaviour
{

    public Transform target;//target hadaf mibashad
    //public GameObject MoveTarget;
    public float speed;
    //public GameObject TargetObj;
    public GameObject MyGameObject;
    public GameObject Trail;

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }
    // Use this for initialization
    void Start()
    {

    }
    private void OnEnable()
    {
        Trail.SetActive(false);
        this.transform.position = new Vector3(0, 0, 0);
        Trail.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (!this.Arrived())
        {

            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.target.position, step);

            //this.transform.SetParent(target, true);
        }
        else if (this.Arrived())
        {
           
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
