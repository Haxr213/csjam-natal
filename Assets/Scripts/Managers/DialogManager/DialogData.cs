using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Dialog
{
    public string name;
    [TextArea(5, 10)]
    public string text;
}
[CreateAssetMenu(fileName = "DialogData", menuName = "Scriptable Objects/DialogData")]
public class DialogData : ScriptableObject
{
    public List<Dialog> talkScript;
}
