using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARCourseContentMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected Button thisParentButton;
    [SerializeField] protected Button thisStartButton;
    [SerializeField] protected GameObject CourseListMenuPrefab;
    [SerializeField] protected GameObject CourseListMenuLessonPrefab;
    [SerializeField] protected TMP_Text thisCourseTMP;
    [SerializeField] protected TMP_Text thisCourseProgressTMP;
    [SerializeField] protected Image thisStatusImage;

    [Header("Prefabs")]
    [SerializeField] protected GameObject MainMenuCenterLoc;
    [SerializeField] protected GameObject LessonListPrefab;

    [Header("Contents")]
    public string ModuleNumber;
    public string ModuleName;
    public string ModuleDescription;

    [Header("Data's")]
    public PlayerDataSO PlayerData;
    public ModulesSO ModulesData;
    public List<ModuleLessons> ModuleLessons;

    public bool Toggled;
    public bool CanClick = true;

    public int FinishedLessons = 0;
    public int TotalLessons = 0;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisParentButton = GetComponent<Button>();
        this.thisStartButton = this.thisParentObject.transform.Find("CoursesMenu_Holder/CoursesMenu_StartButton").GetComponent<Button>();
        this.thisCourseTMP = this.thisParentObject.transform.Find("CoursesMenu_Holder/CoursesMenu_TMP").GetComponent<TMP_Text>();
        this.thisCourseProgressTMP = this.thisParentObject.transform.Find("CoursesMenu_Holder/CoursesMenu_ProgressTMP").GetComponent<TMP_Text>();
        this.thisStatusImage = this.thisParentObject.transform.Find("CoursesMenu_Holder/CoursesMenu_ProgressTMP/CoursesMenu_StatusImage").GetComponent<Image>();

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");

        this.CourseListMenuPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Courses Panel/CoursesMenu_List");
        this.CourseListMenuLessonPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Courses Panel/CourseMenu_LessonPanel");

        this.thisCourseTMP.text = this.ModuleName + "\nCourse: " + this.ModuleNumber;

        this.thisParentButton.onClick.AddListener(Clicked);
        this.thisStartButton.onClick.AddListener(CreateLessonPanel);

        this.MainMenuCenterLoc = GameObject.Find("MainMenu_CenterPanel");
        this.LessonListPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Lesson Menu Panel/LessonListMain_Panel");

        foreach (var mdl in this.ModulesData.ModuleList)
        {
            if (mdl.ModuleName == this.ModuleName)
            {
                this.TotalLessons = mdl.ModuleLessons.Count;
            }
        }

        SyncProgress();
    }

    private void CreateLessonPanel()
    {
        ResetLessonPanel();

        GameObject create = Instantiate(this.LessonListPrefab);
        create.transform.SetParent(this.MainMenuCenterLoc.transform, false);
        create.name = "LessonListMain_Panel";

        ARLessonListMain script = create.GetComponent<ARLessonListMain>();
        script.ModuleName = this.ModuleName;

        StartCoroutine(OpenLessonPanel());
    }

    public IEnumerator OpenLessonPanel()
    {
        yield return new WaitForSeconds(0.5f);

        if (this.FinishedLessons != this.TotalLessons)
        {
            GameObject find = GameObject.Find("LessonList_CenteredScrollList");

            foreach (Transform child in find.transform)
            {
                if (child.name == "LessonList_CenteredContent")
                {
                    ARLessonContentMain script2 = child.GetComponent<ARLessonContentMain>();

                    if (this.ModuleName == script2.thisCourseName)
                    {
                        if (this.FinishedLessons.ToString() == script2.thisModuleNumber)
                        {
                            script2.ClickedMe();
                        }

                        if (this.FinishedLessons.ToString() == "0")
                        {
                            if (script2.thisModuleNumber == "1")
                            {
                                script2.ClickedMe();
                            }
                        }
                    }
                }
            }
        }
    }

    private void ResetLessonPanel()
    {
        foreach (Transform child in this.MainMenuCenterLoc.transform)
        {
            if (child.name == "LessonListMain_Panel") 
            { 
                Destroy(child.gameObject);
            }
        }
    }

    private void SyncProgress()
    {
        this.FinishedLessons = 0;

        foreach (var thisCourse in this.ModulesData.ModuleList)
        {
            foreach (var thisCourseContent in thisCourse.ModuleLessons)
            {
                foreach (var FinishedList in this.PlayerData.FinishedCourseList)
                {
                    if (FinishedList.CourseID == this.ModuleName)
                    {
                        if (thisCourseContent.LessonTitle == FinishedList.LessonID)
                        {
                            this.FinishedLessons++;
                        }
                    }
                }
            }
        }

        if (this.FinishedLessons == this.TotalLessons)
        {
            Sprite sprite = Resources.Load<Sprite>("Textures/CompletedIcon");
            this.thisStatusImage.sprite = sprite;

            this.thisCourseProgressTMP.text = "Module Completed!";
            this.thisCourseProgressTMP.color = Color.green;
        }
        else if (this.FinishedLessons > 0 && this.FinishedLessons != this.TotalLessons)
        {
            Sprite sprite = Resources.Load<Sprite>("Textures/InProgressIcon");
            this.thisStatusImage.sprite = sprite;

            this.thisCourseProgressTMP.text = "In Progress";
            this.thisCourseProgressTMP.color = Color.yellow;
        }
        else
        {
            this.thisStatusImage.sprite = null;
            this.thisStatusImage.color = new Color32(0, 0, 0, 0);

            this.thisCourseProgressTMP.text = "";
            this.thisCourseProgressTMP.color = Color.white;
        }
    }

    private void Clicked()
    {
        if (!this.CanClick) return;
        this.CanClick = false;

        if (!this.Toggled)
        {
            this.Toggled = true;
            StartCoroutine(SpawnCourseMenu());
        }
        else
        {
            this.Toggled = false;
            StartCoroutine(CloseCourseMenu());
        }
    }

    private IEnumerator SpawnCourseMenu()
    {
        GameObject create = Instantiate(this.CourseListMenuPrefab);
        create.transform.SetParent(this.thisParentObject.transform, false);
        create.transform.name = "CourseMenu_LessonPanel";

        GameObject courseMenuHolder = create.transform.Find("CourseMenu_ButtonHolder").gameObject;
        TMP_Text DescriptionTMP = create.transform.Find("CourseMenu_DescriptionTMP").GetComponent<TMP_Text>();

        DescriptionTMP.text = this.ModuleDescription;

        foreach (var ModuleLessonList in this.ModuleLessons)
        {
            GameObject courseLessonPanel = Instantiate(this.CourseListMenuLessonPrefab);
            courseLessonPanel.transform.SetParent(courseMenuHolder.transform, false);
            courseLessonPanel.name = "CourseMenu_LessonPanel";

            TMP_Text LessonText = courseLessonPanel.transform.Find("CourseMenu_LessonText").GetComponent<TMP_Text>();
            LessonText.text = ModuleLessonList.LessonTitle;

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.5f);
        this.CanClick = true;
    }

    private IEnumerator CloseCourseMenu()
    {
        foreach (Transform child in this.thisParentObject.transform)
        {
            if (child.name == "CourseMenu_LessonPanel")
            {
                Transform buttonHolder = child.Find("CourseMenu_ButtonHolder");

                if (buttonHolder != null)
                {
                    foreach (Transform lessonPanel in buttonHolder)
                    {
                        if (lessonPanel.name == "CourseMenu_LessonPanel")
                        {
                            Animator lessonAnimator = lessonPanel.GetComponent<Animator>();
                            if (lessonAnimator != null)
                            {
                                lessonAnimator.SetTrigger("Close");
                            }

                            yield return new WaitForSeconds(0.15f);

                            Destroy(lessonPanel.gameObject);
                        }
                    }
                }
            }
        }

        foreach (Transform child in this.thisParentObject.transform)
        {
            if (child.name == "CourseMenu_LessonPanel")
            {
                TMP_Text DescriptionTMP = child.Find("CourseMenu_DescriptionTMP").GetComponent<TMP_Text>();
                DescriptionTMP.text = "";

                Destroy(child.gameObject);
            }
        }

        yield return new WaitForSeconds(0.5f);
        this.CanClick = true;
    }
}
