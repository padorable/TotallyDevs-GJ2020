using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="News")]
public class NewsScriptable : ScriptableObject
{
    public string NewsTitle;

    [TextArea(3, 10)]
    public string ActualNews;
}
