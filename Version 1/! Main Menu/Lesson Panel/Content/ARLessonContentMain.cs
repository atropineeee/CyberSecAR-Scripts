using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARLessonContentMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected Button thisParentButton;

    [SerializeField] protected TMP_Text thisModuleNameTMP;
    [SerializeField] protected TMP_Text thisModuleNumberTMP;
    [SerializeField] protected TMP_Text thisModuleStatusTMP;
    [SerializeField] protected Image thisModuleStatusImage;

    [Header("Required Scripts")]
    [SerializeField] public ARLessonContentUpdate ARLessonContentUpdate;

    protected GameObject LessonPanelPrefab;
    protected GameObject LessonPanelLoc;

    public PlayerDataSO PlayerData;
    public ModulesSO ModulesData;

    public string thisCourseName;
    public string thisModuleNumber;
    public string thisModuleName;
    public string thisModuleContent;

    public bool IsPreviousFinished;
    public bool IsFinished;
    private bool IsClicked;

    public string CurrentEmail;
    public string LessonID;
    public string LessonContent;
    public string CourseID;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisParentButton = GetComponent<Button>();

        this.thisModuleNumberTMP = this.thisParentObject.transform.Find("LessonList_LessonNumberTMP").GetComponent<TMP_Text>();
        this.thisModuleNameTMP = this.thisParentObject.transform.Find("LessonList_LessonNameTMP").GetComponent<TMP_Text>();
        this.thisModuleStatusTMP = this.thisParentObject.transform.Find("LessonList_LessonProgressTMP").GetComponent<TMP_Text>();
        this.thisModuleStatusImage = this.thisParentObject.transform.Find("LessonList_LessonProgressTMP/LessonList_ImageStatus").GetComponent<Image>();

        this.LessonPanelLoc = GameObject.Find("MainMenu_CenterPanel").gameObject;
        this.LessonPanelPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Lesson Menu Panel/LessonMain_Panel");

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");

        this.thisModuleNumberTMP.text = "Module " + this.thisModuleNumber;
        this.thisModuleNameTMP.text = this.thisModuleName;

        this.ARLessonContentUpdate = new ARLessonContentUpdate(this);

        this.thisParentButton.onClick.AddListener(ClickedMe);

        if (this.IsPreviousFinished)
        {
            if (this.IsFinished)
            {
                Sprite sprite = Resources.Load<Sprite>("Textures/CompletedIcon");
                this.thisModuleStatusImage.sprite = sprite;

                this.thisModuleStatusTMP.text = "Completed!";
                this.thisModuleStatusTMP.color = Color.green;
            }
            else
            {
                Sprite sprite = Resources.Load<Sprite>("Textures/InProgressIcon");
                this.thisModuleStatusImage.sprite = sprite;

                this.thisModuleStatusTMP.text = "In Progress!";
                this.thisModuleStatusTMP.color = Color.yellow;
            }
        }
        else
        {
            this.thisModuleStatusImage.sprite = null;
            this.thisModuleStatusImage.color = new Color32(0, 0, 0, 0);

            this.thisModuleStatusTMP.text = "";
        }

        this.ARLessonContentUpdate.SyncFirebase();

        this.CurrentEmail = this.PlayerData.User_Email;
        this.LessonID = this.thisModuleName;
        this.LessonContent = this.thisModuleContent;
        this.CourseID = this.thisCourseName;
    }

    public void ClickedMe()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;
        StartCoroutine(ResetClick());
        CreateObject(this.thisModuleNumber, this.thisModuleName, this.thisModuleContent, this.thisCourseName, this.ModulesData);
        if (!this.IsFinished && this.IsPreviousFinished)
        {
            this.ARLessonContentUpdate.SyncData();
        }
    }

    private void CreateObject(string MNn, string MNm, string MCn, string CNm, ModulesSO MSo)
    {
        GameObject create = Instantiate(this.LessonPanelPrefab);
        create.transform.SetParent(this.LessonPanelLoc.transform, false);
        create.name = "LessonMain_Panel";

        ARSelectedModuleMain script = create.GetComponent<ARSelectedModuleMain>();
        script.currentModuleNumber = MNn;
        script.currentModuleTitle = MNm;
        script.currentModuleContent = MCn;
        script.currentCourseName = CNm;
        script.ModulesData = MSo;
    }

    private IEnumerator ResetClick()
    {
        yield return new WaitForSeconds(1f);
        this.IsClicked = false;
    }
}
