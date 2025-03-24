using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARQuizFinished
{
    #region
    protected ARQuizStartMain ARQuizStartMain;
    public ARQuizFinished(ARQuizStartMain main)
    {
        ARQuizStartMain = main;
    }
    #endregion

    public void CheckIfQuizFinished()
    {
        if (this.ARQuizStartMain.CurrentQuestionNumber == this.ARQuizStartMain.TotalQuestionNumber)
        {
            this.ARQuizStartMain.NextQuestionButton.interactable = false;

            this.ARQuizStartMain.StartCoroutine(CreateFinishedPanel());
        }
    }

    private IEnumerator CreateFinishedPanel()
    {
        yield return new WaitForSeconds(5f);

        GameObject create = ARQuizStartMain.Instantiate(this.ARQuizStartMain.FinishedQuizPrefab);
        create.transform.SetParent(this.ARQuizStartMain.FinishedQuizLoc.transform, false);

        ARQuizSubmit script = create.GetComponent<ARQuizSubmit>();
        script.TotalScore = this.ARQuizStartMain.MaximumScore.ToString();
        script.CurrentScore = this.ARQuizStartMain.CurrentScore.ToString();
        script.SelectedChoices = this.ARQuizStartMain.SelectedChoices;
        script.CourseName = this.ARQuizStartMain.thisCourseName;

        ARQuizStartMain.Destroy(this.ARQuizStartMain.thisParentObject.gameObject, 0.5f);
    }
}
