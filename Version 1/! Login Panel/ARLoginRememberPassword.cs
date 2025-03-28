using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARLoginRememberPassword
{
    #region
    protected ARLoginPanelMain ARLoginPanelMain;
    public ARLoginRememberPassword(ARLoginPanelMain main)
    {
        ARLoginPanelMain = main;
    }
    #endregion

    public void StoreSavedData(string email, string password, bool rememberMe)
    {
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Password", password);
        PlayerPrefs.SetInt("Remember", rememberMe ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void RetrieveData()
    {
        var savedData = RetrieveSavedData();

        if (savedData != null)
        {
            this.ARLoginPanelMain.EmailInputField.text = savedData.Value.Item1;
            this.ARLoginPanelMain.PasswordInputField.text = savedData.Value.Item2;
            this.ARLoginPanelMain.ARLoginFunctions.RememberMeClicked();
        }
    }

    public (string, string, bool)? RetrieveSavedData()
    {
        if (!PlayerPrefs.HasKey("Email") || !PlayerPrefs.HasKey("Password"))
        {
            return null;
        }

        string email = PlayerPrefs.GetString("Email", "");
        string password = PlayerPrefs.GetString("Password", "");
        bool rememberMe = PlayerPrefs.GetInt("Remember", 0) == 1;

        return (email, password, rememberMe);
    }
}
