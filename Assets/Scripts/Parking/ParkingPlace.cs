using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParkingPlace : MonoBehaviour
{
    public Animator animLight,UnlockPlace;
    public GameObject objBack;
    public MoveXP XPTrail;
    public GameObject helpPlace;
    public Car GetCar()
    {//اگر ماشینی داخل این پارکینگ باشد اونو برمیگردونه
        if (transform.childCount > 2 && transform.GetChild(2).tag == "Car")
        {
            return transform.GetChild(2).GetComponent<Car>();
        }
        return null;
    }

    public GiftBox GetBox()//اگر باکسی داخل این پارکینگ هست را برمیگردارند
    {
        if (transform.childCount > 2 && transform.GetChild(2).tag == "Box")
        {
            return transform.GetChild(2).GetComponent<GiftBox>();
        }
        return null;
    }

    public bool IsEmpty()//اگر پارکینگ خالی باشد درست برمیگرداند و درغیراینصورت غلط برمیگرداند
    {
        return transform.childCount == 2;
    }

    public void CheckAutoOpenGift()//بصورت اتوماتیک باز شود باکس داخل پارکینگ
    {
        if (transform.childCount > 2 && transform.GetChild(2).tag == "Box")
        {
            transform.GetChild(2).GetComponent<GiftBox>().StartAutoOpen(3f);
        }
    }

    public int GetPlaceIndex()
    {//عدد فرزندی را برمیگرداند
        return transform.GetSiblingIndex();//یعنی از نظر فرزندی برای پدر خود عدد چند است
    }


    public void UnlockAnimation()
    {
        StartCoroutine(UnlockParkingIEnumerator());
    }

    public IEnumerator UnlockParkingIEnumerator()
    {
        yield return new WaitForSeconds(0.1f);
        UnlockPlace.Play("Open");
        yield return new WaitForSeconds(0.11f);
        animLight.Play("Open");
    }
}
