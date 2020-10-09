using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler 
{
    public static float Mood = 0,Social = 0,Fitness = 0,Nourishment = 0, Money = 0;

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

    public static void LevelDownRelationship(string name)
    {
        Relationships.Find((x) => x.Name == name).LevelDown();
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

    public static int MoodChoices = 0, SocialChoices = 0, FitnessChoices = 0, NourishmentChoices = 0;

    public static void LevelUpChoices(Stat type)
    {
        switch (type)
        {
            case Stat.Fitness: FitnessChoices++; break;
            case Stat.Mood: MoodChoices++; break;
            case Stat.Nourishment: NourishmentChoices++; break;
            case Stat.Social: SocialChoices++; break;
        }
    }

    public static int GetChoicesLevel(Stat type)
    {
        switch (type)
        {
            case Stat.Fitness: return FitnessChoices;
            case Stat.Mood: return MoodChoices;
            case Stat.Nourishment: return NourishmentChoices;
            case Stat.Social: return SocialChoices++;

            default: return 0;
        }
    }
}

public class Relationship
{
    public string Name;
    public int Level = 0;
    public int MaxLevelReached = 0;

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
