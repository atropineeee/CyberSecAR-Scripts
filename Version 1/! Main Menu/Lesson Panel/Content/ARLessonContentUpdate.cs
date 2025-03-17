using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using System.Linq;

[Serializable]
public class ARLessonContentUpdate
{
    #region
    protected ARLessonContentMain ARLessonContentMain;
    public ARLessonContentUpdate (ARLessonContentMain main)
    {
        ARLessonContentMain = main;
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
        string CurrentEmail = this.ARLessonContentMain.PlayerData.User_Email;
        string LessonID = this.ARLessonContentMain.thisModuleName;
        string LessonContent = this.ARLessonContentMain.thisModuleContent;
        string CourseID = this.ARLessonContentMain.thisCourseName;

        FirebaseFirestore.Collection("records").WhereEqualTo("email", CurrentEmail).GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;

                if (snapshot.Count > 0)
                {
                    DocumentSnapshot document = snapshot.Documents.First();

                    List<Dictionary<string, object>> courseList = new List<Dictionary<string, object>>();

                    if (document.Exists && document.ContainsField("course"))
                    {
                        courseList = document.GetValue<List<Dictionary<string, object>>>("course") ?? new List<Dictionary<string, object>>();
                    }

                    if (courseList.Any(course => course.ContainsKey("LessonID") && course["LessonID"].ToString() == LessonID)) { return; }

                    var CourseData = new Dictionary<string, object>
                    {
                        { "LessonID" , LessonID },
                        { "LessonContent", LessonContent },
                        { "CourseID", CourseID }
                    };

                    courseList.Add(CourseData);

                    var LessonProgress = new Dictionary<string, object>
                    {
                        { "course", courseList }
                    };

                    document.Reference.UpdateAsync(LessonProgress);
                }
            }
        });
    }
}
