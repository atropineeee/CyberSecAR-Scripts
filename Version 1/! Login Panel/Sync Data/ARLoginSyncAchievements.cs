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
public class ARLoginSyncAchievements
{
    #region
    protected ARLoginPanelMain ARLoginPanelMain;
    public ARLoginSyncAchievements(ARLoginPanelMain main)
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

    private void SyncData()
    {
        this.ARLoginPanelMain.AchievementsData.AchievementsList.Clear();
        FirebaseFirestore.Collection("achievements").GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;

                this.ARLoginPanelMain.AchievementsData.AchievementsList = new List<AchievementInfo>();

                foreach (DocumentSnapshot document in snapshot.Documents) 
                {
                    Dictionary<string, object> DatabaseData = document.ToDictionary();

                    AchievementInfo achInf = new AchievementInfo
                    {
                        AchievementID = DatabaseData.ContainsKey("id") ? DatabaseData["id"].ToString() : "",
                        AchievementTitle = DatabaseData.ContainsKey("title") ? DatabaseData["title"].ToString() : "",
                        AchievementDescription = DatabaseData.ContainsKey("description") ? DatabaseData["description"].ToString() : "",
                    };

                    this.ARLoginPanelMain.AchievementsData.AchievementsList.Add(achInf);
                }
            }
        });
    }
}
