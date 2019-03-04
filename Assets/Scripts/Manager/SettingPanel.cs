using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingPanel : MonoBehaviour
{
    public Controller controller;
    public Sprite sprRed, sprGreen;
    public Text txtBtnMusic, txtBtnSfx;
    public Image imgBtnMusic, imgBtnSfx;
    public AudioMixerSnapshot muteMusic, unmuteMusic, muteSfx, unmuteSfx;
    void Start()
    {
        if (PlayerPrefs.GetInt("MusicMute", 1) == 1)
        {
            unmuteMusic.TransitionTo(0.2f);
            txtBtnMusic.text = "فعال";
            imgBtnMusic.sprite = sprGreen;
        }
        else {
            muteMusic.TransitionTo(1f);
            txtBtnMusic.text = "غیرفعال";
            imgBtnMusic.sprite = sprRed;
        }

        if (PlayerPrefs.GetInt("SfxMute", 1) == 1)
        {
            unmuteSfx.TransitionTo(0.2f);
            txtBtnSfx.text = "فعال";
            imgBtnSfx.sprite = sprGreen;
        }
        else {
            muteSfx.TransitionTo(1f);
            txtBtnSfx.text = "غیرفعال";
            imgBtnSfx.sprite = sprRed;
        }
    }
    public void ConnectToSupport()
    {
        Application.OpenURL("tg://resolve?domain=baloot_game");
    }
    public void JoinInsta()
    {
        Application.OpenURL("instagram://user?username=taxi.khati");
        if (ObscuredPrefs.GetBool("join_insta", false) == false)
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 5) + 6);
            controller.SetText();
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "تبریک! 6 الماس اضافه شد";
            ObscuredPrefs.SetBool("join_insta", true);
        }
    }
    public void JoinTelegram()
    {
        Application.OpenURL("tg://resolve?domain=taxi_khati");
        if (ObscuredPrefs.GetBool("join_tele", false) == false)
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 5) + 4);
            controller.SetText();
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "تبریک! 4 الماس اضافه شد";
            ObscuredPrefs.SetBool("join_tele", true);
        }
    }

    public void ChangeMusic()
    {
        if (PlayerPrefs.GetInt("MusicMute", 1) == 1)
        {
            muteMusic.TransitionTo(1f);
            txtBtnMusic.text = "غیرفعال";
            imgBtnMusic.sprite = sprRed;
            PlayerPrefs.SetInt("MusicMute", 0);
        }
        else {
            unmuteMusic.TransitionTo(0.2f);
            txtBtnMusic.text = "فعال";
            imgBtnMusic.sprite = sprGreen;
            PlayerPrefs.SetInt("MusicMute", 1);
        }
    }

    public void ChangeSfx()
    {
        if (PlayerPrefs.GetInt("SfxMute", 1) == 1)
        {
            muteSfx.TransitionTo(1f);
            txtBtnSfx.text = "غیرفعال";
            imgBtnSfx.sprite = sprRed;
            PlayerPrefs.SetInt("SfxMute", 0);
        }
        else {
            unmuteSfx.TransitionTo(0.2f);
            txtBtnSfx.text = "فعال";
            imgBtnSfx.sprite = sprGreen;
            PlayerPrefs.SetInt("SfxMute", 1);
        }
    }
}
