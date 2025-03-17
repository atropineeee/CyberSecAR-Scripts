using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARScriptHolderMain : MonoBehaviour
{
    [Header("Required Scripts")]
    [SerializeField] public ARScriptFunction ARScriptFunction;
    [SerializeField] public ARScriptPanel ARScriptPanel;
    [SerializeField] public ARScriptSyncFinishedQuiz ARScriptSyncFinishedQuiz;
    [SerializeField] public ARScriptSyncFinishedCourse ARScriptSyncFinishedCourse;
    [SerializeField] public ARScriptSyncAchievementsObtained ARScriptSyncAchievementsObtained;

    [Header("Player Data")]
    public PlayerDataSO PlayerData;
    public QuizesSO QuizesData;
    public ModulesSO ModulesData;
    public AchievementsSO AchievementsData;

    private void Start()
    {
        GetRequiredComponents();
        StartRequiredComponents();
    }

    private void GetRequiredComponents()
    {
        this.ARScriptFunction = new ARScriptFunction(this);
        this.ARScriptPanel = new ARScriptPanel(this);
        this.ARScriptSyncFinishedQuiz = new ARScriptSyncFinishedQuiz(this);
        this.ARScriptSyncFinishedCourse = new ARScriptSyncFinishedCourse(this);
        this.ARScriptSyncAchievementsObtained = new ARScriptSyncAchievementsObtained(this);

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.QuizesData = Resources.Load<QuizesSO>("! Scriptable Objects/Quizes Data/QuizData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");
        this.AchievementsData = Resources.Load<AchievementsSO>("! Scriptable Objects/Achievements Data/AchievementsData");
    }

    private void StartRequiredComponents()
    {
        ARScriptFunction.Start();
        ARScriptPanel.Start();
    }
}
