using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARQuizQuestionaire
{
    #region
    protected ARQuizStartMain ARQuizStartMain;
    public ARQuizQuestionaire(ARQuizStartMain main)
    {
        ARQuizStartMain = main;
    }
    #endregion

    public void ResetQuestions()
    {
        this.ARQuizStartMain.CurrentQuestionNumber++;

        this.ARQuizStartMain.SelectedA = false;
        this.ARQuizStartMain.SelectedB = false;
        this.ARQuizStartMain.SelectedC = false;
        this.ARQuizStartMain.SelectedD = false;

        foreach (var QuizList in this.ARQuizStartMain.QuizesData.QuizList)
        {
            if (QuizList.QuizTopicName == this.ARQuizStartMain.thisCourseName)
            {
                this.ARQuizStartMain.TotalQuestionNumber = QuizList.QuizQNAList.Count;
                this.ARQuizStartMain.MaximumScore = this.ARQuizStartMain.TotalQuestionNumber;
                this.ARQuizStartMain.QuestionNumberTMP.text = "Question " + this.ARQuizStartMain.CurrentQuestionNumber + "/" + this.ARQuizStartMain.TotalQuestionNumber;
                foreach (var QuestionList in QuizList.QuizQNAList)
                {
                    if (QuestionList.QuestionID == this.ARQuizStartMain.CurrentQuestionNumber.ToString())
                    {
                        this.ARQuizStartMain.QuestionTMP.text = QuestionList.QuestionName;

                        foreach (var QuestionChoices in QuestionList.QuestionChoices)
                        {
                            if (QuestionChoices.ChoiceID == 1.ToString())
                            {
                                this.ARQuizStartMain.ChoiceAText.text = QuestionChoices.Statement;

                                if (QuestionChoices.IsCorrect == "True")
                                {
                                    this.ARQuizStartMain.ChoiceACorrect = true;
                                }
                                else
                                {
                                    this.ARQuizStartMain.ChoiceACorrect = false;
                                }
                            }

                            if (QuestionChoices.ChoiceID == 2.ToString())
                            {
                                this.ARQuizStartMain.ChoiceBText.text = QuestionChoices.Statement;

                                if (QuestionChoices.IsCorrect == "True")
                                {
                                    this.ARQuizStartMain.ChoiceBCorrect = true;
                                }
                                else
                                {
                                    this.ARQuizStartMain.ChoiceBCorrect = false;
                                }
                            }

                            if (QuestionChoices.ChoiceID == 3.ToString())
                            {
                                this.ARQuizStartMain.ChoiceCText.text = QuestionChoices.Statement;

                                if (QuestionChoices.IsCorrect == "True")
                                {
                                    this.ARQuizStartMain.ChoiceCCorrect = true;
                                }
                                else
                                {
                                    this.ARQuizStartMain.ChoiceCCorrect = false;
                                }
                            }

                            if (QuestionChoices.ChoiceID == 4.ToString())
                            {
                                this.ARQuizStartMain.ChoiceDText.text = QuestionChoices.Statement;

                                if (QuestionChoices.IsCorrect == "True")
                                {
                                    this.ARQuizStartMain.ChoiceDCorrect = true;
                                }
                                else
                                {
                                    this.ARQuizStartMain.ChoiceDCorrect = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
