using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public CarItem[] carItems;
    public Controller controller;
    private int def = 0;
    public void UpdateCarItems()
    {
        int unloacked = PlayerPrefs.GetInt("unlocked_car", 1);
        //Debug.Log("unloacked" + unloacked + "lastSalableLevel>" + controller.lastSalableLevel[unloacked - 1]);
        carItems[0].UpdateCarItem(true, controller.lastSalableLevel[unloacked - 1], 0);
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
            carItems[i].UpdateCarItem(i < controller.lastSalableLevel[unloacked - 1], controller.lastSalableLevel[unloacked - 1], def);
        }
    }

    public void BuyCarClick(int index)
    {
        Controller.instance.CheckAndSpawnNewCar(index, true);
        UpdateCarItems();
    }


}
