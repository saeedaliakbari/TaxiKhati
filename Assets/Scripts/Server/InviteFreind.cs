using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class InvitedScript : MonoBehaviour
{
    //public BarScript bar;
    public Text txtInviteCode, txtMessage, txtTitle, gemTxt, coinTxt, txtMessageInviteReward, txtMessagePanelMessageInviteReward;
    public SpriteRenderer imgReward;
    public GameObject btnInvite, PanelWarningInvate, PanelInvate, panelMessage, panelInviteReward, PanelMessageInviteReward, imageCopy, PanelLoading, panelHelp;
    public CountInvitedStatus countInvitedStatus;
    public string txtTemplate, txtTemplateMessageInviteReward, txtTemplateNext5;
    public inviteReward InviteReward;
    private int countNotReceived;
    #region urls
    private const string url = "https://balootvas.ir/balootvas/TaxiKhati/storage.php?f=";
    private const string urlGetInviteCode = url + "getInvitedCodeNew";
    private const string urlChangeStaus = url + "changeInvitedStatus";
    private const string urlGetCountInvite = url + "getCountInvitedStatus";
    #endregion
    void Start()
    {
        if (ObscuredPrefs.GetString("inviteCode", "") == "")//اگر کد دعوت نداشت کاربر براش کد دعوت در سرور تعریف شود
        {
            StartCoroutine(IEGetInvitedCode());
        }
        else
        {
            txtInviteCode.text = "0" + invite(ObscuredPrefs.GetString("inviteCode", ""));
            btnInvite.SetActive(true);//دکمه نقشه بازی
            if (ObscuredPrefs.GetInt("countReward", 0) > 0)//تعداد افرادی که تا حالا دعوت کرده
            {
                StartCoroutine(IEChangeInvitedStatus(ObscuredPrefs.GetInt("countReward", 0)));
            }
            else
            {
                StartCoroutine(IEGetCountInvitedStatus());
            }

            if (PlayerPrefs.GetInt("EnterFromSplash", 0) == 1)
            {
                PlayerPrefs.SetInt("EnterFromSplash", 0);

                if (PlayerPrefs.GetInt("showInvitePanelCount", 0) % 2 == 0)
                {
                    GetCountInvitedStatus();

                    if (PlayerPrefs.GetInt("showInvitePanelCount", 0) > 100)
                    {
                        PlayerPrefs.SetInt("showInvitePanelCount", 0);
                    }
                }
            }
        }
    }

    public void GetCountInvitedStatus()
    {
        int index = PlayerPrefs.GetInt("countRewardReceived", 0);

        if (index >= 6)
        {
            index = 5;
            //bar.fillAmount = InviteReward.fillAmount[index];
            txtMessageInviteReward.text = string.Format(txtTemplateNext5, InviteReward.txtReward[index]);
            imgReward.sprite = InviteReward.spriteReward[index];
        }
        else
        {
            //bar.fillAmount = InviteReward.fillAmount[index];
            txtMessageInviteReward.text = string.Format(txtTemplate, InviteReward.txtFriends[index], InviteReward.txtReward[index]);
            imgReward.sprite = InviteReward.spriteReward[index];
        }


        if (PlayerPrefs.GetString("inviteCode", "") != "")
        {
            panelInviteReward.SetActive(true);
        }



        if (PlayerPrefs.GetInt("countReward", 0) > 0)
        {
            StartCoroutine(IEChangeInvitedStatus(PlayerPrefs.GetInt("countReward", 0)));
        }
        else
        {
            StartCoroutine(IEGetCountInvitedStatus());
        }

    }

    public IEnumerator IEGetInvitedCode()//کد دعوت فرد را ایجاد می کند یا اینکه از سرور میگیرد
    {
        var postData = new WWWForm();
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        postData.AddField("uniqcode", SystemInfo.deviceUniqueIdentifier);
        WWW www = new WWW(url, postData);
        yield return www;
        if (www.error == null)
        {
            if (www.text.Trim() != "4")// درصورتی که کد تخصیص داده باشد
            {
                txtInviteCode.text = "0" + invite(www.text.Trim());
                PlayerPrefs.SetString("inviteCode", www.text.Trim());
                btnInvite.SetActive(true);
            }
        }
    }

    public IEnumerator IESetInvitedCode(string invitedCode)
    {
        PanelLoading.SetActive(true);

        var postData = new WWWForm();
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        postData.AddField("uniqcode", SystemInfo.deviceUniqueIdentifier);
        postData.AddField("invitedCode", invitedCode.Substring(1, 10));
        //var url = com.getUrl("setInvitedCode");
        StartCoroutine(IECloseLoadingPanel(PanelLoading));

        WWW www = new WWW(url, postData);
        yield return www;

        PanelLoading.SetActive(false);

        if (www.error == null)
        {
            if (www.text.Trim() == "1")
            {
                PanelInvate.gameObject.SetActive(false);
                PlayerPrefs.SetInt("GemTotal", PlayerPrefs.GetInt("GemTotal") + 100);
                ShowMessage(panelMessage, "تبريک", "شما برنده 100 الماس شديد");
                //gemTxt.text = ArabicFixer.Fix(PlayerPrefs.GetInt("GemTotal", 0).ToString(), false, true);
                PlayerPrefs.SetInt("cancelInvate", 1);
                panelHelp.SetActive(true);
            }
            else if (www.text.Trim() == "3")
            {
                ShowMessage(panelMessage, "توجه", "کد وارد شده صحيح نمي باشد");
            }
        }
    }

    IEnumerator IECloseLoadingPanel(GameObject panel)
    {
        yield return new WaitForSeconds(7f);
        panel.SetActive(false);
    }

    public void SetInvitedCode(InputField invitedCode)
    {


        if (invitedCode.text.Length == 0)
        {
            invitedCode.Select();
            invitedCode.ActivateInputField();
            return;
        }
        Debug.Log(invitedCode.text.Substring(0, 3));
        if (invitedCode.text.Length == 11 && invitedCode.text.Substring(0, 3) == "095")
        {
            StartCoroutine(IESetInvitedCode(invitedCode.text));
        }
        else
        {
            ShowMessage(panelMessage, "توجه", "لطفا کد را به درستي وارد کنيد");
        }
    }

    public void CopyToCipBoard()
    {
        UniClipboard.SetText("0" + PlayerPrefs.GetString("inviteCode", ""));
        UniClipboard.GetText();
        StartCoroutine(copyToast());
    }

    public string invite(string result)
    {
        var str1 = result.Substring(0, 3);
        var str2 = result.Substring(3, 3);
        var str3 = result.Substring(6, 4);

        return string.Format("{0}  {1}  {2}", str1, str2, str3);
    }

    public void CancelInvate(GameObject panel)
    {
        PlayerPrefs.SetInt("cancelInvate", 1);
        panel.SetActive(false);

    }

    public void ShowMessage(GameObject gameObject, string title, string message)
    {
        //txtTitle.text = ArabicFixer.Fix(title, false, true);
        //txtMessage.text = ArabicFixer.Fix(message, false, true);
        gameObject.SetActive(true);
    }

    public IEnumerator IEGetCountInvitedStatus()
    {
        var postData = new WWWForm();
        postData.AddField("inviteCode", PlayerPrefs.GetString("inviteCode", ""));
        WWW www = new WWW(urlGetCountInvite, postData);
        yield return www;
        if (www.error == null)
        {
            if (www.text.Trim() != "")
            {
                var res = www.text.Trim().Replace("]", "").Replace("[", "");
                countInvitedStatus = new CountInvitedStatus();
                countInvitedStatus = JsonMapper.ToObject<CountInvitedStatus>(res);
                int countRewardNotReceived = int.Parse(countInvitedStatus.countRewardNotReceived.count1);
                int countRewardReceived = int.Parse(countInvitedStatus.countRewardReceived.count1);
                ObscuredPrefs.SetInt("countRewardReceived", countRewardReceived);
                if (countRewardNotReceived > 0)
                {
                    countNotReceived = countRewardNotReceived;
                    CheckCountReward();
                }
            }
        }
    }
    public IEnumerator IEChangeInvitedStatus(int count)
    {
        Debug.Log("IEChangeInvitedStatus inviteCode:" + ObscuredPrefs.GetString("inviteCode", ""));
        var postData = new WWWForm();
        postData.AddField("inviteCode", ObscuredPrefs.GetString("inviteCode", ""));
        postData.AddField("count", count);
        WWW www = new WWW(urlChangeStaus, postData);
        yield return www;
        if (www.error == null)
        {
            var res = www.text.Trim();
            Debug.Log(res + "==" + count.ToString());
            if (res == count.ToString())
            {
                Debug.Log("IECangeInvitedStatus ok>");
                ObscuredPrefs.SetInt("countReward", 0);
            }
        }
    }

    public void CheckCountReward()
    {
        int index = ObscuredPrefs.GetInt("countRewardReceived", 0);
        if (index >= 6)
        {
            index = 5;
            txtMessageInviteReward.text = string.Format(txtTemplateNext5, InviteReward.txtReward[index]);
        }
        if (countNotReceived > 0)
        {
            int plus = 1;
            if (index == 5)
                plus = 0;
            Debug.Log("index + plus :" + index + plus);
            txtMessageInviteReward.text = string.Format(txtTemplate, InviteReward.txtFriends[index + plus], InviteReward.txtReward[index + plus]);
            imgReward.sprite = InviteReward.spriteReward[index + plus];
            //bar.fillAmount = InviteReward.fillAmount[index + plus];

            if (ObscuredPrefs.GetString("inviteCode", "") != "")
            {
                panelInviteReward.SetActive(true);
            }

            if (ObscuredPrefs.GetInt("countRewardReceived", 0) >= 6)// تعداد نفراتی که جایزه را دریافت کردند
                txtMessagePanelMessageInviteReward.text = string.Format(txtTemplateMessageInviteReward, PlayerPrefs.GetInt("countRewardReceived", 0), InviteReward.txtRewardMessage[5]);
            else
            {
                txtMessagePanelMessageInviteReward.text = string.Format(txtTemplateMessageInviteReward, InviteReward.txtFriends[index], InviteReward.txtRewardMessage[index]);
            }

            StartCoroutine(IEOpenPanelDelayed(PanelMessageInviteReward));
        }
    }

    public IEnumerator IEOpenPanelDelayed(GameObject panel)
    {

        yield return new WaitForSeconds(1.9f);
        panel.SetActive(true);
    }

    public void subtractCountReward()
    {
        PlayerPrefs.SetInt("countRewardReceived", PlayerPrefs.GetInt("countRewardReceived", 0) + 1);

        int countReceived = PlayerPrefs.GetInt("countRewardReceived", 0);

        if (countReceived >= 6)
        {
            PlayerPrefs.SetInt("GemTotal", PlayerPrefs.GetInt("GemTotal", 0) + InviteReward.gem[5]);
            PlayerPrefs.SetInt("CoinTotal", PlayerPrefs.GetInt("CoinTotal", 0) + InviteReward.coin[5]);
        }
        else
        {
            PlayerPrefs.SetInt("GemTotal", PlayerPrefs.GetInt("GemTotal", 0) + InviteReward.gem[countReceived - 1]);
            PlayerPrefs.SetInt("CoinTotal", PlayerPrefs.GetInt("CoinTotal", 0) + InviteReward.coin[countReceived - 1]);
        }

        //gemTxt.text = ArabicFixer.Fix(PlayerPrefs.GetInt("GemTotal", 0).ToString(), false, true);
        //coinTxt.text = ArabicFixer.Fix(PlayerPrefs.GetInt("CoinTotal", 400).ToString(), false, true);


        if (countNotReceived > 0)
        {
            countNotReceived--;
            PlayerPrefs.SetInt("countReward", PlayerPrefs.GetInt("countReward", 0) + 1);
        }

        if (countNotReceived == 0)
        {
            StartCoroutine(IEChangeInvitedStatus(PlayerPrefs.GetInt("countReward", 0)));
        }
        else
        {
            CheckCountReward();
        }


    }

    private IEnumerator copyToast()
    {
        imageCopy.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        imageCopy.SetActive(false);
    }

    [System.Serializable]
    public class inviteReward
    {
        public string[] txtReward, txtRewardMessage, txtFriends;
        public Sprite[] spriteReward;
        public float[] fillAmount;
        public int[] gem;
        public int[] coin;

    }
}

public class CountInvitedStatus
{
    public countRewardReceived countRewardReceived { get; set; }
    public countRewardNotReceived countRewardNotReceived { get; set; }
}

public class countRewardReceived
{
    public string count1 { get; set; }
}

public class countRewardNotReceived
{
    public string count1 { get; set; }
}