using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeCar : MonoBehaviour
{
    public Image left, right, center, hide;
    public Text txtCarName;
    //public Sprite[] hideSprites, showSprites;
    public Animator anim;
    //public Controller controller;
    public void ShowMergeCar(int fromIndex)//داخل پنل مرج انجام می شود
    {//اسپرایت های مربوط به انیمیشن مرج را تغییر می دهد با توجه به لولی که از ان داره میاد
        //Sound.instance.Play(Sound.Others.MergeNew);
        txtCarName.text = Controller.instance.carName[fromIndex + 1];
        left.sprite = Controller.instance.activeCar[fromIndex];
        right.sprite = Controller.instance.activeCar[fromIndex];
        hide.sprite = Controller.instance.inActiveCar[fromIndex + 1];
        center.sprite = Controller.instance.activeCar[fromIndex + 1];
        anim.Play("MergePlane");//انیمیشن مرج اجرا می شود
    }
}
