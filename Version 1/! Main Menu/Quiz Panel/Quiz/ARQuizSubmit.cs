using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARQuizSubmit : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObejct;
    [SerializeField] protected TMP_Text NameTMP;
    [SerializeField] protected TMP_Text ScoreTMP;

    [SerializeField] protected Button ContinueButton;
    [SerializeField] protected Button RetakeButton;

    [SerializeField] public ARQuizSubmitSync ARQuizSubmitSync;

    [Header("Contents")]
    public PlayerDataSO PlayerData;
    public QuizesSO QuizesData;

    public string[] SelectedChoices;
    public string CourseName;
    public string CurrentScore;
    public string TotalScore;

    private void Start()
    {
        this.thisParentObejct = this.gameObject;
        this.NameTMP = this.thisParentObejct.transform.Find("QuizFinished_SCRect/QuizFinished_SCList/QuizFinished_Holder/QuizFinished_Badge/QuizFinished_NameTMP").GetComponent<TMP_Text>();
        this.ScoreTMP = this.thisParentObejct.transform.Find("QuizFinished_SCRect/QuizFinished_SCList/QuizFinished_Holder/QuizFinished_Badge/QuizFinished_ScoreTMP").GetComponent<TMP_Text>();
        this.ContinueButton = this.thisParentObejct.transform.Find("QuizFinished_SCRect/QuizFinished_SCList/QuizFinished_Holder/QuizFinished_Badge/QuizContinue_Button").GetComponent<Button>();
        this.RetakeButton = this.thisParentObejct.transform.Find("QuizFinished_SCRect/QuizFinished_SCList/QuizFinished_Holder/QuizFinished_Badge/QuizRetake_Button").GetComponent<Button>();

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.QuizesData = Resources.Load<QuizesSO>("! Scriptable Objects/Quizes Data/QuizData");

        this.ARQuizSubmitSync = new ARQuizSubmitSync(this);

        this.NameTMP.text = this.PlayerData.User_FullName + "!";
        this.ScoreTMP.text = $"Great job! You scored {this.CurrentScore}/{this.TotalScore}, proving your cybersecurity skills are on point. Keep up the great work and continue learning!";

        this.ARQuizSubmitSync.SyncFirebase();
    }
}
