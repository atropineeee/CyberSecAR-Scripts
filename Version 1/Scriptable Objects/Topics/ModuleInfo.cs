using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleInfo
{
    public string ModuleNumber;
    public string ModuleName;
    public string ModuleDescription;

    public List<ModuleLessons> ModuleLessons;
}
