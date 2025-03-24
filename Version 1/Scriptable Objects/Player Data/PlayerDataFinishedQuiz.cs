using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDataFinishedQuiz
{
    public string CourseName;
    public string QuizScore;
    public List<PlayerDataFinishedQuizQNA> QuizQuestions;
}

[Serializable]
public class PlayerDataFinishedQuizQNA
{
    public string Question;
    public string QuestionID;
    public string CurrentAnswer;
    public List<PlayerDataFinishedQuizQNAChoices> QuestionChoices;
}

[Serializable]
public class PlayerDataFinishedQuizQNAChoices
{
    public string IsCorrect;
    public string Statement;
}
