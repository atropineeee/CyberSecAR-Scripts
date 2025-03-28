using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ARNewsMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected GameObject thisNewsContentLoc;
    [SerializeField] protected GameObject thisNewsContentPrefab;

    [Header("Contents")]
    [SerializeField] protected NewsSO NewsData;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisNewsContentLoc = this.thisParentObject.transform.Find("NewsCenter_Panel/News_PanelScrollRect/News_PanelScrollList").gameObject;
        this.thisNewsContentPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/News Panel/NewsContents");
        this.NewsData = Resources.Load<NewsSO>("! Scriptable Objects/News Data/NewsData");

        CreateContents();
    }

    private void CreateContents()
    {
        ResetList();

        foreach(var newsList in this.NewsData.NewsList)
        {
            GameObject create = Instantiate(this.thisNewsContentPrefab);
            create.transform.SetParent(this.thisNewsContentLoc.transform, false);

            TMP_Text title = create.transform.Find("NewsContentTitle").GetComponent<TMP_Text>();
            TMP_Text desc = create.transform.Find("NewsContentDescription").GetComponent<TMP_Text>();

            title.text = newsList.NewsTopic;
            desc.text = newsList.NewsDescription;
        }
    }

    private void ResetList()
    {
        foreach(Transform child in this.thisNewsContentLoc.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
