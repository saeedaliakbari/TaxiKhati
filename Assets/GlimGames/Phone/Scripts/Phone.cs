using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GlimGames
{
    public class Phone : MonoBehaviour
    {
        public GameObject myGameObject;
        public Text screenText;
        public AudioSource dialButtonSoundEffect;
        public GiftsWindow giftsWindow;

        public GameObject pasteButtonGameObject;



        public static void Activate()
        {
            FetchGifts.Instance.Activate();
        }

        public static event GiftSection.giftCollected OnCollected;

        public static void FireEvent(int categoryIndex, int item)
        {
            if (OnCollected != null)
                OnCollected(categoryIndex, item);
        }


        void Start()
        {
            screenText.text = "";
            pasteButtonGameObject.SetActive(false);
        }

        public void PasteButton_Click()
        {
            pasteButtonGameObject.SetActive(false);

            string clipboard = UniClipboard.GetText();
            string editedClipboard = "";
            for (int i = 0; i < clipboard.Length; i++)
            {
                char ch = clipboard[i];
                if (char.IsDigit(ch) || ch == '*' || ch == '#')
                    editedClipboard += ch;
            }

            screenText.text += editedClipboard;
        }

        public void CloseButton_Click()
        {
            Deactivate();
        }

        public void ActivatePhone()
        {
            giftsWindow.Deactivate();
            myGameObject.SetActive(true);
        }

        public void Deactivate()
        {
            myGameObject.SetActive(false);
        }

        public void DialButton_Click()
        {
            giftsWindow.Activate(screenText.text);
        }

        public void ResetButton_Click()
        {
            screenText.text = "";
            dialButtonSoundEffect.Play();
        }

        public void BackSpaceButton_Click()
        {
            if (screenText.text.Length > 0)
            {
                screenText.text = screenText.text.Remove(screenText.text.Length - 1);
                dialButtonSoundEffect.Play();
            }
        }

        public void KeypadButton_Click(string character)
        {
            if (screenText.text.Length < 20)
            {
                screenText.text += character;
                _PlaySound(character);
            }
            else
                Debug.Log("screen is full");
        }

        void _PlaySound(string character)
        {
            int value = 0;
            if (character == "*" || character == "#")
                value = 0;
            else
                value = int.Parse(character);

            dialButtonSoundEffect.pitch = 0.85f + 0.015f * value;
            dialButtonSoundEffect.Play();
        }

        float touchTime;
        bool isTouching;
        public void ScreenText_Touched()
        {
            isTouching = true;
            touchTime = Time.time;
        }

        public void ScreenText_Detouched()
        {
            isTouching = false;
        }
        void Update()
        {
            if (isTouching)
            {
                if (Time.time - touchTime >= 0.75f)
                {
                    isTouching = false;
                    pasteButtonGameObject.SetActive(true);
                }
            }
        }
    }

    

    [System.Serializable]
    public class RewardCategory
    {
        public int categoryIndex;
        public Sprite icon;
        public string textFormat;
    }
}