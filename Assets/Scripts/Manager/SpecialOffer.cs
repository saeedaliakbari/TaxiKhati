using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpecialOffer : MonoBehaviour
{
    public SpecialOfferObj prefabSpecialOffer;
    public Controller controller;
    [HideInInspector]
    public SpecialOfferObj specialOfferObj;
    public GameObject panelOffer, objIncome, objGoldBox;
    public Button btnVideo, btnGem;

    private int farmingtime;
    // Use this for initialization
    void Start()
    {
        Timer.Schedule(this, 30f, () =>
         {
             if (ObscuredPrefs.GetInt("helpStep", 0) == 22)
                 NewOffer();
         });
        //Debug.Log("5x_earning_for_1m_special: " + (Manager.GetActionTime("earning_5x_for_1m_special") - Manager.GetCurrentTime()));
        //Debug.Log("5x_earning_for_1m: " + (Manager.GetActionTime("earning_5x_for_1m") - Manager.GetCurrentTime()));
        //Debug.Log("2x_speed_for_150s: " + (Manager.GetActionTime("speed_2x_for_150s") - Manager.GetCurrentTime()));
        if (ObscuredPrefs.GetBool("setTimer", false))
        {
            ObscuredPrefs.SetBool("setTimer", true);
            if (Manager.GetActionTime("earning_5x_for_1m_special") - Manager.GetCurrentTime() > 180)
            {
                Manager.SetActionTime("earning_5x_for_1m_special", Manager.GetCurrentTime() + 180);
            }
            if (Manager.GetActionTime("earning_5x_for_1m") - Manager.GetCurrentTime() > 180)
            {
                Manager.SetActionTime("earning_5x_for_1m", Manager.GetCurrentTime() + 180);
            }
            if (Manager.GetActionTime("speed_2x_for_150s") - Manager.GetCurrentTime() > 450)
            {
                Manager.SetActionTime("speed_2x_for_150s", Manager.GetCurrentTime() + 450);
            }
        }
        //Debug.Log("5x_earning_for_1m_special: " + (Manager.GetActionTime("earning_5x_for_1m_special") - Manager.GetCurrentTime()));
        //Debug.Log("5x_earning_for_1m: " + (Manager.GetActionTime("earning_5x_for_1m") - Manager.GetCurrentTime()));
        //Debug.Log("2x_speed_for_150s: " + (Manager.GetActionTime("speed_2x_for_150s") - Manager.GetCurrentTime()));
    }
    private void NewOffer()
    {
        //Debug.Log("New Offer");
        specialOfferObj = (SpecialOfferObj)Instantiate(prefabSpecialOffer, Vector3.zero, Quaternion.EulerAngles(0f, 0f, 0f)/*, Quaternion.identity*/);
        specialOfferObj.transform.localScale = Vector3.one * 0.4f;//اسکیل ماشین در حال حرکت ایجاد شده
        specialOfferObj.transform.position = controller.slotManager.transform.position;//موقعیت ماشین درحال حرکت در مکان استارت قرار میگیرد
        specialOfferObj.specialOffer = this;
        specialOfferObj.giftBox = (Random.Range(0, 100) % 2 == 0);
        objGoldBox.SetActive(specialOfferObj.giftBox);
        objIncome.SetActive(!specialOfferObj.giftBox);
        specialOfferObj.DiverARound();//حرکت ماشین 
        if (ObscuredPrefs.GetInt(controller.videoAds.zoneGiftPanel.zoneId) == 0)
        {
            controller.videoAds.LoadAd(controller.videoAds.zoneGiftPanel, false);
        }
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
                controller.SpawnABoxSpecialOffer(i);
            }
            //controller.txtPanelMessage.text = "4 جعبه طلایی به پارکینگ شما اضافه شد";
            controller.myGiftPanel.changeNum(1);
        }
        else//درآمد 5 برابر برای یک دقیقه
        {
            if (Manager.GetCurrentTime() < Manager.GetActionTime("earning_5x_for_1m_special"))
            {
                Manager.SetActionTime("earning_5x_for_1m_special", (Manager.GetActionTime("earning_5x_for_1m_special") + 60));

            }
            else {
                Manager.SetActionTime("earning_5x_for_1m_special", (60 + Manager.GetCurrentTime()));

            }
            controller.earning5X.check();
            //controller.txtPanelMessage.text = "به مدت 1 دقیقه در آمد شما 5 برابر شد";
            controller.slotManager.UpdateEarningSpeedText();
            controller.myGiftPanel.changeNum(3);
        }
        panelOffer.SetActive(false);
        controller.myGiftPanel.openPanel();
        Destroy(specialOfferObj.gameObject);
        //controller.panelMessage.SetActive(true);
    }
    public void BtnBuyOffer()
    {
        if (ObscuredPrefs.GetDouble("gem", 0) >= 3)
        {
            btnGem.interactable = false;
            btnVideo.interactable = false;
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem") - 3);
            controller.SetText();
            ManageGift();
        }
        else
        {
            controller.panelNoGem.SetActive(true);
            controller.parkingManager.DisableCarInPark();
            //Debug.Log("Gem<5");
        }
    }

}

