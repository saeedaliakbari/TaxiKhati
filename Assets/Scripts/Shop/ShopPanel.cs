using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopPanel : MonoBehaviour
{
    public CarItem[] carItems;
    public Controller controller;
    public Button btnTaxi, btnOther, btnBuyCore;
    public Image imgSelectTaxi, imgUnselectTaxi;
    public Image imgSelectOther, imgUnselectOther;
    public GameObject objTaxi, objOther;
    public GameObject[] objLblNews;
    public Color clrSel, clrUnSel;
    public Outline txtTaxi, txtOther;
    public ScrollRect scrCar;
    //public Scrollbar sbrCar;
    private int def = 0;
    void Start()
    {
        BtnSelect(true);
    }
    public void OpenPanel()
    {
        int unloacked = ObscuredPrefs.GetInt("unlocked_car", 1);
        Debug.Log("unload Car : " + unloacked);
        scrCar.verticalNormalizedPosition = 1;
        //sbrCar.value = scrCar.verticalNormalizedPosition;
        //if (unloacked > 22)
        //{
        //Debug.Log(unloacked + "> " + scrCar.verticalNormalizedPosition);
        //    scrCar.verticalNormalizedPosition = 1 - ((unloacked - 7) * 0.0215f);
        //    sbrCar.value = scrCar.verticalNormalizedPosition;
        //    Debug.Log("def=2>" + scrCar.verticalNormalizedPosition + ">" + sbrCar.value);
        //}
        //else 
        if (unloacked >6)
        {
            scrCar.verticalNormalizedPosition = 1 - ((unloacked - 6) * 0.0213f);
            //sbrCar.value = scrCar.verticalNormalizedPosition;
        }
        //else if (unloacked > 6)
        //{
        //    scrCar.verticalNormalizedPosition = 1 - ((unloacked - 5) * 0.0215f);
        //    //sbrCar.value = scrCar.verticalNormalizedPosition;
        //}
        //Debug.Log(unloacked + "> " + scrCar.verticalNormalizedPosition);
    }
    public void UpdateCarItems()
    {
        for (int i = 0; i < carItems.Length; i++)
        {
            carItems[i].objLblNew = objLblNews[i];
            carItems[i].btnBuyCore = btnBuyCore;
        }
        int unloacked = ObscuredPrefs.GetInt("unlocked_car", 1);
        //Debug.Log("unloacked" + unloacked + "lastSalableLevel>" + controller.lastSalableLevel[unloacked - 1]);
        carItems[0].UpdateCarItem(0, controller.lastSalableLevel[unloacked - 1], 0);

        for (int i = 1; i < carItems.Length; i++)
        {
            if (unloacked == 2)
            {
                def = 1;
            }
            else
            {
                def = 2;
            }
            carItems[i].UpdateCarItem(i, controller.lastSalableLevel[unloacked - 1], def);
        }
    }

    public void BuyCarClick(int index)
    {
        controller.CheckAndSpawnNewCar(index - 1, true, 2);
        UpdateCarItems();
    }

    public void BtnSelect(bool taxi)
    {
        objTaxi.SetActive(taxi);
        objOther.SetActive(!taxi);
        imgSelectTaxi.gameObject.SetActive(taxi);
        imgUnselectTaxi.gameObject.SetActive(!taxi);
        imgUnselectOther.gameObject.SetActive(taxi);
        imgSelectOther.gameObject.SetActive(!taxi);
        if (taxi)
        {
            btnTaxi.targetGraphic = imgSelectTaxi;
            btnOther.targetGraphic = imgUnselectOther;
            txtTaxi.effectColor = clrSel;
            txtOther.effectColor = clrUnSel;
        }
        else
        {
            btnTaxi.targetGraphic = imgUnselectTaxi;
            btnOther.targetGraphic = imgSelectOther;
            txtTaxi.effectColor = clrUnSel;
            txtOther.effectColor = clrSel;
        }
    }
}
