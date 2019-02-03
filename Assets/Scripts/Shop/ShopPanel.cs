using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public CarItem[] carItems;
    public void UpdateCarItems()
    {
        int unloacked = PlayerPrefs.GetInt("unlocked_car");
        Debug.Log("unloacked" + unloacked);
        carItems[0].UpdateCarItem(true);
        for (int i = 1; i < carItems.Length; i++)
        {
            carItems[i].UpdateCarItem(i < unloacked - 1);
        }
    }

    public void BuyCarClick(int index)
    {
        Controller.instance.CheckAndSpawnNewCar(index, true);
        UpdateCarItems();
    }

}
