using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InvitedScript : MonoBehaviour
{
    public Image imgSlider, imgReward;
    public Text inviteCode, txtMessage, txtGem, txtMessageInviteReward, txtMessageReward;
    public GameObject btnInvite, panelInvite, panelMessage, panelMessageReward, panelInviteReward, objCopy, panelWait;
    public inviteReward[] InviteReward;
    private int countNotReceived;
    private CountInvitedStatus countInvitedStatus;
    #region urls
    private const string url = "https://balootvas.ir/balootvas/TaxiKhati/storage.php?f=";
    private const string urlInvitedCodeNew = url + "getInvitedCodeNew";
    private const string urlSetInvitedCode = url + "setInvitedCode";
    private const string urlGetCountInvitedStatus = url + "getCountInvitedStatus";
    private const string urlChangeInvitedStatus = url + "changeInvitedStatus";
    #endregion
    void Start()
    {
        if (ObscuredPrefs.GetString("inviteCode", "") == "")
        {
            StartCoroutine(IEGetInvitedCode());
        }
        else
        {
            inviteCode.text = "0" + invite(ObscuredPrefs.GetString("inviteCode", ""));
            btnInvite.SetActive(true);//دکمه نقشه بازی
            if (ObscuredPrefs.GetInt("countReward", 0) > 0)
            {
                StartCoroutine(IECangeInvitedStatus(ObscuredPrefs.GetInt("countReward", 0)));
            }
            else
            {
                StartCoroutine(IEGetCountInvitedStatus());
            }
            if (ObscuredPrefs.GetInt("enterFromSplash", 0) == 1)//اگر از اسپلش وارد شد
            {
                ObscuredPrefs.SetInt("enterFromSplash", 0);
                //Debug.Log("showInvitePanelCount:> " + ObscuredPrefs.GetInt("showInvitePanelCount", 0));
                if (ObscuredPrefs.GetInt("showInvitePanelCount", 0) % 2 == 0)//اگر دفعات زوج بود براش پنل جایزه های دعوت از دوستان را نمایش بده
                {
                    GetCountInvitedStatus();
                    if (ObscuredPrefs.GetInt("showInvitePanelCount", 0) > 100)
                    {
                        ObscuredPrefs.SetInt("showInvitePanelCount", 0);
                    }
                }
            }
        }
    }
    public void GetCountInvitedStatus()
    {
        Debug.Log("GetCountInvitedStatus");
        int index = ObscuredPrefs.GetInt("countRewardReceived", 0);
        if (index >= 6)
        {
            index = 5;
            imgSlider.fillAmount = InviteReward[index].fillAmount;
            txtMessageInviteReward.text = "جایزه دعوت از نفر بعدی: " + InviteReward[index].txtReward;
            //txtMessageInviteReward.text = string.Format(strTemplateNext5, );
            imgReward.sprite = InviteReward[index].spriteReward;
            imgReward.SetNativeSize();
        }
        else
        {
            imgSlider.fillAmount = InviteReward[index].fillAmount;
            txtMessageInviteReward.text = "جایزه دعوت از " + InviteReward[index].txtFriends + " نفر: " + InviteReward[index].txtReward;//string.Format(strTemplate, , );
            imgReward.sprite = InviteReward[index].spriteReward;
            imgReward.SetNativeSize();
        }
        if (ObscuredPrefs.GetString("inviteCode", "") != "")
        {
            if (ObscuredPrefs.GetInt("helpStep", 0) >= 22)
            {
                panelInviteReward.SetActive(true);
            }
        }
        if (ObscuredPrefs.GetInt("countReward", 0) > 0)
        {
            StartCoroutine(IECangeInvitedStatus(ObscuredPrefs.GetInt("countReward", 0)));
        }
        else
        {
            StartCoroutine(IEGetCountInvitedStatus());
        }
    }

    public IEnumerator IEGetInvitedCode()
    {
        var postData = new WWWForm();
        Debug.Log("IEGetInvitedCode : " + SystemInfo.deviceUniqueIdentifier);
        postData.AddField("uniqcode", SystemInfo.deviceUniqueIdentifier);
        WWW www = new WWW(urlInvitedCodeNew, postData);
        yield return www;
        Debug.Log("IEGetInvitedCode : " + www.text + "/" + www.error);
        if (www.error == null)
        {
            if (www.text.Trim() != "4")// درصورتی که کد تخصیص داده باشد
            {
                inviteCode.text = "0" + invite(www.text.Trim());
                ObscuredPrefs.SetString("inviteCode", www.text.Trim());
                btnInvite.SetActive(true);
            }
        }
    }

    public IEnumerator IESetInvitedCode(string invitedCode)
    {
        panelWait.SetActive(true);
        var postData = new WWWForm();
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        postData.AddField("uniqcode", SystemInfo.deviceUniqueIdentifier);
        postData.AddField("invitedCode", invitedCode.Substring(1, 10));
        StartCoroutine(IECloseLoadingPanel(panelWait));
        WWW www = new WWW(urlSetInvitedCode, postData);
        yield return www;
        Debug.Log("IESetInvitedCode : " + www.text + "/" + www.error);
        panelWait.SetActive(false);
        if (www.error == null)
        {
            if (www.text.Trim() == "1")
            {
                panelInvite.SetActive(false);
                ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 5) + 100);
                ShowMessage(panelMessage, "شما برنده 100 الماس شديد");
                txtGem.text = ObscuredPrefs.GetDouble("gem", 5).ToString();
                ObscuredPrefs.SetInt("cancelInvite", 1);
            }
            else if (www.text.Trim() == "3")
            {
                ShowMessage(panelMessage, "کد وارد شده صحيح نمي باشد");
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
        if (invitedCode.text.Length == 11 && invitedCode.text.Substring(0, 3) == "098")
        {
            StartCoroutine(IESetInvitedCode(invitedCode.text));
        }
        else
        {
            ShowMessage(panelMessage, "لطفا کد را به درستي وارد کنيد");
        }
    }

    public void CopyToCipBoard()
    {
        UniClipboard.SetText("0" + ObscuredPrefs.GetString("inviteCode", ""));
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
        ObscuredPrefs.SetInt("cancelInvite", 1);
        panel.SetActive(false);
    }

    public void ShowMessage(GameObject gameObject, string message)
    {
        txtMessage.text = message;
        gameObject.SetActive(true);
    }

    public IEnumerator IEGetCountInvitedStatus()
    {
        var postData = new WWWForm();
        postData.AddField("inviteCode", ObscuredPrefs.GetString("inviteCode", ""));
        WWW www = new WWW(urlGetCountInvitedStatus, postData);
        yield return www;
        //Debug.Log("IEGetCountInvitedStatus : " + www.text + "/" + www.error);
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

    public IEnumerator IECangeInvitedStatus(int count)
    {
        Debug.Log("inviteCode:" + ObscuredPrefs.GetString("inviteCode", ""));
        var postData = new WWWForm();
        postData.AddField("inviteCode", ObscuredPrefs.GetString("inviteCode", ""));
        postData.AddField("count", count);
        WWW www = new WWW(urlChangeInvitedStatus, postData);
        yield return www;
        Debug.Log("IECangeInvitedStatus : " + www.text + "/" + www.error);
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
        Debug.Log("CheckCountReward");
        int index = ObscuredPrefs.GetInt("countRewardReceived", 0);
        if (index >= 6)
        {
            index = 5;
            txtMessageInviteReward.text = "جایزه دعوت از نفر بعدی: " + InviteReward[index].txtReward;
            //txtMessageInviteReward.text = string.Format(strTemplateNext5, InviteReward[index].txtReward);
        }
        if (countNotReceived > 0)
        {
            int plus = 1;
            if (index == 5)
                plus = 0;
            Debug.Log("index + plus :" + index + plus);
            txtMessageInviteReward.text = "جایزه دعوت از " + InviteReward[index + plus].txtFriends + " نفر: " + InviteReward[index + plus].txtReward;
            //txtMessageInviteReward.text = string.Format(strTemplate, InviteReward[index + plus].txtFriends, InviteReward[index + plus].txtReward);
            imgReward.sprite = InviteReward[index + plus].spriteReward;
            imgReward.SetNativeSize();
            imgSlider.fillAmount = InviteReward[index + plus].fillAmount;
            if (ObscuredPrefs.GetString("inviteCode", "") != "")
            {
                if (ObscuredPrefs.GetInt("helpStep", 0) >= 22)
                {
                    panelInviteReward.SetActive(true);
                }
            }
            if (ObscuredPrefs.GetInt("countRewardReceived", 0) >= 6)// تعداد نفراتی که جایزه را دریافت کردند
                txtMessageReward.text = "شما برنده" + InviteReward[5].txtRewardMessage + " شدید"; //string.Format(strTemplateMessageInviteReward, ObscuredPrefs.GetInt("countRewardReceived", 0), );
            else
            {
                txtMessageReward.text = "شما برنده" + InviteReward[index].txtRewardMessage + " شدید"; //string.Format(strTemplateMessageInviteReward, InviteReward[index].txtFriends, InviteReward[index].txtRewardMessage);
            }
            StartCoroutine(IEOpenPanelDelayed(panelMessageReward));
        }
    }

    public IEnumerator IEOpenPanelDelayed(GameObject panel)
    {
        yield return new WaitForSeconds(1.9f);
        panel.SetActive(true);
    }

    public void subtractCountReward()//کلید حله پنل دریافت جایزه
    {
        ObscuredPrefs.SetInt("countRewardReceived", ObscuredPrefs.GetInt("countRewardReceived", 0) + 1);
        int countReceived = ObscuredPrefs.GetInt("countRewardReceived", 0);
        if (countReceived >= 6)
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 0) + InviteReward[5].gem);
        }
        else
        {
            ObscuredPrefs.SetDouble("gem", ObscuredPrefs.GetDouble("gem", 0) + InviteReward[countReceived - 1].gem);
        }
        txtGem.text = ObscuredPrefs.GetDouble("gem", 0).ToString();
        //txtCoin.text = ArabicFixer.Fix(ObscuredPrefs.GetInt("CoinTotal", 400).ToString(), false, true);
        if (countNotReceived > 0)
        {
            countNotReceived--;
            ObscuredPrefs.SetInt("countReward", ObscuredPrefs.GetInt("countReward", 0) + 1);
        }
        if (countNotReceived == 0)
        {
            StartCoroutine(IECangeInvitedStatus(ObscuredPrefs.GetInt("countReward", 0)));
        }
        else
        {
            CheckCountReward();
        }
    }

    private IEnumerator copyToast()
    {
        objCopy.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        objCopy.SetActive(false);
    }

    [System.Serializable]
    public class inviteReward
    {
        public string txtReward, txtRewardMessage, txtFriends;
        public Sprite spriteReward;
        public float fillAmount;
        public int gem;
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