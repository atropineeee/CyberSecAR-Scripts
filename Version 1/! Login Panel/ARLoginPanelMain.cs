using Firebase.Firestore;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using System;
using Firebase.Extensions;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class ARLoginPanelMain : MonoBehaviour
{
    [Header("Sub Scripts")]
    public ARLoginFunctions ARLoginFunctions;
    public ARLoginSyncModules ARLoginSyncModules;
    public ARLoginSyncQuiz ARLoginSyncQuiz;
    public ARLoginSyncAchievements ARLoginSyncAchievements;
    public ARLoginRememberPassword ARLoginRememberPassword;

    [Header("Firease Connection")]
    public DependencyStatus FirebaseStatus;
    public FirebaseUser FirebaseUser;
    public FirebaseFirestore FirebaseFirestore;
    public DatabaseReference DatabaseReference;
    public Animator Animator;

    [Header("Scriptable Objects")]
    public PlayerDataSO PlayerData;
    public QuizesSO QuizesData;
    public ModulesSO ModulesData;
    public AchievementsSO AchievementsData;

    [Header("GameObjects")]
    public GameObject ParentObject;
    public InputField EmailInputField;
    public InputField PasswordInputField;
    public Button LoginButton;

    public Button RememberMeButton;
    public Image RememberMeImage;

    public Button ViewPassButton;
    public Image ViewPassImage;

    public bool tried = false;
    public bool showPass = false;
    public bool rememberMe = false;

    public GameObject WarningPanelLoc;
    public GameObject WarningPanelPrefab;

    [SerializeField] protected ARScriptHolderMain ARScriptHolderMain;

    private void Start()
    {
        GetRequiredComponents();
        StartRequiredComponents();
    }

    private void GetRequiredComponents()
    {
        // Required Components
        this.ARLoginFunctions = new ARLoginFunctions(this);
        this.ARLoginSyncModules = new ARLoginSyncModules(this);
        this.ARLoginSyncQuiz = new ARLoginSyncQuiz(this);
        this.ARLoginSyncAchievements = new ARLoginSyncAchievements(this);
        this.ARLoginRememberPassword = new ARLoginRememberPassword(this);

        this.Animator = GetComponent<Animator>();

        // Required Data
        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");
        this.QuizesData = Resources.Load<QuizesSO>("! Scriptable Objects/Quizes Data/QuizData");
        this.ModulesData = Resources.Load<ModulesSO>("! Scriptable Objects/Topics Data/TopicsData");
        this.AchievementsData = Resources.Load<AchievementsSO>("! Scriptable Objects/Achievements Data/AchievementsData");

        this.ParentObject = this.gameObject;
        this.EmailInputField = this.gameObject.transform.Find("Centered_Input/Centered_EmailInput").GetComponent<InputField>();
        this.PasswordInputField = this.gameObject.transform.Find("Centered_Input/Centered_PassInput").GetComponent<InputField>();
        this.LoginButton = this.gameObject.transform.Find("Centered_Input/Centered_LoginButton").GetComponent<Button>();
        this.RememberMeButton = this.gameObject.transform.Find("Centered_Input/Centered_RememberMe").GetComponent<Button>();
        this.RememberMeImage = this.gameObject.transform.Find("Centered_Input/Centered_RememberMe/Centered_RememberMeIMG").GetComponent<Image>();

        this.ViewPassButton = this.gameObject.transform.Find("Centered_Input/Centered_ViewPass").GetComponent<Button>();
        this.ViewPassImage = this.gameObject.transform.Find("Centered_Input/Centered_ViewPass").GetComponent<Image>();

        this.WarningPanelLoc = this.ParentObject.transform.Find("Centered_Error").gameObject;
        this.WarningPanelPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Login Panel/Centered_ErrorCode");

        this.ARScriptHolderMain = GameObject.Find("ScriptsHolder").GetComponent<ARScriptHolderMain>();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp App = FirebaseApp.DefaultInstance;

            if (task.IsCompleted)
            {
                this.FirebaseFirestore = FirebaseFirestore.DefaultInstance;
            }
        });

        this.LoginButton.onClick.AddListener(SyncFirebase);
        this.RememberMeButton.onClick.AddListener(this.ARLoginFunctions.RememberMeClicked);
        this.ViewPassButton.onClick.AddListener(this.ARLoginFunctions.ViewPassword);
    }

    private void StartRequiredComponents()
    {
        this.ARLoginFunctions.ResetPlayerData();
        this.ARLoginFunctions.ResetAllData();
        this.ARLoginRememberPassword.RetrieveData();
    }

    private void SyncFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp App = FirebaseApp.DefaultInstance;

            if (task.IsCompleted)
            {
                this.FirebaseFirestore = FirebaseFirestore.DefaultInstance;
                AttemptLogin();
            }
        });
    }

    private void AttemptLogin()
    {
        if (this.tried) { return; }
        tried = true;
        StartCoroutine(TriedCD());

        this.ARLoginFunctions.ResetPlayerData();

        string UserEmail = this.EmailInputField.text;
        string Password = this.PasswordInputField.text;

        FirebaseFirestore.Collection("users").WhereEqualTo("email", UserEmail).GetSnapshotAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted) 
            {
                QuerySnapshot snaphot = task.Result;

                if (snaphot.Count > 0) 
                {
                    foreach (DocumentSnapshot doc in snaphot.Documents)
                    {
                        Dictionary<string, object> UserData = doc.ToDictionary();

                        if (UserData.ContainsKey("email") && UserData.ContainsKey("password"))
                        {
                            string StoredEmail = UserData["email"].ToString();
                            string StoredPassword = UserData["password"].ToString();
                            string StoredName = UserData["name"].ToString();

                            if (UserEmail == StoredEmail && Password == StoredPassword) 
                            {
                                this.PlayerData.User_Email = StoredEmail;
                                this.PlayerData.User_Password = StoredPassword;
                                this.PlayerData.User_FullName = StoredName;

                                StartCoroutine(SyncAllData(StoredEmail, StoredName));
                                
                                if (this.rememberMe)
                                {
                                    this.ARLoginRememberPassword.StoreSavedData(UserEmail, Password, rememberMe);
                                }
                                else
                                {
                                    PlayerPrefs.DeleteAll();
                                    PlayerPrefs.Save();
                                }
                            }
                            else if (UserEmail == StoredEmail && Password != StoredPassword)
                            {
                                ShowWarning("Wrong Password");
                                ResetPassword();
                            }
                            else
                            {
                                ShowWarning("Invalid Email or Password!");
                                ResetPassword();
                            }
                        }
                        else 
                        {
                            Debug.Log("Warning 1");
                        }
                    }
                }
                else
                {
                    ShowWarning("No User Found!");
                }
            }
            else
            {
                Debug.Log("Warning 2");
            }
        });
    }

    private IEnumerator SyncAllData(string StoredEmail, string StoredName)
    {
        // Sync Normal Data
        yield return StartCoroutine(this.ARLoginSyncQuiz.SyncFirebase());
        yield return StartCoroutine(this.ARLoginSyncModules.SyncFirebase());
        yield return StartCoroutine(this.ARLoginSyncAchievements.SyncFirebase());

        // Sync Player Data
        this.ARScriptHolderMain.ARScriptSyncFinishedQuiz.SyncData();
        this.ARScriptHolderMain.ARScriptSyncFinishedCourse.SyncData();
        this.ARScriptHolderMain.ARScriptSyncAchievementsObtained.SyncData();

        this.ARScriptHolderMain.ARScriptPanel.OpenMainMenuPanel();

        AddUserToRecords(StoredEmail, StoredName);
        ResetEmail();
        ResetPassword();

        CloseThis();
    }

    private void AddUserToRecords(string DatabaseEmail, string DatabaeName)
    {
        FirebaseFirestore.DefaultInstance.Collection("records").WhereEqualTo("email", DatabaseEmail).GetSnapshotAsync().ContinueWith(task =>
        {
            var snapshot = task.Result;

            if (snapshot.Count > 0)
            {
                return;
            }

            var UserRecord = new
            {
                email = DatabaseEmail,
                name = DatabaeName,
                createdAt = FieldValue.ServerTimestamp,
            };

            FirebaseFirestore.Collection("records").AddAsync(UserRecord);
        });
    }

    private void CloseThis()
    {
        this.Animator.SetTrigger("Close");
        Destroy(this.gameObject, 2f);
    }

    private IEnumerator TriedCD()
    {
        yield return new WaitForSeconds(1.5f);
        tried = false;
    }

    private void ResetEmail()
    {
        this.EmailInputField.text = "";
    }

    private void ResetPassword()
    {
        this.PasswordInputField.text = "";
    }

    private void ShowWarning(string Message)
    {
        GameObject create = Instantiate(this.WarningPanelPrefab);
        create.transform.SetParent(this.WarningPanelLoc.transform, false);
        TMP_Text text = create.transform.Find("Centered_Text").GetComponent<TMP_Text>();
        text.text = Message;
        Destroy(create, 2f);
    }
}
