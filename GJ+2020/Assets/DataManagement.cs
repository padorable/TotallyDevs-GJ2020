using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DataManagement")]
public class DataManagement : ScriptableObject
{
    public int StartingMoney = 1000;
    public List<DataValues> Data;
    [Space]
    public List<string> Relationships;

    public DataValues GetDataValue(Stat type) { return Data.Find(x => x.stat == type); }
}

[System.Serializable]
public struct DataValues
{
    public Stat stat;
    [Range(0,1)]
    public float InitialValue;
    [Range(0, 1)]
    public List<float> MeterFill;
    [Range(0, 1)]
    public float BonusMeterFill;
    [TextArea(2,4)]
    public List<string> FlavorText;
}