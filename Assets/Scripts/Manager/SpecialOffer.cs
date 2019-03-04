using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpecialOffer : MonoBehaviour
{
    public SpecialOfferObj prefabSpecialOffer;
    public Controller controller;
    private SpecialOfferObj specialOfferObj;
    public GameObject panelOffer, objIncome, objGoldBox;
    public Button btnVideo, btnGem;
    // Use this for initialization
    void Start()
    {
        Timer.Schedule(this, 30f, () =>
         {
             if (ObscuredPrefs.GetInt("helpStep", 0) == 13)
                 NewOffer();
         });
    }
    private void NewOffer()
    {
        Debug.Log("New Offer");
        specialOfferObj = (SpecialOfferObj)Instantiate(prefabSpecialOffer, Vector3.zero, Quaternion.EulerAngles(0f, 0f, 0f)/*, Quaternion.identity*/);
        specialOfferObj.transform.localScale = Vector3.one * 0.4f;//اسکیل ماشین در حال حرکت ایجاد شده
        specialOfferObj.transform.position = controller.slotManager.transform.position;//موقعیت ماشین درحال حرکت در مکان استارت قرار میگیرد
        specialOfferObj.specialOffer = this;
        specialOfferObj.giftBox = (Random.Range(0, 10000) % 2 == 0);
        objGoldBox.SetActive(specialOfferObj.giftBox);
        objIncome.SetActive(!specialOfferObj.giftBox);
        specialOfferObj.DiverARound();//حرکت ماشین 
    }


    public void OfferNext()
    {
        Destroy(specialOfferObj.gameObject);
        Start();
    }

    public void ManageGift()
    {
        if (specialOfferObj.giftBox)//4 تا باکس طلایی
        {
            for (int i = 0; i < 4; i++)
            {
                controller.SpawnABoxSpecialOffer();
            }
            //controller.txtPanelMessage.text = "4 جعبه طلایی به پارکینگ شما اضافه شد";
        }
        else//درآمد 5 برابر برای یک دقیقه
        {
            if (Manager.GetCurrentTime() < Manager.GetActionTime("5x_earning_for_1m_special"))
            {
                Manager.SetActionTime("5x_earning_for_1m_special", (Manager.GetActionTime("5x_earning_for_1m_special") + 60 + Manager.GetCurrentTime()));
            }
            else {
                Manager.SetActionTime("5x_earning_for_1m_special", (60 + Manager.GetCurrentTime()));
            }
            //controller.txtPanelMessage.text = "به مدت 1 دقیقه در آمد شما 5 برابر شد";
            controller.slotManager.UpdateEarningSpeedText();
        }
        panelOffer.SetActive(false);
        Destroy(specialOfferObj.gameObject);
        //controller.panelMessage.SetActive(true);
    }
    public void BtnBuyOffer()
    {
        if (ObscuredPrefs.GetDouble("gem", 0) >= 3)
        {
            btnGem.interactable = false;
            btnVideo.interactable = false;
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") - 5);
            controller.SetText();
            ManageGift();
        }
        else
        {
            Debug.Log("Gem<5");
        }
    }
}

