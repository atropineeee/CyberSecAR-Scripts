using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;

[Serializable]
public class ARLoginSyncQuiz
{
    #region
    protected ARLoginPanelMain ARLoginPanelMain;
    public ARLoginSyncQuiz(ARLoginPanelMain main)
    {
        ARLoginPanelMain = main;
    }
    #endregion


    public DependencyStatus FirebaseStatus;
    public FirebaseUser FirebaseUser;
    public FirebaseFirestore FirebaseFirestore;
    public DatabaseReference DatabaseReference;

    public void SyncFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            if (task.IsCompleted)
            {
                this.FirebaseFirestore = FirebaseFirestore.DefaultInstance;
                SyncData();
            }
        });
    }

    private void SyncData()
    {
        this.ARLoginPanelMain.QuizesData.QuizList.Clear();

        FirebaseFirestore.Collection("assessments").GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;

                this.ARLoginPanelMain.QuizesData.QuizList = new List<QuizInfo>();

                foreach (DocumentSnapshot document in snapshot.Documents) 
                {
                    if (document.Exists)
                    {
                        Dictionary<string, object> DatabaseData = document.ToDictionary();

                        QuizInfo Quizinfo = new QuizInfo
                        {
                            QuizTopicName = DatabaseData.ContainsKey("module") ? DatabaseData["module"].ToString() : "",
                            QuizQNAList = new List<QuizQNA>()
                        };

                        if (DatabaseData.ContainsKey("questions") && DatabaseData["questions"] is List<object> QNAData)
                        {
                            foreach (var Question in QNAData)
                            {
                                if (Question is Dictionary<string, object> QuestionDict)
                                {
                                    QuizQNA quizQNA = new QuizQNA
                                    {
                                        QuestionID = QuestionDict.ContainsKey("id") ? QuestionDict["id"].ToString() : "",
                                        QuestionName = QuestionDict.ContainsKey("statement") ? QuestionDict["statement"].ToString() : "",
                                        QuestionChoices = new List<QuizQNAChoice>()   
                                    };

                                    if (QuestionDict.ContainsKey("choices") && QuestionDict["choices"] is List<object> ChoiceData)
                                    {
                                        foreach (var Choice in ChoiceData)
                                        {
                                            if (Choice is Dictionary<string, object> ChoiceDict)
                                            {
                                                quizQNA.QuestionChoices.Add(new QuizQNAChoice
                                                {
                                                    ChoiceID = ChoiceDict.ContainsKey("id") ? ChoiceDict["id"].ToString() : "",
                                                    IsCorrect = ChoiceDict.ContainsKey("isCorrect") ? ChoiceDict["isCorrect"].ToString() : "",
                                                    Statement = ChoiceDict.ContainsKey("statement") ? ChoiceDict["statement"].ToString() : "",
                                                });
                                            }
                                        }
                                    }

                                    Quizinfo.QuizQNAList.Add(quizQNA);
                                }
                            }
                        }

                        this.ARLoginPanelMain.QuizesData.QuizList.Add(Quizinfo);
                    }
                }
            }
        });
    }
}
