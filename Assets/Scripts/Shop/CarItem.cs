using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarItem : MonoBehaviour
{
    public Image currency, car, earnSlider, speedSlider;
    public Text price, txtName;
    public Sprite greenBtn, lockBtn, coinSprite, gemSprite;
    public Button btnBuy;
    public Controller controller;
    public void UpdateCarItem(bool visible)
    {//وضعیت ماشین را بروز رسانی می کند در فروشگاه
        int index = transform.GetSiblingIndex();//یعنی از نظر فرزندی برای پدرخود چندم است در هایرارکی
        Debug.Log("index : " + index);
        txtName.text = controller.carName[index];
        car.sprite = visible ? controller.activeCar[index] : controller.inActiveCar[index];//با توجه به فعال یا غیرفعال بودن اسپرایت فعال یا غیرفعال می شود
        float priceValue = index < 49 ? PlayerPrefs.GetFloat("car_price_" + index, (int)(controller.basePrice[index] * (1 + controller.increaseRate[index]))) : PlayerPrefs.GetFloat("car_price_gem_" + index, (int)Mathf.Pow(2, (index - 6)));
        priceValue = priceValue * PlayerPrefs.GetFloat("offCar", 1);
        float balance = index < 49 ? PlayerPrefs.GetFloat("coin", 5000) : PlayerPrefs.GetFloat("gem", 0);
        currency.gameObject.SetActive(visible);
        currency.sprite = index < 8 ? coinSprite : gemSprite;
        currency.sprite = coinSprite;
        price.text = visible ? priceValue.ToString() : "";
        btnBuy.interactable = visible && balance >= priceValue;
        btnBuy.GetComponent<Image>().sprite = visible ? greenBtn : lockBtn;
        earnSlider.fillAmount = controller.earning[index]/* / controller.earning[49]*/;
        speedSlider.fillAmount = controller.speed[index] / controller.speed[49];
        //Debug.Log(index + ">>" + earnSlider.fillAmount);
    }
}

