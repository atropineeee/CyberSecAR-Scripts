using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ARProfileMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected TMP_Text thisUserNameTMP;
    [SerializeField] protected TMP_Text thisUserEmailTMP;

    [Header("Sub Scripts")]
    [SerializeField] public ARProfileFunctions ARProfileFunctions;

    [Header("Contents")]
    [SerializeField] protected PlayerDataSO PlayerData;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisUserNameTMP = this.thisParentObject.transform.Find("ProfileTop_Panel/ProfileTop_Image/ProfileTop_UserName").GetComponent<TMP_Text>();
        this.thisUserEmailTMP = this.thisParentObject.transform.Find("ProfileTop_Panel/ProfileTop_Image/ProfileTop_UserEmail").GetComponent<TMP_Text>();

        this.PlayerData = Resources.Load<PlayerDataSO>("! Scriptable Objects/Player Data/PlayerData");

        this.ARProfileFunctions = new ARProfileFunctions(this);

        this.thisUserNameTMP.text = this.PlayerData.User_FullName;
        this.thisUserEmailTMP.text = this.PlayerData.User_Email;

        this.ARProfileFunctions.Start();
    }
}
