using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARSignInMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected Button thisSignInButton;

    [Header("Prefab Objects")]
    [SerializeField] protected GameObject AR_LoginPanel;

    [Header("Prefab Objects Location")]
    [SerializeField] protected GameObject AR_NonSafeAreaPanel;
    [SerializeField] protected GameObject AR_SafeAreaPanel;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisSignInButton = this.thisParentObject.transform.Find("SignIn_Button").GetComponent<Button>();

        // Prefab Objects
        this.AR_LoginPanel = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Login Panel/Login_Panel");

        // Prefab Locations
        this.AR_NonSafeAreaPanel = GameObject.Find("NonSafeAreaPanel");
        this.AR_SafeAreaPanel = GameObject.Find("SafeAreaPanel");

        this.thisSignInButton.onClick.AddListener(OpenLoginPanel);
    }

    private void OpenLoginPanel()
    {
        ResetPanels();

        GameObject create = ARScriptHolderMain.Instantiate(this.AR_LoginPanel);
        create.transform.SetParent(this.AR_SafeAreaPanel.transform, false);
        create.name = "Login_Panel";
    }

    private void ResetPanels()
    {
        foreach (Transform child in this.AR_NonSafeAreaPanel.transform)
        {
            ARScriptHolderMain.Destroy(child.gameObject);
        }

        foreach (Transform child in this.AR_SafeAreaPanel.transform)
        {
            ARScriptHolderMain.Destroy(child.gameObject);
        }
    }
}
