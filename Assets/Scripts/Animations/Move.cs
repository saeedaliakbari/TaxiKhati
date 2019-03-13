using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(GameObject))]
public class Move : MonoBehaviour
{//in code ra be har chizi bedahim be samt target ravane mishavad
    [HideInInspector]
    public Transform target;//target hadaf mibashad




    public GameObject MoveTarget; //target farzi baraye inke gameobject deactive darim  
    public float speed;  //سرعت حرکت به سمت ابچکت 
    public Image MyImage; 
    public GameObject TargetObj;   //ابجکتی که میخوایم به سمتش برویم ولی دی اکتیو است
    public Image cup, quest, whell, time, tycoon;
    public int num;
    public Text NewIteamText;

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    private void OnEnable()
    {
        MoveTarget.transform.position=new Vector3(TargetObj.transform.position.x,TargetObj.transform.position.y,TargetObj.transform.position.z);
        target = MoveTarget.transform;
       

        if (num == 0)
        {
            MyImage.sprite = cup.sprite;

            //cup.color = new Color(1, 1, 1, 0);
            NewIteamText.text = "رتبه بندی";


        }
        else if (num == 1)
        {
            MyImage.sprite = quest.sprite;
            //quest.color = new Color(1, 1, 1, 0);
            NewIteamText.text = "ماموریت";
        }
        else if (num == 2)
        {
            MyImage.sprite = whell.sprite;
            //whell.color = new Color(1, 1, 1, 0);
            NewIteamText.text = "گردونه شانس";
        }
        else if (num == 3)
        {
            MyImage.sprite = time.sprite;
            //time.color = new Color(1, 1, 1, 0);
            NewIteamText.text = "سفر در زمان";
        }
        else
        {
            MyImage.sprite = tycoon.sprite;
            //tycoon.color = new Color(1, 1, 1, 0);
            NewIteamText.text = "شتاب دهنده";
        }


        TargetObj.SetActive(false);
        MyImage.SetNativeSize();
        MyImage.transform.localScale=new Vector3(1,1,1);
        this.transform.position = new Vector3(0, 0, 0);
        
        //MyImage.transform.localScale=new Vector3();
    }

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
            TargetObj.SetActive(true);
            //this.transform.SetParent(target, true);
            //if (num == 0)
            //{
            //    cup.color = new Color(1, 1, 1, 1);
            //}
            //else if (num == 1)
            //{
            //    quest.color = new Color(1, 1, 1, 1);
            //}
            //else if (num == 2)
            //{
            //    whell.color = new Color(1, 1, 1, 1);
            //}
            //else if (num == 3)
            //{
            //    time.color = new Color(1, 1, 1, 1);
            //}
            //else
            //{
            //    tycoon.color = new Color(1, 1, 1, 1);
            //}

            speed = 0;
            this.gameObject.SetActive(false);
        }
        //speed = (HypotenuseLength(target.position.x, this.transform.position.x) + HypotenuseLength(target.position.y, this.transform.position.y));
    }
    public bool Arrived()
    {
        if (target.position == transform.position)
            return true;
        return false;
    }

    public void StartMove()
    {
        speed = 150;
        StartCoroutine(ChangeSizeOvertime());
    }

    public IEnumerator ChangeSizeOvertime()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.015f);
            MyImage.transform.localScale=new Vector3(MyImage.transform.localScale.x-0.0175f, MyImage.transform.localScale.y - 0.0175f,1);
        }
    }

    


}
