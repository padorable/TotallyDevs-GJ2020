using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Diary")]
public class DiaryEntry : ScriptableObject
{
    public List<Entry> Entries;
}

[System.Serializable]
public class Entry
{
    public string EntryText;
    public int Length;
}
