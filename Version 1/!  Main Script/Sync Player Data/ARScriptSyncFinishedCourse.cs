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
public class ARScriptSyncFinishedCourse
{
    #region
    protected ARScriptHolderMain ARScriptHolderMain;
    public ARScriptSyncFinishedCourse(ARScriptHolderMain main)
    {
        ARScriptHolderMain = main;
    }
    #endregion
    public void SyncData()
    {

        this.ARScriptHolderMain.PlayerData.FinishedCourseList.Clear();
        string CurrentEmail = this.ARScriptHolderMain.PlayerData.User_Email;

        this.ARScriptHolderMain.ARScriptFunction.FirebaseFirestore.Collection("records").WhereEqualTo("email", CurrentEmail).GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted) 
            {
                QuerySnapshot snapshot = task.Result;

                ARScriptHolderMain.PlayerData.FinishedCourseList = new List<PlayerDataFinishedCourse>();

                if (snapshot.Count > 0)
                {
                    foreach (DocumentSnapshot document in snapshot.Documents)
                    {
                        if (document.Exists)
                        {
                            Dictionary<string, object> DatabaseData = document.ToDictionary();

                            if (DatabaseData.ContainsKey("course") && DatabaseData["course"] is List<object> courseList)
                            {
                                foreach (var clist in courseList) 
                                {
                                    if (clist is Dictionary<string, object> courseData) 
                                    {
                                        if (courseData.ContainsKey("LessonID") && courseData["LessonID"] is string lessonID &&
                                        courseData.ContainsKey("LessonContent") && courseData["LessonContent"] is string lessonContent &&
                                        courseData.ContainsKey("CourseID") && courseData["CourseID"] is string courseID)
                                        {
                                            var CoursesData = new PlayerDataFinishedCourse
                                            {
                                                CourseID = courseID,
                                                LessonContent = lessonContent,
                                                LessonID = lessonID,
                                            };

                                            ARScriptHolderMain.PlayerData.FinishedCourseList.Add(CoursesData);
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
    
