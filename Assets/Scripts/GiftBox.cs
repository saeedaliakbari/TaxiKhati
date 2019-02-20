using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    public SpriteRenderer spriteBox;
    public Sprite[] sprBox;//0: normal , 1:special, 2: store;
    [HideInInspector]
    public ParkingPlace parkPlace;
    [HideInInspector]
    public int carIndex;
    private bool opened = false;

    public void SetUpBox(int index, ParkingPlace place,int modelBox)
    {//ساختن باکس جدید درصفحه
        carIndex = index;//شماره ماشین
        spriteBox.sprite = sprBox[modelBox];//باکس در لول های پایین تر از 4 و بالاتر از 4 متفاوت است
        parkPlace = place;//بهش پارکینگ را هم می دهد
    }
    public void StartAutoOpen()//بعد از 3 ثانیه باکس خودش باز شود
    {
        Timer.Schedule(this, 3f, () =>
        {
            OnMouseUp();
        });
    }

    public void OpenBox()//باز شدن باکس 
    {
        opened = true;
        GetComponent<Animator>().SetTrigger("open");
    }

    private void OnMouseUp()//با موس روش کلیک کنی وبرداری
    {
        //Debug.Log("On Mouse Up: "+opened);
        if (!opened)//اگر باز نشده بود
        {
            OpenBox();//انیمیشن باز شدن اجرا شود
            Timer.Schedule(this, 0.5f, (Timer.Task)(() =>
            {
                Controller.instance.SpawnACar((int)carIndex, (ParkingPlace)parkPlace);
                Destroy(gameObject);
            }));//بعد از نیم ثانیه که انیمیشن تموم شد ماشین داخل پارکینگ مورد نظر ایجاد شود
        }
    }
}
