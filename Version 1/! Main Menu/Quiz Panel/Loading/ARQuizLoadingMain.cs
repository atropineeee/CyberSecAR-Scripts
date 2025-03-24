using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARQuizLoadingMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected GameObject qStartPrefab;
    [SerializeField] protected GameObject QuizMainPanelPrefabLoc;

    public string thisCourseName;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.qStartPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizStart_Panel");
        this.QuizMainPanelPrefabLoc = GameObject.Find("QuizMain_Panel");
        StartCoroutine(StartQuiz());
    }

    private IEnumerator StartQuiz()
    {
        yield return new WaitForSeconds(1f);

        GameObject create = Instantiate(this.qStartPrefab);
        create.transform.SetParent(this.QuizMainPanelPrefabLoc.transform, false);
        create.name = "QuizStart_Panel";

        ARQuizStartMain script = create.GetComponent<ARQuizStartMain>();
        script.thisCourseName = this.thisCourseName;

        Destroy(this.thisParentObject.gameObject, 0.25f);
    }
}
