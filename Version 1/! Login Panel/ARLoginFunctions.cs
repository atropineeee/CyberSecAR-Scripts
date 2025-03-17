using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ARLoginFunctions
{
    #region
    protected ARLoginPanelMain ARLoginPanelMain;
    public ARLoginFunctions(ARLoginPanelMain main)
    {
        ARLoginPanelMain = main;
    }
    #endregion

    public void ResetPlayerData()
    {
        ARLoginPanelMain.PlayerData.User_Email = "";
        ARLoginPanelMain.PlayerData.User_Password = "";
        ARLoginPanelMain.PlayerData.User_FullName = "";

        ARLoginPanelMain.PlayerData.FinishedQuizList.Clear();
        ARLoginPanelMain.PlayerData.FinishedCourseList.Clear();
        ARLoginPanelMain.PlayerData.AchievementsObtained.Clear();
    }

    public void ResetAllData()
    {
        this.ARLoginPanelMain.QuizesData.QuizList.Clear();
        this.ARLoginPanelMain.ModulesData.ModuleList.Clear();
        this.ARLoginPanelMain.AchievementsData.AchievementsList.Clear();
    }

    public void RememberMeClicked()
    {
        if (!this.ARLoginPanelMain.rememberMe)
        {
            this.ARLoginPanelMain.rememberMe = true;   
            Sprite sprite = Resources.Load<Sprite>("Textures/Check1");
            this.ARLoginPanelMain.RememberMeImage.sprite = sprite;
        } 
        else
        {
            this.ARLoginPanelMain.rememberMe = false;
            Sprite sprite = Resources.Load<Sprite>("Textures/Check2");
            this.ARLoginPanelMain.RememberMeImage.sprite = sprite;
        }
    }

    public void ViewPassword()
    {
        if (!this.ARLoginPanelMain.showPass)
        {
            this.ARLoginPanelMain.showPass = true;
            // Show Password
            this.ARLoginPanelMain.PasswordInputField.contentType = InputField.ContentType.Standard;
            Sprite sprite = Resources.Load<Sprite>("Textures/Eye1");
            this.ARLoginPanelMain.ViewPassImage.sprite = sprite;
        } 
        else
        {
            ARLoginPanelMain.showPass = false;
            // Hide Password
            this.ARLoginPanelMain.PasswordInputField.contentType = InputField.ContentType.Password;
            Sprite sprite = Resources.Load<Sprite>("Textures/Eye2");
            this.ARLoginPanelMain.ViewPassImage.sprite = sprite;
        }

        this.ARLoginPanelMain.PasswordInputField.ForceLabelUpdate();
        this.ARLoginPanelMain.PasswordInputField.text = this.ARLoginPanelMain.PasswordInputField.text;
    }
}
