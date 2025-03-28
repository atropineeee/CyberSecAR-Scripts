using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARLocalMain : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected GameObject thisMainPanelObject;
    [SerializeField] protected GameObject thisInfoPanel;
    [SerializeField] protected GameObject thisRotatePanel;

    [SerializeField] protected Button BackButton;

    [SerializeField] protected Button InfoButton;
    [SerializeField] protected Button PlayButton;
    [SerializeField] protected Button PauseButton;
    [SerializeField] protected Button CloseButton;

    [SerializeField] public string CurrentCourse;
    [SerializeField] protected ARPanelRotator CurrentActiveObj;

    protected bool OpenInfo;

    protected ARScriptHolderMain ARScriptHolderMain;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisMainPanelObject = GameObject.Find("MainMenu_Panel").gameObject;
        this.thisInfoPanel = this.thisParentObject.transform.Find("ARInfo_Panel").gameObject;
        this.thisRotatePanel = this.thisParentObject.transform.Find("ARRotate_Anim").gameObject;

        this.BackButton = this.thisParentObject.transform.Find("ARInfo_Panel/ARInfo_BackButton").GetComponent<Button>();

        this.InfoButton = this.thisParentObject.transform.Find("ARPanel_InfoButton").GetComponent<Button>();
        this.PlayButton = this.thisParentObject.transform.Find("ARPanel_PlayButton").GetComponent<Button>();
        this.PauseButton = this.thisParentObject.transform.Find("ARPanel_PauseButton").GetComponent<Button>();
        this.CloseButton = this.thisParentObject.transform.Find("ARPanel_CloseButton").GetComponent<Button>();

        this.InfoButton.onClick.AddListener(OpenInfoPanel);
        this.BackButton.onClick.AddListener(CloseInfoPanel);

        this.CloseButton.onClick.AddListener(CloseThisPanel);

        this.thisInfoPanel.SetActive(false);

        this.ARScriptHolderMain = GameObject.Find("ScriptsHolder").GetComponent<ARScriptHolderMain>();

        StartCoroutine(CloseMainPanel());
        StartCoroutine(CloseRotatePanel());
        StartCoroutine(GetActive());
    }

    private IEnumerator GetActive()
    {
        yield return new WaitForSeconds(0.5f);
        this.CurrentActiveObj = GameObject.Find("AX_RULEFOUR").GetComponent<ARPanelRotator>();

        this.PlayButton.onClick.AddListener(this.CurrentActiveObj.PlayBtn);
        this.PauseButton.onClick.AddListener(this.CurrentActiveObj.PauseBtn);
    }


    private void OpenInfoPanel()
    {
        if (this.OpenInfo) { return; }
        this.OpenInfo = true;

        this.thisInfoPanel.SetActive(true);
    }

    private void CloseInfoPanel()
    {
        this.OpenInfo = false;
        this.thisInfoPanel.SetActive(false);
    }

    private void CloseThisPanel()
    {
        this.thisMainPanelObject.SetActive(true);
        this.ARScriptHolderMain.ARScriptPanel.CloseARPanel();
        Destroy(this.thisParentObject.gameObject, 0.25f);
    }

    private IEnumerator CloseRotatePanel()
    {
        yield return new WaitForSeconds(6.5f);
        this.thisRotatePanel.SetActive(false);
    }

    private IEnumerator CloseMainPanel()
    {
        yield return new WaitForSeconds(0.5f);
        this.thisMainPanelObject.SetActive(false);
    }
}
