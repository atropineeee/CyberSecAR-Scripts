using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCoursesMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected GameObject thisScrollLoc;
    [SerializeField] protected GameObject thisScrollPrefab;

    [SerializeField] protected ModulesSO ModulesData;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisScrollLoc = this.thisParentObject.transform.Find("CoursesMenu_CenterScrollRect/CoursesMenu_CenterScrollList").gameObject;
        this.thisScrollPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Courses Panel/CoursesMenu_Content");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");
        AddComponentsInScroll();
    }

    private void AddComponentsInScroll()
    {
        ResetContents();

        foreach(var Modules in this.ModulesData.ModuleList)
        {
            GameObject create = Instantiate(this.thisScrollPrefab);
            create.transform.SetParent(this.thisScrollLoc.transform, false);
            
            ARCourseContentMain script = create.GetComponent<ARCourseContentMain>();
            script.ModuleNumber = Modules.ModuleNumber;
            script.ModuleName = Modules.ModuleName;
            script.ModuleDescription = Modules.ModuleDescription;
            script.ModuleLessons = Modules.ModuleLessons;
        }
    }

    private void ResetContents()
    {
        foreach(Transform child in this.thisScrollLoc.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
