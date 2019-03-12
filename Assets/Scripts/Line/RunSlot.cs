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
        spriteRun.color = new Color(1, 1, 1, 1);
        spriteRun.sprite = hasCar ? full[index] : empty[index];//اگر ماشینی داخلش بود یا نبود اسپرایت متفاوت است

    }

    public void changeSprite(bool fullSprite, bool fill)
    {
        int index = 0;
        if (fullSprite)
        {
            if (fill)
            {
                spriteRun.sprite = full[index];
                spriteRun.color = new Color(1, 1, 1, 1);
            }
            else
            {
                spriteRun.sprite = full[index];
                spriteRun.color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {

            spriteRun.sprite = empty[index];
        }
    }

}
