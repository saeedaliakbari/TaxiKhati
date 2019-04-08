using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsersScripts : MonoBehaviour
{
    public GameObject panelWait, userRank, userRankMe, panelParentRanking, btnRename;
    private List<GameObject> listRanking = new List<GameObject>();
    public UserRank myRank;
    public Controller controller;
    public Sprite[] sprMedal;
    private int rankUser = 0;
    #region link
    private string strGetRankUser = "https://balootvas.ir/balootvas/TaxiKhati/rankUser.php";
    private string strInsertUser = "https://balootvas.ir/balootvas/TaxiKhati/insertUser.php";
    private string strUpdateUser = "https://balootvas.ir/balootvas/TaxiKhati/updateUser.php";
    #endregion
    public void StartRanking(bool btnRank)
    {
        if (btnRank)
        {
            GetRanking();
            Timer.Schedule(this, 120f, () =>
            {
                //Debug.Log("Schedule(thiss");
                GetRanking();
            });
        }
        else {
            if (listRanking.Count == 0 || Manager.GetCurrentTime() > Manager.GetActionTime("updateRank"))
            {
                //Debug.Log(Manager.GetCurrentTime() + "< " + Manager.GetActionTime("updateRank"));
                //Debug.Log("count " + listRanking.Count);
                GetRanking();
            }
            else
            {
                double delay = Manager.GetActionTime("updateRank") - Manager.GetCurrentTime();
                //Debug.Log("Delay : " + delay);
                Timer.Schedule(this, (float)delay, () =>
                {
                    //Debug.Log("Schedule(thiss");
                    GetRanking();
                });
            }
        }
    }
    #region Get Rank User
    public void GetRanking()
    {//رتبه شخص را میگیرد
        panelWait.SetActive(true);
        if (ObscuredPrefs.GetInt("userid", 0) == 0)
        {
            if (ObscuredPrefs.GetString("username", "") == "")
            {
                ObscuredPrefs.SetString("username", "تاکسی ران " + Random.Range(100000, 999999));
            }
            StartCoroutine(IEInsertUser(true));
        }
        else {
            StartCoroutine(IEUpdateUser(true));
        }
    }
    IEnumerator IEGetRanking()
    {
        //panelWait.SetActive(true);
        for (int i = 0; i < listRanking.Count; i++)
        {
            Destroy(listRanking[i]);
        }
        listRanking.RemoveRange(0, listRanking.Count);
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", ObscuredPrefs.GetInt("userid", 0));
        WWW www = new WWW(strGetRankUser, wwwForm);
        yield return www;
        Debug.Log("www.error: " + www.error);
        if (www.error == null)
        {
            if (www.isDone)
            {
                JsonData jsonBooks = JsonMapper.ToObject(ChangeToJson(www.text));
                rankUser = int.Parse(jsonBooks[0]["rank"].ToString());
                if (rankUser < 1000)
                {
                    ObscuredPrefs.SetInt("mainAchiv5", 1);
                }
                if (rankUser < 5000)
                {
                    ObscuredPrefs.SetInt("mainAchiv4", 1);
                }
                controller.achivmentManager.CheckAchivments();
                //Debug.Log(rankUser.ToString());
                myRank.txtRank.text = rankUser.ToString();
                myRank.txtCoin.text = jsonBooks[0]["coin"].ToString();
                myRank.txtName.text = ObscuredPrefs.GetString("username", "تاکسی ران");
                myRank.objRename.SetActive(true);
                float length = myRank.txtName.text.Length * myRank.txtName.fontSize * 0.75f + 30;
                bool isLeft = true;
                if (length / 2 > 232)
                {
                    length = 232 * 2;
                }
                btnRename.transform.localPosition = new Vector3(isLeft ? -length / 2f : length / 2.5f, 5f);
                ////Debug.Log("length: " + length + "/ " + myRank.txtName.text.Length + "/ " + myRank.txtName.fontSize);
                ////Debug.Log(btnRename.transform.localPosition);
                if (rankUser < 4)
                {
                    myRank.txtRank.gameObject.SetActive(false);
                    myRank.imgMedal.gameObject.SetActive(true);
                    myRank.imgMedal.sprite = sprMedal[rankUser - 1];
                }
                else
                {
                    myRank.txtRank.gameObject.SetActive(true);
                    myRank.imgMedal.gameObject.SetActive(false);
                    myRank.txtRank.text = rankUser.ToString();
                }
                for (int i = 1; i < 4; i++)
                {
                    GameObject objUser;
                    if (i == rankUser)
                    {
                        //Debug.Log("YOUUUUU" + rankUser);
                        objUser = Instantiate(userRankMe);
                    }
                    else
                    {
                        objUser = Instantiate(userRank);
                    }
                    objUser.transform.SetParent(panelParentRanking.transform);
                    objUser.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    listRanking.Add(objUser);
                    UserRank scrUser = objUser.GetComponent<UserRank>();
                    scrUser.txtRank.gameObject.SetActive(false);
                    scrUser.imgMedal.gameObject.SetActive(true);
                    scrUser.imgMedal.sprite = sprMedal[i - 1];
                    scrUser.txtName.text = jsonBooks[i][1].ToString();
                    scrUser.txtCoin.text = jsonBooks[i][2].ToString();

                }
                int count = jsonBooks.Count;
                for (int i = 4; i < count; i++)
                {
                    GameObject objUser;
                    if (i == rankUser)
                    {
                        //Debug.Log("YOUUUUU" + rankUser);
                        objUser = Instantiate(userRankMe);
                    }
                    else
                    {
                        objUser = Instantiate(userRank);
                    }
                    objUser.transform.SetParent(panelParentRanking.transform);
                    objUser.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    listRanking.Add(objUser);
                    UserRank scrUser = objUser.GetComponent<UserRank>();
                    scrUser.txtRank.text = i.ToString();
                    scrUser.txtName.text = jsonBooks[i][1].ToString();
                    scrUser.txtCoin.text = jsonBooks[i][2].ToString();
                }
                Manager.SetActionTime("updateRank", Manager.GetCurrentTime() + 120f);
                panelWait.SetActive(false);
                Timer.Schedule(this, 120f, () =>
                {
                    //Debug.Log("Schedule(thiss");
                    StartCoroutine(IEGetRanking());
                });
            }
        }
        else
        {
            //Debug.LogError("error to connet internet");
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "اتصال به اینترنت برقرار نشد بعدا تلاش نمایید";
            gameObject.SetActive(false);
            panelWait.SetActive(false);
            yield break;
        }
    }
    #endregion
    private string ChangeToJson(string input)
    {
        string output = input.Replace("]\"", ",");
        output = output.Replace("\"[", "");
        output = output.Replace("][", ",");
        output = output.Replace("[", "");
        output = output.Replace("]", "");
        output = output.Replace("\\", "");
        output = "[" + output + "]";
        return output;
    }

    public void SetName()
    {
        if (ObscuredPrefs.GetInt("userid", 0) == 0)
        {
            StartCoroutine(IEInsertUser(false));
        }
        else
        {
            StartCoroutine(IEUpdateUser(false));
        }
    }
    IEnumerator IEInsertUser(bool GetRank)
    {
        //panelWait.SetActive(true);
        //Debug.Log("IEInsertUser");
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("username", ObscuredPrefs.GetString("username", "").ToString());
        wwwForm.AddField("coin", ObscuredPrefs.GetDouble("coinTotal", 100).ToString());
        WWW www = new WWW(strInsertUser, wwwForm);
        yield return www;
        if (www.error == null)
        {
            if (www.isDone)
            {
                JsonData jsonBooks = JsonMapper.ToObject(ChangeToJson(www.text));
                ObscuredPrefs.SetInt("userid", int.Parse(jsonBooks[0][0].ToString()));
                if (GetRank)
                {
                    StartCoroutine(IEGetRanking());
                }
            }
        }
        else
        {
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "اتصال به اینترنت برقرار نشد بعدا تلاش نمایید";
            gameObject.SetActive(false);
            panelWait.SetActive(false);
        }
    }
    IEnumerator IEUpdateUser(bool GetRank)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", ObscuredPrefs.GetInt("userid", 0).ToString());
        wwwForm.AddField("username", ObscuredPrefs.GetString("username", "").ToString());
        wwwForm.AddField("coin", ObscuredPrefs.GetDouble("coinTotal", 100).ToString());
        WWW www = new WWW(strUpdateUser, wwwForm);
        yield return www;
        if (www.error == null)
        {
            if (www.isDone)
            {
                //Debug.Log("Update www : " + www.text);
                JsonData jsonBooks = JsonMapper.ToObject(ChangeToJson(www.text));
                ObscuredPrefs.SetInt("userid", int.Parse(jsonBooks[0][0].ToString()));
                ObscuredPrefs.SetString("username", jsonBooks[0][1].ToString());
                ObscuredPrefs.SetDouble("coinTotal", double.Parse(jsonBooks[0][2].ToString()));
                if (GetRank)
                {
                    StartCoroutine(IEGetRanking());
                }
            }
        }
        else
        {
            controller.panelMessage.SetActive(true);
            controller.txtPanelMessage.text = "اتصال به اینترنت برقرار نشد بعدا تلاش نمایید";
            gameObject.SetActive(false);
            panelWait.SetActive(false);
        }
    }
}