using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Extensions;
using System.Linq;

[Serializable]
public class ARQuizSubmitSync
{
    #region
    protected ARQuizSubmit ARQuizSubmit;
    public ARQuizSubmitSync(ARQuizSubmit main)
    {
        ARQuizSubmit = main;
    }
    #endregion

    protected DependencyStatus FirebaseStatus;
    protected FirebaseUser FirebaseUser;
    protected FirebaseFirestore FirebaseFirestore;
    protected DatabaseReference DatabaseReference;

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
        string CurrentEmail = this.ARQuizSubmit.PlayerData.User_Email;
        string CurrentCourse = this.ARQuizSubmit.CourseName;
        string CurrentScore = this.ARQuizSubmit.CurrentScore;

        Debug.Log("HERE");

        FirebaseFirestore.Collection("records").WhereEqualTo("email", CurrentEmail).GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted) 
            {
                QuerySnapshot snapshot = task.Result;

                if (snapshot.Count > 0) 
                {
                    DocumentSnapshot document = snapshot.Documents.First();

                    List<Dictionary<string, object>> ScoresList = new List<Dictionary<string, object>>();
                    if (document.Exists && document.ContainsField("scores"))
                    {
                        ScoresList = document.GetValue<List<Dictionary<string, object>>>("scores") ?? new List<Dictionary<string, object>>();
                    }

                    var ExistingQuizData = ScoresList.FirstOrDefault(q => q.ContainsKey("CourseName") && q["CourseName"].ToString() == CurrentCourse);

                    var QuizData = new Dictionary<string, object>
                    {
                        { "CourseName", CurrentCourse },
                        { "QuizScore", CurrentScore },
                        { "Questions", new List<object>() }
                    };

                    var QuestionList = new List<object>();

                    foreach (var QuizInfo in this.ARQuizSubmit.QuizesData.QuizList)
                    {
                        if (QuizInfo.QuizTopicName == CurrentCourse)
                        {
                            foreach (var QuizQNA in QuizInfo.QuizQNAList)
                            {
                                int AnswerIndex = QuizInfo.QuizQNAList.IndexOf(QuizQNA);

                                var QuestionData = new Dictionary<string, object>
                                {
                                    { "Question", QuizQNA.QuestionName },
                                    { "QuestionID", QuizQNA.QuestionID },
                                    { "CurrentAnswer", this.ARQuizSubmit.SelectedChoices[AnswerIndex] },
                                    { "Choices", new List<object>() }
                                };

                                var ChoiceList = new List<object>();
                                foreach (var QuestionChoices in QuizQNA.QuestionChoices)
                                {
                                    var ChoiceData = new Dictionary<string, object>
                                    {
                                        { "IsCorrect", QuestionChoices.IsCorrect },
                                        { "Statement", QuestionChoices.Statement },
                                    };

                                    ChoiceList.Add(ChoiceData);
                                }

                                QuestionData["Choices"] = ChoiceList;

                                QuestionList.Add(QuestionData);
                            }
                        }
                        else
                        {
                            Debug.Log("NO Course Found THE SAME!");
                        }
                    }

                    QuizData["Questions"] = QuestionList;

                    if (ExistingQuizData != null)
                    {
                        ScoresList.Remove(ExistingQuizData);
                    }

                    ScoresList.Add(QuizData);

                    var ScoreData = new Dictionary<string, object>
                    {
                        { "scores", ScoresList },
                    };

                    document.Reference.UpdateAsync(ScoreData);

                }
            }
        });
    }
}
