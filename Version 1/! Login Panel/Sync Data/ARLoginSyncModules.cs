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
public class ARLoginSyncModules
{
    #region
    protected ARLoginPanelMain ARLoginPanelMain;
    public ARLoginSyncModules(ARLoginPanelMain main)
    {
        ARLoginPanelMain = main;
    }
    #endregion

    public DependencyStatus FirebaseStatus;
    public FirebaseUser FirebaseUser;
    public FirebaseFirestore FirebaseFirestore;
    public DatabaseReference DatabaseReference;

    public IEnumerator SyncFirebase()
    {
        bool isDone = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            if (task.IsCompleted)
            {
                this.FirebaseFirestore = FirebaseFirestore.DefaultInstance;

                SyncData();

                isDone = true;
            }
        });

        yield return new WaitUntil(() => isDone);
    }

    public void SyncData()
    {
        this.ARLoginPanelMain.ModulesData.ModuleList.Clear();

        FirebaseFirestore.Collection("modules").GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted) 
            {
                QuerySnapshot snapshot = task.Result;

                this.ARLoginPanelMain.ModulesData.ModuleList = new List<ModuleInfo>();
                foreach (DocumentSnapshot document in snapshot.Documents) 
                {
                    if (document.Exists)
                    {
                        Dictionary<string, object> DatabaseData = document.ToDictionary();

                        ModuleInfo newModule = new ModuleInfo
                        {
                            ModuleNumber = DatabaseData.ContainsKey("moduleNumber") ? DatabaseData["moduleNumber"].ToString() : "",
                            ModuleName = DatabaseData.ContainsKey("title") ? DatabaseData["title"].ToString() : "",
                            ModuleDescription = DatabaseData.ContainsKey("description") ? DatabaseData["description"].ToString() : "",
                            ModuleLessons = new List<ModuleLessons>()
                        };

                        if (DatabaseData.ContainsKey("lessons") && DatabaseData["lessons"] is List<object> lessonData)
                        {
                            foreach(var Lessons in lessonData)
                            {
                                if (Lessons is Dictionary<string, object> lessonDict)
                                {
                                    newModule.ModuleLessons.Add(new ModuleLessons 
                                    {
                                        LessonID = lessonDict.ContainsKey("id") ? lessonDict["id"].ToString() : "",
                                        LessonTitle = lessonDict.ContainsKey("title") ? lessonDict["title"].ToString() : "",
                                        LessonContent = lessonDict.ContainsKey("content") ? lessonDict["content"].ToString() : "",
                                    });
                                }
                            }
                        }

                        this.ARLoginPanelMain.ModulesData.ModuleList.Add(newModule);
                    }
                }
            }
        });
    }
}
