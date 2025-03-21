using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ARDashboardMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObkect;
    [SerializeField] protected TMP_Text thisDisplayName;

    [Header("Prefab Contents")]
    [SerializeField] protected GameObject thisProgressPrefab1;
    [SerializeField] protected GameObject thisProgressPrefab2;
    [SerializeField] protected GameObject thisCyberNewsPrefab;

    [Header("Prefab Holders")]
    [SerializeField] protected GameObject thisProgressLoc;
    [SerializeField] protected GameObject thisCyberNewsLoc;

    [Header("Contents")]
    [SerializeField] public PlayerDataSO PlayerData;
    [SerializeField] public ModulesSO ModulesData;

    private void Start()
    {
        this.thisParentObkect = this.gameObject;
        this.thisDisplayName = this.thisParentObkect.transform.Find("Dashboard_TopPanel/Dashboard_TopNameTMP").GetComponent<TMP_Text>();
        
        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");

        this.thisProgressPrefab1 = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Dashboard Panel/Dashboard_ProgressContentA");
        this.thisProgressPrefab2 = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Dashboard Panel/Dashboard_ProgressContentB");
        this.thisCyberNewsPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Dashboard Panel/Dashboard_CyberNewsContentA");

        this.thisProgressLoc = this.thisParentObkect.transform.Find("Dashboard_CoursesPanel/Dashboard_CoursesScrollPanel/Dashboard_CoursesScrollList").gameObject;
        this.thisCyberNewsLoc = this.thisParentObkect.transform.Find("Dashboard_CyberNewsPanel/Dashboard_CyberNewsScrollPanel/Dashboard_CyberNewsScrollList").gameObject;

        this.thisDisplayName.text = "Welcome Back " + this.PlayerData.User_FullName + "!";

        StartCoroutine(RefreshCourseList());
        //RefreshCyberNewsList();
    }

    private IEnumerator RefreshCourseList()
    {
        RefreshLists();

        yield return new WaitForSeconds(0.5f);

        foreach (var modulelist in this.ModulesData.ModuleList)
        {
            var lessonCount = modulelist.ModuleLessons.Count;

            foreach (var fmodule in this.PlayerData.FinishedCourseList)
            {
                var flessonCount = this.PlayerData.FinishedCourseList.Count;

                if (flessonCount == 0)
                {
                    GameObject create = Instantiate(this.thisProgressPrefab1);
                    create.transform.SetParent(this.thisProgressLoc.transform, false);
                    break;
                }

                if (modulelist.ModuleName == fmodule.CourseID)
                {
                    if (lessonCount != flessonCount)
                    {
                        GameObject create = Instantiate(this.thisProgressPrefab2);
                        create.transform.SetParent(this.thisProgressLoc.transform, false);

                        ARDashboardProgress script = create.GetComponent<ARDashboardProgress>();
                        script.ModuleName = modulelist.ModuleName;
                        break;
                    }

                    GameObject create2 = Instantiate(this.thisProgressPrefab1);
                    create2.transform.SetParent(this.thisProgressLoc.transform, false);
                    break;
                }
            }
        }
    }

    private void RefreshLists()
    {
        foreach (Transform child in this.thisProgressLoc.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in this.thisCyberNewsLoc.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
