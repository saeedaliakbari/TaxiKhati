using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsersScripts : MonoBehaviour
{
    public GameObject panelWait, userRank, panelParentRanking;
    private List<GameObject> listRanking = new List<GameObject>();
    public UserRank myRank;
    private int rankUser = 0;
    #region link
    private string strGetRankUser = "http://185.55.226.163/moshtary/TaxiKhati/rankUser.php";
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
                Debug.Log(rankUser.ToString());
                myRank.txtRank.text = rankUser.ToString();
                if (rankUser <= 100)//اگر رتبه کشوری کمتر از 50 بود
                {
                    myRank.gameObject.SetActive(false);
                }
                else
                {
                    myRank.gameObject.SetActive(true);
                }
                for (int i = 1; i < jsonBooks.Count; i++)
                {
                    GameObject objUser = Instantiate(userRank);
                    objUser.transform.SetParent(panelParentRanking.transform);
                    objUser.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    listRanking.Add(objUser);
                    UserRank scrUser = objUser.GetComponent<UserRank>();
                    scrUser.txtRank.text = i.ToString();
                    scrUser.txtName.text = jsonBooks[i][1].ToString();
                    scrUser.txtCoin.text = jsonBooks[i][2].ToString();
                    if (i == rankUser)
                    {
                        Debug.Log("YOUUUUU" + rankUser);
                    }
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
