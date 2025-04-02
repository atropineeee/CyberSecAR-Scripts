using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARDashboardProgress : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected Button thisResumeButton;
    [SerializeField] protected TMP_Text thisTMPText;

    [Header("Prefabs")]
    [SerializeField] protected GameObject MainMenuCenterLoc;
    [SerializeField] protected GameObject LessonListPrefab;

    [SerializeField] protected ARMainMenuMain ARMainMenuMain;

    [Header("Contents")]
    public string ModuleName;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisResumeButton = this.thisParentObject.transform.Find("Dashboard_Resume").GetComponent<Button>();
        this.thisTMPText = this.thisParentObject.transform.Find("Dashboard_TitleTMP").GetComponent<TMP_Text>();

        this.MainMenuCenterLoc = GameObject.Find("MainMenu_CenterPanel");
        this.LessonListPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Lesson Menu Panel/LessonListMain_Panel");

        this.ARMainMenuMain = GameObject.Find("MainMenu_Panel").GetComponent<ARMainMenuMain>();

        this.thisTMPText.text = this.ModuleName;

        this.thisResumeButton.onClick.AddListener(CreateLessonPanel);
    }

    private void CreateLessonPanel()
    {
        ResetLessonPanel();

        this.ARMainMenuMain.ARMainMenuPanels.CreateCoursesPanel();
        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.Courses;
        this.ARMainMenuMain.ChangeTopLabel("Courses");

        GameObject create = Instantiate(this.LessonListPrefab);
        create.transform.SetParent(this.MainMenuCenterLoc.transform, false);
        create.name = "LessonListMain_Panel";

        ARLessonListMain script = create.GetComponent<ARLessonListMain>();
        script.ModuleName = this.ModuleName;
        script.AutoOpen = true;
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

}
