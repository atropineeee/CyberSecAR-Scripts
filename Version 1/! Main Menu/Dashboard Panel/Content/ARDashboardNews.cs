using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARDashboardNews : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected TMP_Text thisNewsTitle;
    [SerializeField] protected TMP_Text thisNewsDescription;
    [SerializeField] protected Image thisNewsImage;
    [SerializeField] protected Button thisReadmoreButton;

    [SerializeField] protected ARMainMenuMain ARMainMenuMain;

    [Header("Contents")]
    public string NewsTitle;
    public string NewsDescription;
    public Sprite NewsImage;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisNewsImage = this.thisParentObject.transform.Find("Dashboard_CyberNewsImg").GetComponent<Image>();
        this.thisNewsTitle = this.thisParentObject.transform.Find("Dashboard_CyberNewsImg/Dashboard_CyberNewsTitle").GetComponent<TMP_Text>();
        this.thisReadmoreButton = this.thisParentObject.transform.Find("Dashboard_CyberNewsReadMoreButton").GetComponent<Button>();

        this.ARMainMenuMain = GameObject.Find("MainMenu_Panel").GetComponent<ARMainMenuMain>();

        this.thisNewsTitle.text = this.NewsTitle;

        this.thisReadmoreButton.onClick.AddListener(OpenNewsList);
    }

    private void OpenNewsList()
    {
        this.ARMainMenuMain.ARMainMenuPanels.CreateCyberNewsPanel();
        this.ARMainMenuMain.CurrentActivePanel = ActivePanels.CyberNews;
        this.ARMainMenuMain.ChangeTopLabel("CyberNews");
    }
}
