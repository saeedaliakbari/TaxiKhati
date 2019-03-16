using UnityEngine;
using System.Collections;

namespace GlimGames
{
    public class FetchGifts : MonoBehaviour
    {
        #region Singleton

        static FetchGifts _instance;
        public static FetchGifts Instance
        {
            get 
            { return FetchGifts._instance; }
        }

        void Awake()
        {
            _instance = this;
            phone.Deactivate();
        }

        void OnDestroy()
        {
            _instance = null;
        }

        #endregion


        public static event GiftSection.giftCollected OnCollected;


        public string webserviceUrl;
        public RewardCategory[] categories;
        public string noInternetErrorMessage, serverNotAvailableErrorMessage, generalErrorMessage;


        public Phone phone;


        bool initialized;

        public void Activate()
        {
            if (!initialized)
            {
                phone.giftsWindow.giftSection.OnCollected += giftSection_OnCollected;
                initialized = true;
            }

            phone.ActivatePhone();   
        }

        void giftSection_OnCollected(int categoryIndex, int item)
        {
            Phone.FireEvent(categoryIndex, item);
            if (OnCollected != null)
                OnCollected(categoryIndex, item);
        }
    }
}