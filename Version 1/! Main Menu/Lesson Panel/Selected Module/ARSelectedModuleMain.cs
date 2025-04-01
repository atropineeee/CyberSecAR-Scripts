using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARSelectedModuleMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] public GameObject thisParentObject;
    [SerializeField] public TMP_Text thisLessonTitle;
    [SerializeField] public TMP_Text thisLessonContent;
    [SerializeField] public Animator thisAnimator;

    [SerializeField] protected Button thisPreviousButton;
    [SerializeField] protected Button thisNextButton;
    [SerializeField] protected Button thisMenuButton;

    [Header("Menu Components")]
    [SerializeField] public Button thisARButton;
    [SerializeField] public Button thisFunFactButton;
    [SerializeField] public Button thisBackButton;
    [SerializeField] public Button thisReturnButton;
    [SerializeField] public Slider thisSlider;
    [SerializeField] public TMP_Text thisSliderText;
    [SerializeField] public Animator thisMenuAnimator;

    [SerializeField] public GameObject VideoPrefabLoc;
    [SerializeField] public GameObject VideoPrefab;

    public string currentCourseName;
    public string currentModuleTitle;
    public string currentModuleContent;
    public string currentModuleNumber;
    public int currentModuleInt;
    public int currentMaxModuleInt;

    public ModulesSO ModulesData;

    public bool IsClicked;

    private ARSelectedModuleMenu ARSelectedModuleMenu;
    public ARScriptHolderMain ARScriptHolderMain;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisAnimator = GetComponent<Animator>();
        this.thisLessonTitle = this.thisParentObject.transform.Find("Lesson_TopPanel/Lesson_TitleTMP").GetComponent<TMP_Text>();
        this.thisLessonContent = this.thisParentObject.transform.Find("Lesson_CenterPanel/Lesson_CenterScrollRect/Lesson_CenterScrollList/Lesson_CenterContent/Lesson_CenterTextTMP").GetComponent<TMP_Text>();

        this.thisNextButton = this.thisParentObject.transform.Find("Lesson_BottomPanel/Lesson_NextButton").GetComponent<Button>();
        this.thisPreviousButton = this.thisParentObject.transform.Find("Lesson_BottomPanel/Lesson_PreviousButton").GetComponent<Button>();    
        this.thisMenuButton = this.thisParentObject.transform.Find("Lesson_BottomPanel/Lesson_MenuButton").GetComponent<Button>();

        this.thisARButton = this.thisParentObject.transform.Find("Lesson_MenuHolder/Lesson_MenuPanel/Lesson_ARButton").GetComponent<Button>();
        this.thisFunFactButton = this.thisParentObject.transform.Find("Lesson_MenuHolder/Lesson_MenuPanel/Lesson_FunFactButton").GetComponent<Button>();
        this.thisBackButton = this.thisParentObject.transform.Find("Lesson_MenuHolder/Lesson_MenuPanel/Lesson_BackButton").GetComponent<Button>();
        this.thisReturnButton = this.thisParentObject.transform.Find("Lesson_MenuHolder").GetComponent<Button>();
        this.thisSlider = this.thisParentObject.transform.Find("Lesson_MenuHolder/Lesson_MenuPanel/LessonAdjust_Holder/LessonAdjust_Slider").GetComponent<Slider>();
        this.thisSliderText = this.thisParentObject.transform.Find("Lesson_MenuHolder/Lesson_MenuPanel/LessonAdjust_Holder/LessonAdjust_TextTMP").GetComponent<TMP_Text>();
        this.thisMenuAnimator = this.thisParentObject.transform.Find("Lesson_MenuHolder").GetComponent<Animator>();

        this.ARScriptHolderMain = GameObject.Find("ScriptsHolder").GetComponent<ARScriptHolderMain>();

        this.VideoPrefabLoc = GameObject.Find("Mid Air Stage");

        foreach (var mdl in this.ModulesData.ModuleList)
        {
            this.currentMaxModuleInt = mdl.ModuleLessons.Count;
        }

        this.thisLessonTitle.text = this.currentModuleTitle;
        this.thisLessonContent.text = this.currentModuleContent;
        this.currentModuleInt = Convert.ToInt32(this.currentModuleNumber);

        this.ARSelectedModuleMenu = new ARSelectedModuleMenu(this);

        this.thisPreviousButton.onClick.AddListener(PreviousModule);
        this.thisNextButton.onClick.AddListener(NextModule);
        this.thisBackButton.onClick.AddListener(this.ARSelectedModuleMenu.CloseThisMenu);
        this.thisARButton.onClick.AddListener(this.ARSelectedModuleMenu.OpenARMenu);
        this.thisARButton.onClick.AddListener(this.ARSelectedModuleMenu.CloseThisMenu);

        this.thisMenuButton.onClick.AddListener(this.ARSelectedModuleMenu.OpenMenu);
        this.thisReturnButton.onClick.AddListener(this.ARSelectedModuleMenu.CloseMenu);
        this.thisSlider.onValueChanged.AddListener(value => this.ARSelectedModuleMenu.ChangeTextSize(value));
    }

    private void NextModule()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;
        StartCoroutine(ResetClicked());

        if (this.currentModuleInt != this.currentMaxModuleInt)
        {
            this.currentModuleInt++;
            var mdl = this.ModulesData.ModuleList.FirstOrDefault(m => m.ModuleName == this.currentCourseName);
            this.currentModuleTitle = mdl.ModuleLessons[this.currentModuleInt -1].LessonTitle;
            this.currentModuleContent = mdl.ModuleLessons[this.currentModuleInt - 1].LessonContent;
            this.currentModuleNumber = mdl.ModuleLessons[this.currentModuleInt - 1].LessonID;

            ResetList();

            RefreshText();
            SyncUpdate();
        }
    }

    private void PreviousModule()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;
        StartCoroutine(ResetClicked());

        if (this.currentModuleInt != 1)
        {
            this.currentModuleInt--;
            var mdl = this.ModulesData.ModuleList.FirstOrDefault(m => m.ModuleName == this.currentCourseName);
            this.currentModuleTitle = mdl.ModuleLessons[this.currentModuleInt - 1].LessonTitle;
            this.currentModuleContent = mdl.ModuleLessons[this.currentModuleInt - 1].LessonContent;
            this.currentModuleNumber = mdl.ModuleLessons[this.currentModuleInt - 1].LessonID;

            ResetList();

            RefreshText();
            SyncUpdate();
        }
    }

    private void RefreshText()
    {
        this.thisLessonTitle.text = "";
        this.thisLessonContent.text = "";

        this.thisLessonTitle.text = this.currentModuleTitle;
        this.thisLessonContent.text = this.currentModuleContent;
    }

    private void ResetList()
    {
        GameObject find = GameObject.Find("LessonListMain_Panel");
        ARLessonListMain script = find.GetComponent<ARLessonListMain>();
        script.CreateLessonsList();
    }

    private void SyncUpdate()
    {
        GameObject find = GameObject.Find("LessonList_CenteredScrollList");

        foreach (Transform child in find.transform)
        {
            if (child.name == "LessonList_CenteredContent")
            {
                ARLessonContentMain script = child.GetComponent<ARLessonContentMain>();

                if (script.thisCourseName == this.currentCourseName)
                {
                    if (script.thisModuleName == this.currentModuleTitle)
                    {
                        if (!script.IsFinished && script.IsPreviousFinished)
                        {
                            script.ARLessonContentUpdate.SyncFirebase();
                        }
                    }
                }
            }
        }
    }
    
    public IEnumerator ResetClicked()
    {
        yield return new WaitForSeconds(2.5f);
        this.IsClicked = false;
    }
}
