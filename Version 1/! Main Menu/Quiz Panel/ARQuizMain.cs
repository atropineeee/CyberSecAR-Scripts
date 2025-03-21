using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARQuizMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;

    [SerializeField] protected GameObject qInstructionPrefab;
    [SerializeField] protected GameObject qLoadingPrefab;
    [SerializeField] protected GameObject qMainQuizPrefab;
    [SerializeField] protected GameObject qViewQuizPrefab;
    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.qInstructionPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizInstruction_Panel");
        this.qLoadingPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizLoading_Panel");
        this.qMainQuizPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizStart_Panel");

        CreateInstructionPref();
    }

    private void CreateInstructionPref()
    {
        GameObject create = Instantiate(this.qInstructionPrefab);
        create.transform.SetParent(this.thisParentObject.transform, false);
        create.name = "QuizInstruction_Panel";
    }
}
