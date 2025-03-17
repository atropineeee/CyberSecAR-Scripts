using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuizQNA
{
    public string QuestionID;
    public string QuestionName;
    public string CurrentAnswer;
    public List<QuizQNAChoice> QuestionChoices;
}
