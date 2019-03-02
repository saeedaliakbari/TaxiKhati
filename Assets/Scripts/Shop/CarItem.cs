using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarItem : MonoBehaviour
{
    public Image car, earnSlider, speedSlider, imgLock, imgOff;
    public Text txtCoin, txtGem, txtName, txtOff;
    public Button btnBuyCoin, btnBuyGem, btnVideo;
    public Controller controller;

    public void UpdateCarItem(bool visible, int lastSalableLevel, int defrenceLastSalableLevel)
    {//وضعیت ماشین را بروز رسانی می کند در فروشگاه
        int index = transform.GetSiblingIndex();//یعنی از نظر فرزندی برای پدرخود چندم است در هایرارکی
        double balance;
        offDetection(visible);
        txtName.text = visible ? controller.carName[index] : "???";
        car.sprite = visible ? controller.activeCar[index] : controller.inActiveCar[index];//با توجه به فعال یا غیرفعال بودن اسپرایت فعال یا غیرفعال می شود
        earnSlider.fillAmount = controller.earning[index]/* / controller.earning[49]*/;
        speedSlider.fillAmount = controller.speed[index] / controller.speed[49];
        int coinSalable = lastSalableLevel - defrenceLastSalableLevel;
        bool coin = index < coinSalable;
        bool video = false;
        if (lastSalableLevel > 3)
        {
            video = (index == coinSalable - 1);
        }
        //Debug.Log("Index: " + index + " coinSalable: " + coinSalable + " Video Salable : " + video+" COin"+ coin);
        imgLock.gameObject.SetActive(!visible);
        if (video && ObscuredPrefs.GetInt("mergeCarForVideo", 1) >= 10)
        {
            btnVideo.gameObject.SetActive(visible);
            btnBuyCoin.gameObject.SetActive(false);
            btnBuyGem.gameObject.SetActive(false);
        }
        else
        {
            btnVideo.gameObject.SetActive(false);
            btnBuyCoin.gameObject.SetActive(coin && visible);
            btnBuyGem.gameObject.SetActive(!coin && visible);
            if (coin)
            {
                double coinPrice = ObscuredPrefs.GetDouble("car_price_" + index, System.Math.Round(controller.basePrice[index]));
                //Debug.Log(index+">coinPrice : " + coinPrice);
                coinPrice = coinPrice * ObscuredPrefs.GetFloat("offCar", 1);
                txtCoin.text = coinPrice.ToString();
                balance = ObscuredPrefs.GetDouble("coin", 5000);
                btnBuyCoin.interactable = visible && balance >= coinPrice;
            }
            else
            {
                txtGem.text = controller.baseGemPrice[index].ToString();
                balance = ObscuredPrefs.GetDouble("gem", 0);
                btnBuyGem.interactable = visible && balance >= controller.baseGemPrice[index];
            }
        }
    }
    private void offDetection(bool visible)
    {
        float off = 1 - ObscuredPrefs.GetFloat("offCar", 1f);
        if (off != 0 && visible)
        {
            imgOff.gameObject.SetActive(true);
            txtOff.text = "%" + Mathf.Round(off * 100);
        }
        else
        {
            imgOff.gameObject.SetActive(false);
        }
    }
}

