using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARLessonListMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected Animator thisAnimator;
    [SerializeField] protected Button thisBackButton;
    [SerializeField] protected TMP_Text thisModuleNameTMP;

    [SerializeField] protected GameObject LessonListLoc;
    [SerializeField] protected GameObject LessonFinPrefab;
    [SerializeField] protected GameObject LessonListPrefab;

    [Header("Contents")]
    public PlayerDataSO PlayerData;
    public ModulesSO ModulesData;
    public string ModuleName;
    public int LessonCount = 0;
    public bool[] Finished;
    public bool IsClicked;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisAnimator = GetComponent<Animator>();

        this.thisBackButton = this.thisParentObject.transform.Find("LessonList_TopPanel/LessonList_BackButton").GetComponent<Button>();
        this.thisModuleNameTMP = this.thisParentObject.transform.Find("LessonList_CenterMainPanel/LessonList_CenterTopPanel/LessonList_ModuleNameTMP").GetComponent<TMP_Text>();
        this.LessonListLoc = this.thisParentObject.transform.Find("LessonList_CenterMainPanel/LessonList_CenteredMainPanel/LessonList_CenteredScrollRect/LessonList_CenteredScrollList").gameObject;

        this.LessonFinPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Lesson Menu Panel/LessonList_CenteredContentF");
        this.LessonListPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Lesson Menu Panel/LessonList_CenteredContent");

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");

        this.thisBackButton.onClick.AddListener(CloseButton);

        CreateLessonsList();

        this.thisModuleNameTMP.text = this.ModuleName + "\n(" + LessonCount + " Modules)";
    }

    public void CreateLessonsList()
    {
        ResetList();

        this.LessonCount = 0;

        foreach (var LessonList in this.ModulesData.ModuleList)
        {
            this.Finished = new bool[LessonList.ModuleLessons.Count];

            foreach (var LessonData in LessonList.ModuleLessons)
            {
                if (LessonList.ModuleName == this.ModuleName)
                {
                    GameObject create = Instantiate(this.LessonListPrefab);
                    create.transform.SetParent(this.LessonListLoc.transform, false);
                    create.name = "LessonList_CenteredContent";

                    ARLessonContentMain script = create.GetComponent<ARLessonContentMain>();
                    script.thisModuleNumber = LessonData.LessonID;
                    script.thisModuleName = LessonData.LessonTitle;
                    script.thisModuleContent = LessonData.LessonContent;
                    script.thisCourseName = LessonList.ModuleName;

                    if (this.LessonCount == 0)
                    {
                        script.IsPreviousFinished = true;
                    }


                    foreach (var LessonFinished in this.PlayerData.FinishedCourseList)
                    {
                        if (this.LessonCount == 0)
                        {
                            script.IsPreviousFinished = true;

                            if (LessonData.LessonTitle == LessonFinished.LessonID)
                            {
                                script.IsFinished = true;
                                this.Finished[this.LessonCount] = true;
                                break;
                            }
                            else
                            {
                                script.IsFinished = false;
                                this.Finished[this.LessonCount] = false;
                            }
                        }
                        else
                        {
                            if (this.Finished[this.LessonCount - 1] == true)
                            {
                                script.IsPreviousFinished = true;

                                if (LessonData.LessonTitle == LessonFinished.LessonID)
                                {
                                    script.IsFinished = true;
                                    this.Finished[this.LessonCount] = true;
                                    break;
                                }
                                else
                                {
                                    script.IsFinished = false;
                                    this.Finished[this.LessonCount] = false;
                                }
                            }
                            else
                            {
                                script.IsPreviousFinished = false;

                                if (LessonData.LessonTitle == LessonFinished.LessonID)
                                {
                                    script.IsFinished = true;
                                    this.Finished[this.LessonCount] = true;
                                    break;
                                }
                                else
                                {
                                    script.IsFinished = false;
                                    this.Finished[this.LessonCount] = false;
                                }
                            }
                        }
                    }

                    if (this.Finished.All(f => f))
                    {
                        GameObject create2 = Instantiate(this.LessonFinPrefab);
                        create2.transform.SetParent(this.LessonListLoc.transform, false);
                        ARLessonFinished script2 = create2.GetComponent<ARLessonFinished>();
                        script2.thisCourseName = this.ModuleName;
                    }

                    this.LessonCount++;
                }
            }
        }
    }

    private void ResetList()
    {
        foreach (Transform child in this.LessonListLoc.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CloseButton()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;
        this.thisAnimator.SetTrigger("Close");
        Destroy(this.thisParentObject.gameObject, 0.6f);
    }
}
