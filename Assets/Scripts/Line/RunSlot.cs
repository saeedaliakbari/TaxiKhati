using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RunSlot : MonoBehaviour
{
    public Sprite[] empty, full;
    public SpriteRenderer spriteRun;
    public void UpdateSprite(bool hasCar)
    {
        //int index = Superpow.Utils.GetUnlockedAirlineLevel() - 1;
        int index = ObscuredPrefs.GetInt("unlocked_line", 1) - 1;//شماره لاین استارت است که در ابتدا یک است و بعد از آن آپدیت می تواند بشود.
        spriteRun.sprite = hasCar ? full[index] : empty[index];//اگر ماشینی داخلش بود یا نبود اسپرایت متفاوت است
    }

}
