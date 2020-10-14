using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler 
{
    //public static float Mood = 0,Social = 0,Fitness = 0,Nourishment = 0, Money = 0, MoodBonus = 0,SocialBonus = 0, FitnessBonus = 0, NourishmentBonus = 0;
    public static int Money;
    public static List<Relationship> Relationships;

    public static List<DataType> DataTypes = new List<DataType>();

    public static float GetPercent(Stat type)
    {
        return DataTypes.Find(x => x.stat == type).Meter;
    }

    public static int GetRelationshipLevel(string name)
    {
        return Relationships.Find((x) => x.Name == name).Level;
    }

    public static void LevelUpRelationship(string name)
    {
        Relationships.Find((x) => x.Name == name).LevelUp();
    }

    public static void AddStat(Stat type, float toAdd)
    {
        DataTypes.Find(x => x.stat == type).Meter += toAdd;
    }

    public static void SetStat(Stat type, float value)
    {
        DataTypes.Find(x => x.stat == type).Meter = value;
    }

    public static void SetBonusStat(Stat type, float value)
    {
        DataTypes.Find(x => x.stat == type).Bonus = value;
    }

    public static float GetBonus(Stat type)
    {
        return DataTypes.Find(x => x.stat == type).Bonus;
    }
}

public class DataType
{
    public Stat stat;
    public float Meter;
    public float Bonus;
}