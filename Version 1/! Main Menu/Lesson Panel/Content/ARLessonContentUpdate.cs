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
            }
        });
    }

    public void SyncData()
    {
        FirebaseFirestore.Collection("records").WhereEqualTo("email", this.ARLessonContentMain.CurrentEmail).GetSnapshotAsync().ContinueWithOnMainThread(task => 
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

                    if (courseList.Any(course => course.ContainsKey("LessonID") && course["LessonID"].ToString() == this.ARLessonContentMain.LessonID)) { return; }

                    var CourseData = new Dictionary<string, object>
                    {
                        { "LessonID" , this.ARLessonContentMain.LessonID },
                        { "LessonContent", this.ARLessonContentMain.LessonContent },
                        { "CourseID",this.ARLessonContentMain.CourseID }
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
