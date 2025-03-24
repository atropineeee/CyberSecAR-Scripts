using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARQuizStartMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] public GameObject thisParentObject;

    [SerializeField] public GameObject FinishedQuizPrefab;
    [SerializeField] public GameObject FinishedQuizLoc;

    [Header("Question Number")]
    [SerializeField] public TMP_Text QuestionTMP;
    [SerializeField] public TMP_Text QuestionNumberTMP;

    [Header("Timer")]
    [SerializeField] protected Slider QuizTimerSlider;
    [SerializeField] protected TMP_Text QuizTimerTMP;

    [SerializeField] protected float quizTime = 60f;
    [SerializeField] protected float currentTime;
    protected Coroutine timerCoroutine;

    [Header("Extended Scripts")]
    public ARQuizFinished ARQuizFinished;
    public ARQuizFunctions ARQuizFunctions;
    public ARQuizQuestionaire ARQuizQuestionaire;

    [Header("Buttons")]
    [SerializeField] public TMP_Text CorrectTMP;

    [SerializeField] public Button ChoiceAButton;
    [SerializeField] public Button ChoiceBButton;
    [SerializeField] public Button ChoiceCButton;
    [SerializeField] public Button ChoiceDButton;
    [SerializeField] public Button NextQuestionButton;
    [SerializeField] public QuizesSO QuizesData;

    [Header("TMP Text")]
    [SerializeField] public TMP_Text ChoiceAText;
    [SerializeField] public TMP_Text ChoiceBText;
    [SerializeField] public TMP_Text ChoiceCText;
    [SerializeField] public TMP_Text ChoiceDText;

    [Header("Images")]
    [SerializeField] public Image ChoiceAImage;
    [SerializeField] public Image ChoiceBImage;
    [SerializeField] public Image ChoiceCImage;
    [SerializeField] public Image ChoiceDImage;

    public string[] SelectedChoices;

    public bool ChoiceACorrect;
    public bool ChoiceBCorrect;
    public bool ChoiceCCorrect;
    public bool ChoiceDCorrect;

    public bool SelectedA;
    public bool SelectedB;
    public bool SelectedC;
    public bool SelectedD;

    public int CurrentQuestionNumber = 0;
    public int TotalQuestionNumber;
    public int CurrentScore = 0;
    public int MaximumScore;

    public string thisCourseName;
    public bool thisAnswered;
    public bool timeIsUp;
    public bool thisQuizDone;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.QuizesData = Resources.Load<QuizesSO>("! Scriptable Objects/Quizes Data/QuizData");

        this.QuestionTMP = this.thisParentObject.transform.Find("QuizStart_Top/QuizStart_QS/QuizStart_QSTMP").GetComponent<TMP_Text>();
        this.QuestionNumberTMP = this.thisParentObject.transform.Find("QuizStart_Top/QuizStart_TMP").GetComponent<TMP_Text>();

        this.QuizTimerSlider = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_CenterTop/QuizStart_CenterTimer").GetComponent<Slider>();
        this.QuizTimerTMP = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_CenterTop/QuizStart_CenterTMPTimer").GetComponent<TMP_Text>();

        this.ChoiceAButton = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonA").GetComponent<Button>();
        this.ChoiceBButton = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonB").GetComponent<Button>();
        this.ChoiceCButton = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonC").GetComponent<Button>();
        this.ChoiceDButton = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonD").GetComponent<Button>();
        this.NextQuestionButton = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizNextButton").GetComponent<Button>();

        this.ChoiceAText = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonA/ChoiceA").GetComponent<TMP_Text>();
        this.ChoiceBText = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonB/ChoiceB").GetComponent<TMP_Text>();
        this.ChoiceCText = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonC/ChoiceC").GetComponent<TMP_Text>();
        this.ChoiceDText = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonD/ChoiceD").GetComponent<TMP_Text>();

        this.ChoiceAImage = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonA").GetComponent<Image>();
        this.ChoiceBImage = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonB").GetComponent<Image>();
        this.ChoiceCImage = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonC").GetComponent<Image>();
        this.ChoiceDImage = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizButtonD").GetComponent<Image>();

        this.CorrectTMP = this.thisParentObject.transform.Find("QuizStart_Center/QuizStart_SCRect/QuizStart_SCList/QuizStart_Holder/QuizCorrectTMP").GetComponent<TMP_Text>();

        this.ChoiceAButton.onClick.AddListener(ChoiceAClicked);
        this.ChoiceBButton.onClick.AddListener(ChoiceBClicked);
        this.ChoiceCButton.onClick.AddListener(ChoiceCClicked);
        this.ChoiceDButton.onClick.AddListener(ChoiceDClicked);
        this.NextQuestionButton.onClick.AddListener(NextQuestionClicked);

        this.ARQuizFinished = new ARQuizFinished(this);
        this.ARQuizFunctions = new ARQuizFunctions(this);
        this.ARQuizQuestionaire = new ARQuizQuestionaire(this);

        this.CorrectTMP.text = "";

        this.FinishedQuizPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/QuizPanel/QuizFinished_Panel");
        this.FinishedQuizLoc = GameObject.Find("QuizMain_Panel");

        foreach (var QuizList in this.QuizesData.QuizList)
        {
            if (QuizList.QuizTopicName == this.thisCourseName)
            {
                this.SelectedChoices = new string[QuizList.QuizQNAList.Count];
            }
        }

        ResetTimer();
        this.ARQuizQuestionaire.ResetQuestions();
        this.ARQuizFunctions.ResetButtonColors();
    }

    private void ChoiceAClicked()
    {
        if (this.thisAnswered) { return; }
        this.thisAnswered = true;
        this.SelectedA = true;

        this.ARQuizFunctions.ShowRightAnswer();
        this.ARQuizFinished.CheckIfQuizFinished();

        this.SelectedChoices[this.CurrentQuestionNumber - 1] = "A";
    }

    private void ChoiceBClicked()
    {
        if (this.thisAnswered) { return; }
        this.thisAnswered = true;
        this.SelectedB = true;

        this.ARQuizFunctions.ShowRightAnswer();
        this.ARQuizFinished.CheckIfQuizFinished();

        this.SelectedChoices[this.CurrentQuestionNumber - 1] = "B";
    }

    private void ChoiceCClicked()
    {
        if (this.thisAnswered) { return; }
        this.thisAnswered = true;
        this.SelectedC = true;

        this.ARQuizFunctions.ShowRightAnswer();
        this.ARQuizFinished.CheckIfQuizFinished();

        this.SelectedChoices[this.CurrentQuestionNumber - 1] = "C";
    }

    private void ChoiceDClicked()
    {
        if (this.thisAnswered) { return; }
        this.thisAnswered = true;
        this.SelectedD = true;

        this.ARQuizFunctions.ShowRightAnswer();
        this.ARQuizFinished.CheckIfQuizFinished();

        this.SelectedChoices[this.CurrentQuestionNumber - 1] = "D";
    }

    private void NextQuestionClicked()
    {
        if (this.thisQuizDone) { return; }
        if (!this.thisAnswered && !this.timeIsUp) { return; }
        this.thisAnswered = false;
        this.timeIsUp = false;
        ResetTimer();
        this.ARQuizQuestionaire.ResetQuestions();
        this.ARQuizFunctions.ResetButtonColors();
    }

    private IEnumerator TimerCounter()
    {
        while(this.currentTime < quizTime)
        {
            if (this.thisAnswered)
            {
                yield break;
            }

            yield return new WaitForSeconds(1f);
            this.currentTime++;
            this.QuizTimerSlider.value = currentTime;
            UpdateTimerText();
        }

        this.timeIsUp = true;
        this.QuizTimerTMP.text = "00:00";
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        this.QuizTimerTMP.text = $"{minutes:00}:{seconds:00}";
    }

    private void ResetTimer()
    {
        if (this.timerCoroutine != null)
        {
            StopCoroutine(this.timerCoroutine);
        }

        this.currentTime = 0f;
        this.QuizTimerSlider.maxValue = quizTime;
        this.QuizTimerSlider.value = currentTime;

        UpdateTimerText();

        this.timerCoroutine = StartCoroutine(TimerCounter());
    }
}
