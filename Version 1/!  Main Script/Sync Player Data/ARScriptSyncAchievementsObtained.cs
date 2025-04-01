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
public class ARScriptSyncAchievementsObtained
{
    #region
    protected ARScriptHolderMain ARScriptHolderMain;
    public ARScriptSyncAchievementsObtained(ARScriptHolderMain main)
    {
        ARScriptHolderMain = main;
    }
    #endregion

    public void SyncData()
    {
        this.ARScriptHolderMain.PlayerData.AchievementsObtained.Clear();
        string CurrentEmail = this.ARScriptHolderMain.PlayerData.User_Email;

        this.ARScriptHolderMain.ARScriptFunction.FirebaseFirestore.Collection("records").WhereEqualTo("email", CurrentEmail).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;

                ARScriptHolderMain.PlayerData.AchievementsObtained = new List<PlayerDataAchievementsObtained>();

                if (snapshot.Count > 0)
                {
                    foreach (DocumentSnapshot document in snapshot.Documents)
                    {
                        if (document.Exists)
                        {
                            Dictionary<string, object> DatabaseData = document.ToDictionary();

                            if (DatabaseData.ContainsKey("achievements") && DatabaseData["achievements"] is List<object> achiementsData)
                            {
                                foreach (var achievementslist in achiementsData)
                                {
                                    if (achievementslist is Dictionary<string, object> achievementsListData)
                                    {
                                        if (achievementsListData.ContainsKey("AchievementID") && achievementsListData["AchievementID"] is string AchievementID &&
                                        achievementsListData.ContainsKey("AchievementTitle") && achievementsListData["AchievementTitle"] is string AchievementTitle &&
                                        achievementsListData.ContainsKey("AchievementDescription") && achievementsListData["AchievementDescription"] is string AchievementDescription)
                                        {
                                            var AchievementData = new PlayerDataAchievementsObtained
                                            {
                                                AchievementID = AchievementID,
                                                AchievementTitle = AchievementTitle,
                                                AchievementDescription = AchievementDescription,
                                            };

                                            ARScriptHolderMain.PlayerData.AchievementsObtained.Add(AchievementData);
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
