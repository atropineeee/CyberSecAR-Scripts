using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARQuizFunctions
{
    #region
    protected ARQuizStartMain ARQuizStartMain;
    public ARQuizFunctions(ARQuizStartMain main)
    {
        ARQuizStartMain = main;
    }
    #endregion

    public void ShowRightAnswer()
    {
        if (this.ARQuizStartMain.SelectedA)
        {
            this.ARQuizStartMain.ChoiceAImage.color = new Color32(255, 191, 191, 255);

            if (this.ARQuizStartMain.ChoiceACorrect)
            {
                this.ARQuizStartMain.ChoiceAImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CurrentScore++;
            }

            if (this.ARQuizStartMain.ChoiceBCorrect)
            {
                this.ARQuizStartMain.ChoiceBImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is B";
            }

            if (this.ARQuizStartMain.ChoiceCCorrect)
            {
                this.ARQuizStartMain.ChoiceCImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is C";
            }

            if (this.ARQuizStartMain.ChoiceDCorrect)
            {
                this.ARQuizStartMain.ChoiceDImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is D";
            }
        }

        if (this.ARQuizStartMain.SelectedB)
        {
            this.ARQuizStartMain.ChoiceBImage.color = new Color32(255, 191, 191, 255);

            if (this.ARQuizStartMain.ChoiceACorrect)
            {
                this.ARQuizStartMain.ChoiceAImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is A";
            }

            if (this.ARQuizStartMain.ChoiceBCorrect)
            {
                this.ARQuizStartMain.ChoiceBImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CurrentScore++;
            }

            if (this.ARQuizStartMain.ChoiceCCorrect)
            {
                this.ARQuizStartMain.ChoiceCImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is C";
            }

            if (this.ARQuizStartMain.ChoiceDCorrect)
            {
                this.ARQuizStartMain.ChoiceDImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is D";
            }
        }

        if (this.ARQuizStartMain.SelectedC)
        {
            this.ARQuizStartMain.ChoiceCImage.color = new Color32(255, 191, 191, 255);

            if (this.ARQuizStartMain.ChoiceACorrect)
            {
                this.ARQuizStartMain.ChoiceAImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is A";
            }

            if (this.ARQuizStartMain.ChoiceBCorrect)
            {
                this.ARQuizStartMain.ChoiceBImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is B";
            }

            if (this.ARQuizStartMain.ChoiceCCorrect)
            {
                this.ARQuizStartMain.ChoiceCImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CurrentScore++;
            }

            if (this.ARQuizStartMain.ChoiceDCorrect)
            {
                this.ARQuizStartMain.ChoiceDImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is D";
            }
        }

        if (this.ARQuizStartMain.SelectedD)
        {
            this.ARQuizStartMain.ChoiceDImage.color = new Color32(255, 191, 191, 255);

            if (this.ARQuizStartMain.ChoiceACorrect)
            {
                this.ARQuizStartMain.ChoiceAImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is A";
            }

            if (this.ARQuizStartMain.ChoiceBCorrect)
            {
                this.ARQuizStartMain.ChoiceBImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is B";
            }

            if (this.ARQuizStartMain.ChoiceCCorrect)
            {
                this.ARQuizStartMain.ChoiceCImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CorrectTMP.text = "The Correct Answer is C";
            }

            if (this.ARQuizStartMain.ChoiceDCorrect)
            {
                this.ARQuizStartMain.ChoiceDImage.color = new Color32(191, 255, 240, 255);
                this.ARQuizStartMain.CurrentScore++;
            }
        }
    }

    public void ResetButtonColors()
    {
        this.ARQuizStartMain.ChoiceAImage.color = new Color32(255, 255, 0, 255);
        this.ARQuizStartMain.ChoiceBImage.color = new Color32(255, 255, 0, 255);
        this.ARQuizStartMain.ChoiceCImage.color = new Color32(255, 255, 0, 255);
        this.ARQuizStartMain.ChoiceDImage.color = new Color32(255, 255, 0, 255);

        this.ARQuizStartMain.CorrectTMP.text = "";
    }
}
