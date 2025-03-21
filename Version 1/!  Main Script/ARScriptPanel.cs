using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARScriptPanel
{
    #region
    protected ARScriptHolderMain ARScriptHolderMain;
    public ARScriptPanel (ARScriptHolderMain main)
    {
        ARScriptHolderMain = main;
    }
    #endregion

    [Header("Prefab Objects")]
    [SerializeField] protected GameObject AR_SignInPanel;
    [SerializeField] protected GameObject AR_MainMenuPanel;

    [Header("Built-In Objects")]
    [SerializeField] protected GameObject AR_Camera;
    [SerializeField] protected GameObject AR_BlackBG;
    [SerializeField] protected GameObject AR_TexturedBG;

    [Header("Prefab Objects Location")]
    [SerializeField] protected GameObject AR_NonSafeAreaPanel;
    [SerializeField] protected GameObject AR_SafeAreaPanel;

    public void Start()
    {
        // Prefab Objects
        AR_SignInPanel = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Login Panel/SignIn_Panel");
        AR_MainMenuPanel = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/MainMenu_Panel");

        // Built-In Objects
        AR_Camera = GameObject.Find("ARCamera");
        AR_BlackBG = GameObject.Find("BlackBG");
        AR_TexturedBG = GameObject.Find("TexturedBG");

        // Prefab Locations
        AR_NonSafeAreaPanel = GameObject.Find("NonSafeAreaPanel");
        AR_SafeAreaPanel = GameObject.Find("SafeAreaPanel");

        OpenSignInPanel();
        CloseCamera();
    }

    public void OpenSignInPanel()
    {
        ResetPanels();

        GameObject create = ARScriptHolderMain.Instantiate(AR_SignInPanel);
        create.transform.SetParent(this.AR_SafeAreaPanel.transform, false);
        create.name = "SignIn_Panel";
    }

    public void OpenMainMenuPanel()
    {
        ResetPanels();

        GameObject create = ARScriptHolderMain.Instantiate(AR_MainMenuPanel);
        create.transform.SetParent(this.AR_SafeAreaPanel.transform, false);
        create.name = "MainMenu_Panel";
    }

    public void OpenCamera()
    {
        AR_Camera.SetActive(true);
    }

    public void CloseCamera()
    {
        AR_Camera.SetActive(false);
    }

    private void ResetPanels()
    {
        foreach (Transform child in AR_NonSafeAreaPanel.transform)
        {
            ARScriptHolderMain.Destroy(child.gameObject);
        }

        foreach (Transform child in AR_SafeAreaPanel.transform)
        {
            ARScriptHolderMain.Destroy(child.gameObject);
        }
    }
}
