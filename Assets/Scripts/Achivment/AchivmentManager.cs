using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchivmentManager : MonoBehaviour
{
    public Controller controller;
    public string achivmentPrefs;
    public Achivment[] achivments;
    public GameObject prefabAchivment, panelParentAchivment, lblNew;
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
        CheckAchivments();
    }
    public void CheckAchivments()
    {
        lblNew.SetActive(false);
        for (int i = 0; i < achivments.Length; i++)
        {
            if (ObscuredPrefs.GetInt(achivmentPrefs + "Get" + i, 0) == 1)//geted
            { }
            else if (ObscuredPrefs.GetInt(achivmentPrefs + i, 0) >= achivments[i].max)//unlock
            {
                lblNew.SetActive(true);
            }
        }
    }
    public void OpenPanel()
    {
        lblNew.SetActive(false);
        for (int i = 0; i < listAchivments.Count; i++)
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
            achivmentPrefab.controller = controller;
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
}
[System.Serializable]
public class Achivment
{
    public int id;
    public string title;
    public int rewardGem;
    public int max;
}
