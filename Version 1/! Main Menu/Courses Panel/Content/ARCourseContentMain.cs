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

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");

        this.CourseListMenuPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Courses Panel/CoursesMenu_List");
        this.CourseListMenuLessonPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Courses Panel/CourseMenu_LessonPanel");

        this.thisCourseTMP.text = this.ModuleName;

        this.thisParentButton.onClick.AddListener(Clicked);
        this.thisStartButton.onClick.AddListener(CreateLessonPanel);

        this.MainMenuCenterLoc = GameObject.Find("MainMenu_CenterPanel");
        this.LessonListPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Lesson Menu Panel/LessonListMain_Panel");

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
            this.TotalLessons = thisCourse.ModuleLessons.Count;

            foreach (var thisCourseContent in thisCourse.ModuleLessons)
            {
                foreach (var FinishedList in this.PlayerData.FinishedCourseList)
                {
                    if (thisCourseContent.LessonTitle == FinishedList.LessonID)
                    {
                        this.FinishedLessons++;
                    }
                }
            }

            if (this.TotalLessons == this.FinishedLessons)
            {
                this.thisCourseProgressTMP.text = "Module Completed!";
                this.thisCourseProgressTMP.color = Color.green;
            }
            else
            {
                this.thisCourseProgressTMP.text = "In Progress";
                this.thisCourseProgressTMP.color = Color.yellow;
            }
        }

    }

    private void Clicked()
    {
        if (!this.CanClick) return;
        this.CanClick = false;
        StartCoroutine(ResetClick());

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

        yield return StartCoroutine(TypeText(DescriptionTMP, this.ModuleDescription, 0.05f));

        foreach (var ModuleLessonList in this.ModuleLessons)
        {
            GameObject courseLessonPanel = Instantiate(this.CourseListMenuLessonPrefab);
            courseLessonPanel.transform.SetParent(courseMenuHolder.transform, false);
            courseLessonPanel.name = "CourseMenu_LessonPanel";

            TMP_Text LessonText = courseLessonPanel.transform.Find("CourseMenu_LessonText").GetComponent<TMP_Text>();
            LessonText.text = ModuleLessonList.LessonTitle;

            yield return new WaitForSeconds(0.15f);
        }
    }

    private IEnumerator CloseCourseMenu()
    {
        List<GameObject> panelsToDestroy = new List<GameObject>();

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

                            panelsToDestroy.Add(lessonPanel.gameObject);
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        foreach (GameObject panel in panelsToDestroy)
        {
            Destroy(panel);
        }

        foreach (Transform child in this.thisParentObject.transform)
        {
            if (child.name == "CourseMenu_LessonPanel")
            {
                TMP_Text DescriptionTMP = child.Find("CourseMenu_DescriptionTMP").GetComponent<TMP_Text>();
                yield return StartCoroutine(EraseText(DescriptionTMP, 0.05f));

                Destroy(child.gameObject);
            }
        }
    }

    private IEnumerator TypeText(TMP_Text textComponent, string text, float duration)
    {
        float timePerChar = duration / text.Length;
        textComponent.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            textComponent.text += text[i];
            yield return new WaitForSeconds(timePerChar);
        }
    }

    private IEnumerator EraseText(TMP_Text textComponent, float duration)
    {
        float timePerChar = duration / textComponent.text.Length;
        string originalText = textComponent.text;

        for (int i = originalText.Length; i > 0; i--)
        {
            textComponent.text = originalText.Substring(0, i - 1);
            yield return new WaitForSeconds(timePerChar);
        }

        textComponent.text = "";
    }

    private IEnumerator ResetClick()
    {
        yield return new WaitForSeconds(1f);
        this.CanClick = true;
    }
}
