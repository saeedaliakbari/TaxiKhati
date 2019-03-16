using UnityEngine;
using System.Collections;
using System.IO;

namespace GlimGames
{
    public class GiftsWindow : MonoBehaviour
    {
        public delegate void errorOccured(string error);


        public GameObject myGameObject, downloadingGameObject, errorSectionGameObject, noGiftsGameObject, closeButtonGameObject;
        public GiftSection giftSection;
        public UnityEngine.UI.Text errorText;


        bool validRequest;
        void Start()
        {
            giftSection.OnCollected += giftSection_OnCollected;

        }

        void giftSection_OnCollected(int a, int b)
        {
            currentGiftIndex++;
            if (gifts != null && gifts.Length > currentGiftIndex && gifts[currentGiftIndex].isFull)
                giftSection.Activate(gifts[currentGiftIndex]);
            else
                Deactivate();
        }

        string code;
        public void Activate(string code)
        {
            myGameObject.SetActive(true);

            this.code = code;

            errorSectionGameObject.SetActive(false);
            noGiftsGameObject.SetActive(false);
            giftSection.Deactivate();

            downloadingGameObject.SetActive(true);
            closeButtonGameObject.SetActive(false);

            GetGifts(code, _GiftsDownloaded, _DownloadGiftsFailed);
            validRequest = true;
        }

        int currentGiftIndex;
        private void _GiftsDownloaded()
        {
            if (!validRequest)
                return;

            downloadingGameObject.SetActive(false);



            currentGiftIndex = 0;
            if (gifts != null && gifts.Length > currentGiftIndex && gifts[currentGiftIndex].isFull)
                giftSection.Activate(gifts[currentGiftIndex]);
            else
                noGiftsGameObject.SetActive(true);

            closeButtonGameObject.SetActive(true);
        }

        private void _DownloadGiftsFailed(string error)
        {
            if (!validRequest)
                return;

            errorText.text = GetErrorText(error);

            downloadingGameObject.SetActive(false);
            errorSectionGameObject.SetActive(true);
            closeButtonGameObject.SetActive(true);
        }

        public void Deactivate()
        {
            validRequest = false;
            myGameObject.SetActive(false);
        }

        public void RetryButton_Click()
        {
            Activate(code);
        }

        public void CloseButton_Click()
        {
            Deactivate();
        }





        #region Get Gifts Section

        GiftStruct[] gifts;

        public void GetGifts(string code, System.Action successMethod, System.Action<string> failMethod)
        {
            StartCoroutine(_getGifts(code, successMethod, failMethod));
        }

        IEnumerator _getGifts(string code, System.Action successMethod, System.Action<string> failMethod)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", code);


            //string url = ServerUrl + getGiftsUrl;
            string url = FetchGifts.Instance.webserviceUrl;
            WWW www = new WWW(url, form);

            yield return www;

            string data = "";
            if (string.IsNullOrEmpty(www.error))
            {
                data = www.text;
            }
            else
            {
                if (failMethod != null)
                    failMethod(www.error);

                yield break;
            }


            //Debug.Log(data);
            StringReader reader = new StringReader(data);
            string doneText = reader.ReadLine();
            if (doneText.Trim().ToLower().StartsWith("done"))
            {
                gifts = new GiftStruct[10];

                int giftIndex = 0;
                while (true)
                {
                    string cID = reader.ReadLine();
                    if (cID == null)
                        break;

                    int catID = int.Parse(cID);
                    int itemID = int.Parse(reader.ReadLine());

                    string text = "";
                    string temp = "";
                    do
                    {
                        temp = reader.ReadLine();
                        if (temp.StartsWith("---"))
                            break;

                        if (!string.IsNullOrEmpty(text))
                            text += System.Environment.NewLine;
                        text += temp;

                    } while (true);

                    if (giftIndex >= gifts.Length)
                        break;

                    gifts[giftIndex] = new GiftStruct(catID, itemID, text);
                    giftIndex++;

                }
            }

            if (successMethod != null)
                successMethod();

            reader.Close();
            www.Dispose();
            www = null;
        }



        public static string GetErrorText(string errorText)
        {
            errorText = errorText.ToLower();

            if (errorText.IndexOf("resolve") != -1)
                return FetchGifts.Instance.noInternetErrorMessage;

            if (errorText.IndexOf("couldn't connect") != -1 || errorText.IndexOf("not found") != -1 || errorText.IndexOf("notfound") != -1)
                return FetchGifts.Instance.serverNotAvailableErrorMessage;

            return FetchGifts.Instance.generalErrorMessage;
        }

        #endregion




    }

    public struct GiftStruct
    {
        public int CategoryID, ItemID;
        public string Reason;
        public bool isFull;

        public GiftStruct(int catID, int itemID, string reason, bool isFull = true)
        {
            this.CategoryID = catID;
            this.ItemID = itemID;
            this.Reason = reason;
            this.isFull = true;
        }
    }
}