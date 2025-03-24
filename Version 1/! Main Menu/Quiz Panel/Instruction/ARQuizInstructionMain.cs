using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARQuizInstructionMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected GameObject qLoadingPrefab;
    [SerializeField] protected GameObject QuizMainPanelPrefabLoc;
    [SerializeField] protected Button thisStartButton;
    [SerializeField] protected Button thisExitButton;

    public string thisCourseName;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.qLoadingPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizLoading_Panel");
        this.QuizMainPanelPrefabLoc = GameObject.Find("QuizMain_Panel");

        this.thisStartButton = this.thisParentObject.transform.Find("StartQuiz_Button").GetComponent<Button>();
        this.thisExitButton = this.thisParentObject.transform.Find("ExitQuiz_Button").GetComponent<Button>();

        this.thisStartButton.onClick.AddListener(StartQuiz);
        this.thisExitButton.onClick.AddListener(CloseQuizPanel);
    }

    private void StartQuiz()
    {
        GameObject create = Instantiate(this.qLoadingPrefab);
        create.transform.SetParent(this.QuizMainPanelPrefabLoc.transform, false);

        ARQuizLoadingMain script = create.GetComponent<ARQuizLoadingMain>();
        script.thisCourseName = this.thisCourseName;
    }

    private void CloseQuizPanel()
    {
        GameObject find = GameObject.Find("QuizMain_Panel");
        Destroy(find.gameObject);
    }
}
