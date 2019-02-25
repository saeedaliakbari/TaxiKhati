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
    public Controller controller;
    public void ShowMergeCar(int fromIndex)//داخل پنل مرج انجام می شود
    {//اسپرایت های مربوط به انیمیشن مرج را تغییر می دهد با توجه به لولی که از ان داره میاد
        //Sound.instance.Play(Sound.Others.MergeNew);
        txtCarName.text = controller.carName[fromIndex + 1];
        left.sprite = controller.activeCar[fromIndex];
        right.sprite = controller.activeCar[fromIndex];
        hide.sprite = controller.inActiveCar[fromIndex + 1];
        center.sprite = controller.activeCar[fromIndex + 1];
        anim.Play("MergePlane");//انیمیشن مرج اجرا می شود
    }
}
