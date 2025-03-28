using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New News Data", menuName = "News Data / Create New News Data")]
public class NewsSO : ScriptableObject
{
    public List<NewsInfo> NewsList;
}
