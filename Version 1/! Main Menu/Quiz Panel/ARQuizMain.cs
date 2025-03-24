using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARQuizMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;

    [SerializeField] protected GameObject qInstructionPrefab;

    public string thisCourseName;
    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.qInstructionPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizInstruction_Panel");
        CreateInstructionPref();
    }

    private void CreateInstructionPref()
    {
        GameObject create = Instantiate(this.qInstructionPrefab);
        create.transform.SetParent(this.thisParentObject.transform, false);
        create.name = "QuizInstruction_Panel";

        ARQuizInstructionMain script = create.GetComponent<ARQuizInstructionMain>();
        script.thisCourseName = this.thisCourseName;
    }
}
