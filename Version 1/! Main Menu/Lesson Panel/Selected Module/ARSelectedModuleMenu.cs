using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARSelectedModuleMenu
{
    #region
    protected ARSelectedModuleMain ARSelectedModuleMain;

    public ARSelectedModuleMenu (ARSelectedModuleMain main)
    {
        ARSelectedModuleMain = main;
    }
    #endregion

    public void OpenMenu()
    {
        if (this.ARSelectedModuleMain.IsClicked) { return; }
        this.ARSelectedModuleMain.IsClicked = true;
        this.ARSelectedModuleMain.StartCoroutine(this.ARSelectedModuleMain.ResetClicked());

        this.ARSelectedModuleMain.thisMenuAnimator.SetTrigger("Open");

    }

    public void CloseMenu() 
    {
        if (this.ARSelectedModuleMain.IsClicked) { return; }
        this.ARSelectedModuleMain.IsClicked = true;
        this.ARSelectedModuleMain.StartCoroutine(this.ARSelectedModuleMain.ResetClicked());

        this.ARSelectedModuleMain.thisMenuAnimator.SetTrigger("Close");

    }

    public void ChangeTextSize(float value)
    {
        this.ARSelectedModuleMain.thisSliderText.text = "Adjust Text Size <" + value + ">";
        this.ARSelectedModuleMain.thisLessonContent.fontSize = value;
    }
}
