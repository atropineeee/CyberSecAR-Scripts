using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "Player Data / Create New Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Player Information")]
    public string User_Email;
    public string User_Password;
    public string User_FullName;

    [Header("Quiz Progress")]
    public List<PlayerDataFinishedQuiz> FinishedQuizList;
    public List<PlayerDataFinishedCourse> FinishedCourseList;  
    public List<PlayerDataAchievementsObtained> AchievementsObtained;  
}
