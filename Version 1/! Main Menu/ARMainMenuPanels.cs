using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARMainMenuPanels
{
    #region
    protected ARMainMenuMain ARMainMenuMain;
    public ARMainMenuPanels (ARMainMenuMain main)
    {
        ARMainMenuMain = main;
    }
    #endregion

    public void CreateDashboardPanel()
    {
        ARMainMenuMain.ResetCenterPanel();
    }

    public void CreateCoursesPanel()
    {
        ARMainMenuMain.ResetCenterPanel();

        GameObject create = ARMainMenuMain.Instantiate(this.ARMainMenuMain.CoursesPrefab);
        create.transform.SetParent(this.ARMainMenuMain.CenterPanelLoc.transform, false);
    }

    public void CreateCyberNewsPanel()
    {
        ARMainMenuMain.ResetCenterPanel();
    }

    public void CreateProfilePanel()
    {
        ARMainMenuMain.ResetCenterPanel();
    }

    public void CreateSettingsPanel()
    {
        ARMainMenuMain.ResetCenterPanel();
    }
}
