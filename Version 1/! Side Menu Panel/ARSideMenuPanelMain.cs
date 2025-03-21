using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ARSideMenuPanelMain : MonoBehaviour, IPointerClickHandler
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected Animator Animator;

    [Header("Side Panel Buttons")]
    [SerializeField] protected Button DashboardButton;
    [SerializeField] protected Button CoursesButton;
    [SerializeField] protected Button CyberNewsButton;
    [SerializeField] protected Button ProfileButton;
    [SerializeField] protected Button SettingsButton;

    [Header("Side Panel Header")]
    [SerializeField] protected PlayerDataSO PlayerData;
    [SerializeField] protected TMP_Text NameTMP;

    [Header("Current Active Panel")]
    [SerializeField] protected ActivePanels CurrentActivePanel;

    private bool IsClicked = true;

    [SerializeField] private ARMainMenuMain ARMainMenuMain;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.Animator = GetComponent<Animator>();

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");

        this.DashboardButton = this.thisParentObject.transform.Find("SideMenu_BG/SideMenu_Dashboard/SideMenu_BottomPanel/DashboardButton").GetComponent<Button>();
        this.CoursesButton = this.thisParentObject.transform.Find("SideMenu_BG/SideMenu_Dashboard/SideMenu_BottomPanel/CoursesButton").GetComponent<Button>();
        this.CyberNewsButton = this.thisParentObject.transform.Find("SideMenu_BG/SideMenu_Dashboard/SideMenu_BottomPanel/CyberNewsButton").GetComponent<Button>();
        this.ProfileButton = this.thisParentObject.transform.Find("SideMenu_BG/SideMenu_Dashboard/SideMenu_BottomPanel/ProfileButton").GetComponent<Button>();
        this.SettingsButton = this.thisParentObject.transform.Find("SideMenu_BG/SideMenu_Dashboard/SideMenu_BottomPanel/SettingsButton").GetComponent<Button>();

        this.NameTMP = this.thisParentObject.transform.Find("SideMenu_BG/SideMenu_Dashboard/SideMenu_TopPanel/SideMenu_TopDisplayName").GetComponent<TMP_Text>();
        this.NameTMP.text = this.PlayerData.User_FullName;

        StartCoroutine(ResetClick());

        this.DashboardButton.onClick.AddListener(DashboardClicked);
        this.CoursesButton.onClick.AddListener(CoursesClicked);
        this.CyberNewsButton.onClick.AddListener(CyberNewsClicked);
        this.ProfileButton.onClick.AddListener(ProfileClicked);
        this.SettingsButton.onClick.AddListener(SettingsClicked);

        this.ARMainMenuMain = GameObject.Find("MainMenu_Panel").GetComponent<ARMainMenuMain>();

        this.CurrentActivePanel = this.ARMainMenuMain.CurrentActivePanel;
    }

    public void DashboardClicked()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;

        if (this.CurrentActivePanel == ActivePanels.Dashboard)
        {
            this.Animator.SetTrigger("Close");
            Destroy(this.thisParentObject, 0.6f);
            return;
        }

        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.Dashboard;
        this.ARMainMenuMain.ChangeTopLabel("Dashboard");

        // Create Dashboard Panel
        this.ARMainMenuMain.ARMainMenuPanels.CreateDashboardPanel();

        this.Animator.SetTrigger("Close");
        Destroy(this.thisParentObject, 0.6f);
    }

    public void CoursesClicked()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;

        if (this.CurrentActivePanel == ActivePanels.Courses)
        {
            this.Animator.SetTrigger("Close");
            Destroy(this.thisParentObject, 0.6f);
            return;
        }

        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.Courses;
        this.ARMainMenuMain.ChangeTopLabel("Courses");

        // Create Courses Panel
        this.ARMainMenuMain.ARMainMenuPanels.CreateCoursesPanel();

        this.Animator.SetTrigger("Close");
        Destroy(this.thisParentObject, 0.6f);
    }

    public void CyberNewsClicked()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;

        if (this.CurrentActivePanel == ActivePanels.CyberNews)
        {
            this.Animator.SetTrigger("Close");
            Destroy(this.thisParentObject, 0.6f);
            return;
        }

        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.CyberNews;
        this.ARMainMenuMain.ChangeTopLabel("CyberNews");

        this.Animator.SetTrigger("Close");
        Destroy(this.thisParentObject, 0.6f);
    }

    public void ProfileClicked()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;

        if (this.CurrentActivePanel == ActivePanels.Profile)
        {
            this.Animator.SetTrigger("Close");
            Destroy(this.thisParentObject, 0.6f);
            return;
        }

        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.Profile;
        this.ARMainMenuMain.ChangeTopLabel("Profile");

        this.Animator.SetTrigger("Close");
        Destroy(this.thisParentObject, 0.6f);
    }

    public void SettingsClicked()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;

        if (this.CurrentActivePanel == ActivePanels.Settings)
        {
            this.Animator.SetTrigger("Close");
            Destroy(this.thisParentObject, 0.6f);
            return;
        }

        this.CurrentActivePanel = ActivePanels.Settings;
        this.ARMainMenuMain.ChangeTopLabel("Settings");

        this.Animator.SetTrigger("Close");
        Destroy(this.thisParentObject, 0.6f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;

        this.Animator.SetTrigger("Close");
        Destroy(this.thisParentObject, 0.6f);
    }

    private IEnumerator ResetClick()
    {
        yield return new WaitForSeconds(0.9f);
        this.IsClicked = false;
    }
}

public enum ActivePanels
{
    Dashboard, Courses, CyberNews, Profile, Settings
}
