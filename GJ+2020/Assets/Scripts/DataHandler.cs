using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler 
{
    public static float Mood = 0,Social = 0,Fitness = 0,Nourishment = 0, Money = 0, MoodBonus = 0,SocialBonus = 0, FitnessBonus = 0, NourishmentBonus = 0;

    public static List<Relationship> Relationships;

    public static float GetPercent(Stat type)
    {
        switch(type)
        {
            case Stat.Fitness: return Fitness;
            case Stat.Mood: return Mood;
            case Stat.Nourishment: return Nourishment;
            case Stat.Social: return Social;

            default: return 0;
        }
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
        switch (type)
        {
            case Stat.Fitness: Fitness = Fitness + toAdd; break;
            case Stat.Mood: Mood = Mood + toAdd; break;
            case Stat.Nourishment: Nourishment = Nourishment + toAdd; break;
            case Stat.Social: Social = Social + toAdd; break;
        }
    }

    public static void SetStat(Stat type, float value)
    {
        switch (type)
        {
            case Stat.Fitness: Fitness = value; break;
            case Stat.Mood: Mood = value; break;
            case Stat.Nourishment: Nourishment = value; break;
            case Stat.Social: Social = value; break;
        }
    }

    public static void SetBonusStat(Stat type, float value)
    {
        switch (type)
        {
            case Stat.Fitness: FitnessBonus = value; break;
            case Stat.Mood: MoodBonus = value; break;
            case Stat.Nourishment: NourishmentBonus = value; break;
            case Stat.Social: SocialBonus = value; break;
        }
    }

    public static float GetBonus(Stat type)
    {
        switch (type)
        {
            case Stat.Fitness: return FitnessBonus;
            case Stat.Mood: return MoodBonus;
            case Stat.Nourishment: return NourishmentBonus;
            case Stat.Social: return SocialBonus;

            default: return 0;
        }
    }
}