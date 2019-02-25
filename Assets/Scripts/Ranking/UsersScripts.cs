using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsersScripts : MonoBehaviour
{
    public GameObject panelWait, userRank, userRankMe, panelParentRanking;
    private List<GameObject> listRanking = new List<GameObject>();
    public UserRank myRank;
    public Sprite[] sprMedal;
    private int rankUser = 0;
    #region link
    private string strGetRankUser = "https://balootvas.ir/balootvas/TaxiKhati/rankUser.php";
    #endregion
    void Start()
    {
        if (listRanking.Count == 0 || Manager.GetCurrentTime() < Manager.GetActionTime("updateRank"))
        {
            Debug.Log("Start");
            GetRanking();
        }
        else
        {
            Timer.Schedule(this, 120f, () =>
            {
                Debug.Log("Schedule(thiss");
                GetRanking();
            });
        }
    }
    #region Get Rank User
    public void GetRanking()
    {//رتبه شخص را میگیرد
        StartCoroutine(IEGetRanking());
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
        wwwForm.AddField("id", PlayerPrefs.GetInt("userid", 1));
        WWW www = new WWW(strGetRankUser, wwwForm);
        yield return www;
        if (www.error == null)
        {
            if (www.isDone)
            {
                JsonData jsonBooks = JsonMapper.ToObject(ChangeToJson(www.text));
                rankUser = int.Parse(jsonBooks[0]["rank"].ToString());
                if (rankUser < 1000)
                {
                    PlayerPrefs.SetInt("mainAchiv5", 1);
                }
                else if (rankUser < 5000)
                {
                    PlayerPrefs.SetInt("mainAchiv4", 1);
                }
                Debug.Log(rankUser.ToString());
                myRank.txtCoin.text = jsonBooks[0]["coin"].ToString();
                myRank.txtName.text = PlayerPrefs.GetString("username", "تاکسی ران");
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
                        Debug.Log("YOUUUUU" + rankUser);
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
                        Debug.Log("YOUUUUU" + rankUser);
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
                Timer.Schedule(this, 120f, () =>
                {
                    Debug.Log("Schedule(thiss");
                    GetRanking();
                });
            }
        }
        else
        {
            Debug.LogError("error to connet internet");
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
}
