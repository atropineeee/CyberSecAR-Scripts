using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Quiz Data", menuName = "Quiz Data / Create New Quiz Data")]
public class QuizesSO : ScriptableObject
{
    public List<QuizInfo> QuizList;
}
