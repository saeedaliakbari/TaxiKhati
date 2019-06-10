using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ShareScript : MonoBehaviour
{
    public Sprite sprIcon;
    private bool isFocus = false;
    private string shareSubject, shareMessage;
    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }
    public void OnShareButtonClick()
    {
        shareSubject = "تاکسی خطی رو نصب کن";
        shareMessage = "تاکسی خطی رو نصب کن \n https://cafebazaar.ir/app/ir.balootgames.taxi/ \n" +
            "کد زیر رو وارد کن " +
        "\n 100 تا الماس جایزه بگیر \n" +
        "دوستای دیگتم دعوت کن \n" +
        "0" + invite(PlayerPrefs.GetString("inviteCode", ""));
        StartCoroutine(TakeSSAndShare());
    }
    private IEnumerator TakeSSAndShare()
    {
        yield return new WaitForEndOfFrame();
        //var croppedTexture = new Texture2D((int)sprIcon.rect.width, (int)sprIcon.rect.height);
        //var pixels = sprIcon.texture.GetPixels((int)sprIcon.textureRect.x,
        //                                        (int)sprIcon.textureRect.y,
        //                                        (int)sprIcon.textureRect.width,
        //                                        (int)sprIcon.textureRect.height);
        //croppedTexture.SetPixels(pixels);
        //croppedTexture.Apply();
        //string filePath1 = Path.Combine(Application.temporaryCachePath, "img1.png");
        //File.WriteAllBytes(filePath1, croppedTexture.EncodeToPNG());
        //Destroy(croppedTexture);
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        string filePath = Path.Combine(Application.temporaryCachePath, "sharedimg.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        // To avoid memory leaks
        Destroy(ss);
        new NativeShare().AddFile(filePath).SetSubject(shareSubject).SetText(shareMessage).Share();
    }

    public string invite(string result)
    {
        var res = "";
        if (result.Length > 1)
        {
            var str1 = result.Substring(0, 3);
            var str2 = result.Substring(3, 3);
            var str3 = result.Substring(6, 4);
            res = string.Format("{0}  {1}  {2}", str1, str2, str3);
        }
        return res;
    }
}