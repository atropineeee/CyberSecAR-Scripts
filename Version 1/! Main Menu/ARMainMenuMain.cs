using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARMainMenuMain : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] protected GameObject thisParentObject;

    [Header("Top Panel")]
    [SerializeField] protected Button TopPanelButton;
    [SerializeField] protected TMP_Text TopPanelLabel;

    [SerializeField] public GameObject CenterPanelLoc;
    [SerializeField] public GameObject NonSafeAreaPanel;
    [SerializeField] public GameObject SafeAreaPanel;

    [Header("Main Panel Prefabs")]
    [SerializeField] public GameObject DashboardPrefab;
    [SerializeField] public GameObject CoursesPrefab;
    [SerializeField] public GameObject CyberNewsPrefab;
    [SerializeField] public GameObject ProfilePrefab;
    [SerializeField] public GameObject SettingsPrefab;

    [Header("Side Panel Prefab")]
    [SerializeField] protected GameObject SidePanelPrefab;

    [SerializeField] protected bool IsClicked = false;

    [SerializeField] public ARMainMenuPanels ARMainMenuPanels;

    private void Start()
    {
        this.thisParentObject = this.gameObject;

        this.TopPanelButton = this.thisParentObject.transform.Find("MainMenu_TopPanel/MainMenu_SideButton").GetComponent<Button>();
        this.TopPanelLabel = this.thisParentObject.transform.Find("MainMenu_TopPanel/MainMenu_LabelTMP").GetComponent<TMP_Text>();

        this.CenterPanelLoc = this.thisParentObject.transform.Find("MainMenu_CenterPanel").gameObject;
        this.NonSafeAreaPanel = GameObject.Find("NonSafeAreaPanel");
        this.SafeAreaPanel = GameObject.Find("SafeAreaPanel");

        this.CoursesPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Courses Panel/CoursesMenu_Panel");

        this.SidePanelPrefab = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Side Panel/SideMenu_Panel");

        this.ARMainMenuPanels = new ARMainMenuPanels(this);

        this.TopPanelButton.onClick.AddListener(MenuClicked);
    }

    private void MenuClicked()
    {
        if (this.IsClicked) { return; }
        this.IsClicked = true;
        StartCoroutine(ResetClicked());

        GameObject create = Instantiate(this.SidePanelPrefab);
        create.transform.SetParent(this.SafeAreaPanel.transform, false);
        create.name = "SideMenu_Panel";
    }

    public void ResetSafeAreaPanel()
    {
        foreach (Transform child in this.SafeAreaPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetNonSafeAreaPanel()
    {
        foreach (Transform child in this.NonSafeAreaPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetCenterPanel()
    {
        foreach (Transform child in this.CenterPanelLoc.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangeTopLabel(string label)
    {
        this.TopPanelLabel.text = label;
    }

    private IEnumerator ResetClicked()
    {
        yield return new WaitForSeconds(1.5f);
        this.IsClicked = false;
    }
}
