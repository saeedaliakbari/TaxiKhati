using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(GameObject))]
public class Move : MonoBehaviour {//in code ra be har chizi bedahim be samt target ravane mishavad

	public Transform target;//target hadaf mibashad
	public  float speed;
    public Image MyImage;
    public GameObject TargetObj;
    public Image[] Images;
    public int num;

	float HypotenuseLength(float sideALength, float sideBLength) 
	{
		return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
	}
	void Start(){

        
        
	}

    private void OnEnable()
    {
        MyImage.sprite = Images[num].sprite;
        Images[num].color=new Color(1,1,1,0);
        MyImage.SetNativeSize();
        //MyImage.transform.localScale=new Vector3();
    }

    void Update() {
	    if (!this.Arrived())
	    {

	        float step = speed * Time.deltaTime;
	        this.transform.position = Vector3.MoveTowards(this.transform.position, this.target.position, step);
	        //this.transform.SetParent(target, true);
	    }
	    else if (this.Arrived())
        {
            this.transform.SetParent(target, true);
        }
        //speed = (HypotenuseLength(target.position.x, this.transform.position.x) + HypotenuseLength(target.position.y, this.transform.position.y));
    }
	public bool Arrived(){
		if (target.position == transform.position)
			return true;
		return false;
	}


}
