using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GlimGames
{
    public class GiftSection : MonoBehaviour
    {
        public delegate void giftCollected(int categoryIndex, int item);
        public event giftCollected OnCollected;


        public GameObject myGameObject;
        public Image frameImage, itemImage;
        public Text itemNameText, reasonText;
        public int lineLength;
        



        string reason;
        int catID, itemID;

        public void Activate(GiftStruct g)
        {
            Activate(g.CategoryID, g.ItemID, g.Reason);
        }

        public void Activate(int catID, int itemID, string reason)
        {
            this.reason = reason;
            this.catID = catID;
            this.itemID = itemID;


            for (int i = 0; i < FetchGifts.Instance.categories.Length; i++)
            {
                if (FetchGifts.Instance.categories[i].categoryIndex == catID)
                {
                    itemImage.sprite = FetchGifts.Instance.categories[i].icon;
                    itemNameText.text = string.Format(FetchGifts.Instance.categories[i].textFormat, itemID);
                    break;
                }
            }

            reasonText.text = RTLService.RTL.GetText(reason, RTLService.RTL.NumberFormat.ArabicFormat, false, lineLength);


            /*
            if (catID == -1)
            {
                itemImage.sprite = coinsSprite;
                itemNameText.text = string.Format(coinsTextFormat, string.Format("{0:#,###0}", itemID));
                itemImage.SetNativeSize();
            }
            else if (catID == -2)
            {
                itemImage.sprite = noAdSprite;
                itemNameText.text = noAdText;
                itemImage.SetNativeSize();
            }
            else if (catID == 9)
            {
                //itemImage.sprite = MainStatic.instance.storeDB.drivers[3].picture;
                itemNameText.text = driver4String;
            }*/

            myGameObject.SetActive(true);
        }

        public void CollectButton_Click()
        {
            if (OnCollected != null)
                OnCollected(catID, itemID);

            Deactivate();
        }

        public void Deactivate()
        {
            myGameObject.SetActive(false);
        }
    }
}