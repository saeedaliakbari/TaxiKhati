using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchivmentManager : MonoBehaviour
{
    public string achivmentPrefs;
    public Achivment[] achivments;
    public GameObject prefabAchivment, panelParentAchivment;
    public Scrollbar srbPanelAchivment;
    public ScrollRect scrPanelAchivment;
    public Text txtGem;
    public int scrollSize;
    [HideInInspector]
    public List<GameObject> listAchivments, listAchvGet, listAchvDone, listAchv;
    [HideInInspector]
    public List<AchivmentPrefab> listAchivmentScripts;
    void Start()
    {
        OpenPanel();
    }
    public void OpenPanel()
    {
        CheckAllAchv();
        for (int i = 0; i < listAchivments.Count; i++)//آواتارهای قبلی را از پنل حذف می کند
        {
            Destroy(listAchivments[i]);
            Destroy(listAchivmentScripts[i]);
        }
        listAchivments.RemoveRange(0, listAchivments.Count);
        listAchv.RemoveRange(0, listAchv.Count);
        listAchvDone.RemoveRange(0, listAchvDone.Count);
        listAchvGet.RemoveRange(0, listAchvGet.Count);
        listAchivmentScripts.RemoveRange(0, listAchivmentScripts.Count);
        for (int i = 0; i < achivments.Length; i++)
        {
            GameObject achvObj = Instantiate(prefabAchivment);
            listAchivments.Add(achvObj);
            AchivmentPrefab achivmentPrefab = achvObj.GetComponent<AchivmentPrefab>();
            listAchivmentScripts.Add(achivmentPrefab);
            achivmentPrefab.txtGem = txtGem;
            achivmentPrefab.Setup(achivments[i], achivmentPrefs, listAchvGet, listAchvDone, listAchv);
        }
        for (int i = 0; i < listAchvDone.Count; i++)
        {
            listAchvDone[i].transform.SetParent(panelParentAchivment.transform);
            listAchvDone[i].transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < listAchv.Count; i++)
        {
            listAchv[i].transform.SetParent(panelParentAchivment.transform);
            listAchv[i].transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < listAchvGet.Count; i++)
        {
            listAchvGet[i].transform.SetParent(panelParentAchivment.transform);
            listAchvGet[i].transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < listAchivments.Count; i++)
        {
            listAchivments[i].transform.localPosition = new Vector3(listAchivments[i].transform.localPosition.x, listAchivments[i].transform.localPosition.y, 1);
        }
    }
    private void CheckAllAchv()
    {
        #region Achv 1
        if (PlayerPrefs.GetInt("DiziMaxLvlNumber", 1) >= 2)
        {
            PlayerPrefs.SetInt("achivmentMain1", 11);
        }
        #endregion
    }
}
[System.Serializable]
public class Achivment
{
    public int id;
    public string title;
    public int rewardGem;
    public int max;
}
