using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ARProfileFunctions
{
    #region
    protected ARProfileMain ARProfileMain;
    public ARProfileFunctions (ARProfileMain main)
    {
        ARProfileMain = main;
    }
    #endregion

    [SerializeField] protected GameObject thisParentObject;

    bool isClicked = false;

    [Header("Buttons")]
    [SerializeField] protected Button RetunButton;
    [SerializeField] protected Button ChangePasswordButton;

    [Header("Panels")]
    [SerializeField] protected GameObject thisPanelLocation;
    [SerializeField] protected GameObject thisButtonLocation;
    [SerializeField] protected GameObject ChangePasswordPanel;
    public void Start()
    {
        this.thisParentObject = this.ARProfileMain.gameObject;
        this.thisPanelLocation = this.thisParentObject.transform.Find("ProfileCenter_Panel/ProfileCenterCenter_Panel/ProfileCenterClickable_Holder/ProfileCenterClickable_ScrollRect/ProfileCenterClickable_ScrollList").gameObject;
        this.thisButtonLocation = this.thisParentObject.transform.Find("ProfileCenter_Panel/ProfileCenterCenter_Panel/ProfileCenterButton_Holder").gameObject;
        
        this.RetunButton = this.thisParentObject.transform.Find("ProfileCenter_Panel/ProfileCenterTop_Panel/ProfileReturnButton").GetComponent<Button>();
        this.ChangePasswordButton = this.thisParentObject.transform.Find("ProfileCenter_Panel/ProfileCenterCenter_Panel/ProfileCenterButton_Holder/ProfileCenterButton_ScrollRect/ProfileCenterButton_ScrollList/ChangePasswordButton").GetComponent<Button>();

        this.ChangePasswordPanel = Resources.Load<GameObject>("! Panel Prefabs/Safe Area Panels/Main Menu Panel/Profile Panel/Panels/ChangePassword");   
        
        this.RetunButton.gameObject.SetActive(false);

        this.RetunButton.onClick.AddListener(ClosePanel);
        this.ChangePasswordButton.onClick.AddListener(OpenCPPanel);
    }

    public void OpenCPPanel()
    {
        if (this.isClicked) { return; }
        this.isClicked = true;
        this.ARProfileMain.StartCoroutine(ResetClicked());

        GameObject create = ARProfileMain.Instantiate(this.ChangePasswordPanel);
        create.transform.SetParent(this.thisPanelLocation.transform, false);

        this.thisButtonLocation.SetActive(false);
        this.RetunButton.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        foreach (Transform child in this.thisPanelLocation.transform) 
        {
            ARProfileMain.Destroy(child.gameObject);
        }

        this.thisButtonLocation.SetActive(true);
        this.RetunButton.gameObject.SetActive(false);
    }

    public IEnumerator ResetClicked()
    {
        yield return new WaitForSeconds(1f);
        this.isClicked = false;
    }
}
