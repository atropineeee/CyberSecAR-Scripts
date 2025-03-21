using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ARScriptFunction
{
    #region
    protected ARScriptHolderMain ARScriptHolderMain;
    public ARScriptFunction(ARScriptHolderMain main)
    {
        ARScriptHolderMain = main;
    }
    #endregion

    public DependencyStatus FirebaseStatus;
    public FirebaseUser FirebaseUser;
    public FirebaseFirestore FirebaseFirestore;
    public DatabaseReference DatabaseReference;
    public ListenerRegistration FirebaseListener;

    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            if (task.IsCompleted)
            {
                this.FirebaseFirestore = FirebaseFirestore.DefaultInstance;
                FirebaseUpdateListener();
            }
        });
    }

    protected void FirebaseUpdateListener()
    {
        FirebaseListener = FirebaseFirestore.Collection("records").Listen(snapshot =>
        {
            foreach (DocumentChange change in snapshot.GetChanges()) 
            {
                this.ARScriptHolderMain.StartCoroutine(this.ARScriptHolderMain.ARScriptSyncAchievementsObtained.SyncFirebase());
                this.ARScriptHolderMain.StartCoroutine(this.ARScriptHolderMain.ARScriptSyncFinishedCourse.SyncFirebase());
                this.ARScriptHolderMain.StartCoroutine(this.ARScriptHolderMain.ARScriptSyncFinishedQuiz.SyncFirebase());
            }
        });
    }
}
