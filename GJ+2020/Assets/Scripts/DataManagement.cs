using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DataManagement")]
public class DataManagement : ScriptableObject
{
    public int StartingMoney = 1000;
    public List<DataValues> Data;
    [Space]
    public List<Relationship> Relationships;

    public DataValues GetDataValue(Stat type) { return Data.Find(x => x.stat == type); }
}

[System.Serializable]
public class DataValues
{
    public Stat stat;
    [Range(0,1)]
    public float InitialValue;
    public List<ChoicesValue> Choices;
}


[System.Serializable]
public class Relationship
{
    public string Name;
    public int Level = 0;
    public bool IsOnline = true;
    [HideInInspector] public int MaxLevelReached = 0;

    public List<ToUnlock> Unlock = new List<ToUnlock>();

    public bool LevelUp()
    {
        if (Level == 3) return false;
        else
        {
            Level++;
            if (MaxLevelReached < Level) MaxLevelReached = Level;
            return true;
        }
    }

    public bool LevelDown()
    {
        if (Level == 0) return false;
        else
        {
            Level -= 1;
            return true;
        }
    }
}

[System.Serializable]
public class ChoicesValue
{
    public bool IsUnlocked;
    [Range(0, 2)]
    public int APCost;
    public float MeterFill;
    public string ChoiceText;
}

[System.Serializable]
public struct ToUnlock
{
    public Stat stat;
    public int LevelToUnlock;
}
