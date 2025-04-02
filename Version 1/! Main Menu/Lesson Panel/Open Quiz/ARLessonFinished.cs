using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARLessonFinished : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected Button thisTakeQuizButton;

    [SerializeField] protected GameObject QuizMainPanelPrefab;
    [SerializeField] protected GameObject QuizMainPanelPrefabLoc;

    public QuizesSO QuizesData;

    public string thisCourseName;

    protected bool isClicked;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisTakeQuizButton = this.thisParentObject.transform.Find("TakeQuizButton").GetComponent<Button>();
        this.QuizesData = Resources.Load<QuizesSO>("! Scriptable Objects/Quizes Data/QuizData");
        this.QuizMainPanelPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizMain_Panel");
        this.QuizMainPanelPrefabLoc = GameObject.Find("MainMenu_CenterPanel");

        this.thisTakeQuizButton.onClick.AddListener(CreateQuizPanel);
    }

    public void CreateQuizPanel()
    {
        if (this.isClicked) { return; }
        this.isClicked = true;
        StartCoroutine(ResetClick());

        foreach (var QuizName in this.QuizesData.QuizList)
        {
            if (this.thisCourseName == QuizName.QuizTopicName)
            {
                GameObject create = Instantiate(this.QuizMainPanelPrefab);
                create.transform.SetParent(this.QuizMainPanelPrefabLoc.transform, false);
                create.name = "QuizMain_Panel";

                ARQuizMain script = create.GetComponent<ARQuizMain>();
                script.thisCourseName = this.thisCourseName;
                return;
            }
        }
    }

    private IEnumerator ResetClick()
    {
        yield return new WaitForSeconds(0.5f);
        this.isClicked = false;
    }
}
