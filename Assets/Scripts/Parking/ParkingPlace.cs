using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingPlace : MonoBehaviour
{
    public Car GetCar()
    {//اگر ماشینی داخل این پارکینگ باشد اونو برمیگردونه
        if (transform.childCount > 0 && transform.GetChild(0).tag == "Car")
        {
            return transform.GetChild(0).GetComponent<Car>();
        }
        return null;
    }

    public GiftBox GetBox()//اگر باکسی داخل این پارکینگ هست را برمیگردارند
    {
        if (transform.childCount > 0 && transform.GetChild(0).tag == "Box")
        {
            return transform.GetChild(0).GetComponent<GiftBox>();
        }
        return null;
    }

    public bool IsEmpty()//اگر پارکینگ خالی باشد درست برمیگرداند و درغیراینصورت غلط برمیگرداند
    {
        return transform.childCount == 0;
    }

    public void CheckAutoOpenGift()//بصورت اتوماتیک باز شود باکس داخل پارکینگ
    {
        if (transform.childCount > 0 && transform.GetChild(0).tag == "Box")
        {
            transform.GetChild(0).GetComponent<GiftBox>().StartAutoOpen();
        }
    }

    public int GetPlaceIndex()
    {//عدد فرزندی را برمیگرداند
        return transform.GetSiblingIndex();//یعنی از نظر فرزندی برای پدر خود عدد چند است
    }
}
