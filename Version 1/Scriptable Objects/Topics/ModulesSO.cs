using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Module Data", menuName = "Topic Module Data / Create New Module Data")]
public class ModulesSO : ScriptableObject
{
    public List<ModuleInfo> ModuleList;
}
