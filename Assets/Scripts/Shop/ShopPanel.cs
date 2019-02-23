using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopPanel : MonoBehaviour
{
    public CarItem[] carItems;
    //public Controller controller;
    public Button btnTaxi, btnOther;
    public Image imgSelectTaxi, imgUnselectTaxi;
    public Image imgSelectOther, imgUnselectOther;
    public GameObject objTaxi, objOther;
    public Color clrSel, clrUnSel;
    public Outline txtTaxi, txtOther;
    private int def = 0;
    void Start()
    {
        BtnSelect(true);
    }
    public void UpdateCarItems()
    {
        int unloacked = PlayerPrefs.GetInt("unlocked_car", 1);
        //Debug.Log("unloacked" + unloacked + "lastSalableLevel>" + controller.lastSalableLevel[unloacked - 1]);
        carItems[0].UpdateCarItem(true, Controller.instance.lastSalableLevel[unloacked - 1], 0);
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
            carItems[i].UpdateCarItem(i < Controller.instance.lastSalableLevel[unloacked - 1], Controller.instance.lastSalableLevel[unloacked - 1], def);
        }
    }

    public void BuyCarClick(int index)
    {
        Controller.instance.CheckAndSpawnNewCar(index - 1, true, 2);
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
