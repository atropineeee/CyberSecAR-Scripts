using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ARDashboardMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected TMP_Text thisDisplayName;
    [SerializeField] protected Button thisViewAllProgressButton;
    [SerializeField] protected Button thisViewAllNewsButton;

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
    [SerializeField] public NewsSO NewsData;

    [SerializeField] protected ARMainMenuMain ARMainMenuMain;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisDisplayName = this.thisParentObject.transform.Find("Dashboard_TopPanel/Dashboard_TopNameTMP").GetComponent<TMP_Text>();
        this.thisViewAllProgressButton = this.thisParentObject.transform.Find("Dashboard_CoursesPanel/Dashboard_CoursesTopPanel/Dashboard_CoursesLabelViewAllButton").GetComponent<Button>();
        this.thisViewAllNewsButton = this.thisParentObject.transform.Find("Dashboard_CyberNewsPanel/Dashboard_CyberNewsTopPanel/Dashboard_CyberNewsViewAllButton").GetComponent<Button>();

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");
        this.NewsData = Resources.Load<NewsSO>("! Scriptable Objects/News Data/NewsData");

        this.thisProgressPrefab1 = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Dashboard Panel/Dashboard_ProgressContentA");
        this.thisProgressPrefab2 = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Dashboard Panel/Dashboard_ProgressContentB");
        this.thisCyberNewsPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Dashboard Panel/Dashboard_CyberNewsContentA");

        this.thisProgressLoc = this.thisParentObject.transform.Find("Dashboard_CoursesPanel/Dashboard_CoursesScrollPanel/Dashboard_CoursesScrollList").gameObject;
        this.thisCyberNewsLoc = this.thisParentObject.transform.Find("Dashboard_CyberNewsPanel/Dashboard_CyberNewsScrollPanel/Dashboard_CyberNewsScrollList").gameObject;

        this.ARMainMenuMain = GameObject.Find("MainMenu_Panel").GetComponent<ARMainMenuMain>();

        this.thisDisplayName.text = "Welcome Back " + this.PlayerData.User_FullName + "!";

        this.thisViewAllProgressButton.onClick.AddListener(CreateCoursesPanel);
        this.thisViewAllNewsButton.onClick.AddListener(CreateNewsPanel);

        StartCoroutine(RefreshCourseList());
        StartCoroutine(RefreshCyberNewsList());
    }

    protected void CreateCoursesPanel()
    {
        this.ARMainMenuMain.ARMainMenuPanels.CreateCoursesPanel();
        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.Courses;
        this.ARMainMenuMain.ChangeTopLabel("Courses");
    }

    protected void CreateNewsPanel()
    {
        this.ARMainMenuMain.ARMainMenuPanels.CreateCyberNewsPanel();
        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.CyberNews;
        this.ARMainMenuMain.ChangeTopLabel("CyberNews");
    }

    private IEnumerator RefreshCourseList()
    {
        RefreshLists();

        yield return new WaitForSeconds(1f);

        var flessonCount = this.PlayerData.FinishedCourseList.Count;

        if (flessonCount == 0)
        {
            GameObject create = Instantiate(this.thisProgressPrefab1);
            create.transform.SetParent(this.thisProgressLoc.transform, false);
        }
        else
        {
            bool anyModuleInProgress = false;

            foreach (var modulelist in this.ModulesData.ModuleList)
            {
                var lessonCount = modulelist.ModuleLessons.Count;
                var similarLessonCount = 0;
                bool created = false;
                bool exist = false;

                foreach (var fmodule in this.PlayerData.FinishedCourseList)
                {
                    if (modulelist.ModuleName == fmodule.CourseID)
                    {
                        exist = true;
                        similarLessonCount++;
                    }
                }

                if (similarLessonCount != lessonCount)
                {
                    if (!created && exist)
                    {
                        anyModuleInProgress = true;
                        created = true;
                        GameObject create = Instantiate(this.thisProgressPrefab2);
                        create.transform.SetParent(this.thisProgressLoc.transform, false);
                        create.name = modulelist.ModuleName;

                        ARDashboardProgress script = create.GetComponent<ARDashboardProgress>();
                        script.ModuleName = modulelist.ModuleName;
                    }
                }
            }

            if (!anyModuleInProgress)
            {
                GameObject create2 = Instantiate(this.thisProgressPrefab1);
                create2.transform.SetParent(this.thisProgressLoc.transform, false);
            }
        }
    }

    private IEnumerator RefreshCyberNewsList ()
    {
        RefreshLists();

        yield return new WaitForSeconds(1f);

        foreach (var newsList in this.NewsData.NewsList) 
        {
            GameObject create = Instantiate(this.thisCyberNewsPrefab);
            create.transform.SetParent(this.thisCyberNewsLoc.transform, false);

            ARDashboardNews script = create.GetComponent<ARDashboardNews>();
            script.NewsTitle = newsList.NewsTopic;
            script.NewsDescription = newsList.NewsDescription;
            script.NewsImage = newsList.NewsImage;
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
