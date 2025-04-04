using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Extensions;
using System.Threading.Tasks;

[Serializable]
public class ARScriptSyncFinishedQuiz
{
    #region
    protected ARScriptHolderMain ARScriptHolderMain;
    public ARScriptSyncFinishedQuiz(ARScriptHolderMain main)
    {
        ARScriptHolderMain = main;
    }
    #endregion

    public void SyncData()
    {
        this.ARScriptHolderMain.PlayerData.FinishedQuizList.Clear();
        string CurrentEmail = this.ARScriptHolderMain.PlayerData.User_Email;

        this.ARScriptHolderMain.ARScriptFunction.FirebaseFirestore.Collection("records").WhereEqualTo("email", CurrentEmail).GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted) 
            {
                QuerySnapshot snapshot = task.Result;

                ARScriptHolderMain.PlayerData.FinishedQuizList = new List<PlayerDataFinishedQuiz>();

                if (snapshot.Count > 0)
                {
                    foreach (DocumentSnapshot document in snapshot.Documents)
                    {
                        if (document.Exists)
                        {
                            Dictionary<string, object> DatabaseData = document.ToDictionary();

                            if (DatabaseData.ContainsKey("scores") && DatabaseData["scores"] is List<object> scoresList)
                            {
                                foreach (var Quizlist in scoresList)
                                {
                                    if (Quizlist is Dictionary<string, object> quizData)
                                    {
                                        if (quizData.ContainsKey("CourseName") && quizData["CourseName"] != null)
                                        {
                                            PlayerDataFinishedQuiz SyncPlayerQuizes = new PlayerDataFinishedQuiz
                                            {
                                                CourseName = quizData.ContainsKey("CourseName") ? quizData["CourseName"].ToString() : "",
                                                QuizScore = quizData.ContainsKey("QuizScore") ? quizData["QuizScore"].ToString() : "",
                                                QuizQuestions = new List<PlayerDataFinishedQuizQNA>() 
                                            };

                                            if (quizData.ContainsKey("Questions") && quizData["Questions"] is List<object> quizQNAData)
                                            {
                                                foreach (var Question in quizQNAData)
                                                {
                                                    if (Question is Dictionary<string, object> questionData)
                                                    {
                                                        PlayerDataFinishedQuizQNA SyncPlayerQNA = new PlayerDataFinishedQuizQNA
                                                        {
                                                            Question = questionData.ContainsKey("Question") ? questionData["Question"].ToString() : "",
                                                            QuestionID = questionData.ContainsKey("QuestionID") ? questionData["QuestionID"].ToString() : "",
                                                            CurrentAnswer = questionData.ContainsKey("CurrentAnswer") ? questionData["CurrentAnswer"].ToString() : "",
                                                            QuestionChoices = new List<PlayerDataFinishedQuizQNAChoices>()
                                                        };

                                                        if (questionData.ContainsKey("Choices") && questionData["Choices"] is List <object> quizQNAChoicesData)
                                                        {
                                                            foreach (var Choices in quizQNAChoicesData)
                                                            {
                                                                if (Choices is Dictionary<string, object> choicesData)
                                                                {
                                                                    SyncPlayerQNA.QuestionChoices.Add(new PlayerDataFinishedQuizQNAChoices
                                                                    {
                                                                        IsCorrect = choicesData.ContainsKey("IsCorrect") ? choicesData["IsCorrect"].ToString() : "",
                                                                        Statement = choicesData.ContainsKey("Statement") ? choicesData["Statement"].ToString() : ""
                                                                    });
                                                                }
                                                            }
                                                        }

                                                        SyncPlayerQuizes.QuizQuestions.Add(SyncPlayerQNA);
                                                    }
                                                }
                                            }

                                            ARScriptHolderMain.PlayerData.FinishedQuizList.Add(SyncPlayerQuizes);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });
    }
}
