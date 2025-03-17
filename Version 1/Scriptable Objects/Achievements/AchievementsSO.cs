using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievements Data", menuName = "Achievements Data / Create New Achievements Data")]
public class AchievementsSO : ScriptableObject
{
    public List<AchievementInfo> AchievementsList;
}
