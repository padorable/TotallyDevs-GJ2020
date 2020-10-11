using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DataManagement Data;
    private int _weekNumber = 0;
    private int _actionPoints = 3;

    public UnityEvent NewWeek = new UnityEvent();
    public UnityEvent OnChangedActionPoints = new UnityEvent();

    public int WeekNumber
    {
        get
        {
            return _weekNumber;
        }
        set
        {
            _weekNumber = value;
            NewWeek?.Invoke();
        }
    }

    public int ActionPoints
    {
        get
        {
            return _actionPoints;
        }
        set
        {
            _actionPoints = value;
            OnChangedActionPoints?.Invoke();
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            if (instance != this)
                Destroy(this);

        Data = Instantiate(Data);
        DataHandler.Money = Data.StartingMoney;
        DataHandler.Mood = Data.GetDataValue(Stat.Mood).InitialValue;
        DataHandler.Nourishment = Data.GetDataValue(Stat.Nourishment).InitialValue;
        DataHandler.Social = Data.GetDataValue(Stat.Social).InitialValue;
        DataHandler.Fitness = Data.GetDataValue(Stat.Fitness).InitialValue;
        DataHandler.Relationships = Data.Relationships;
        //List<Relationship> relationships = new List<Relationship>();
        //foreach(string a in Data.Relationships)
        //{
        //    relationships.Add(new Relationship
        //    {
        //        Name = a,
        //        Level = 0,
        //        MaxLevelReached = 0
        //    });
        //}
        //DataHandler.Relationships = relationships;
    }

    private void Start()
    {
        NewWeek.AddListener(() =>
        {
            float p = Outcome();
            if(p < .3f)
            {
                //bad outcome
            }
            else if (p < .5f)
            {
                // okay outcome
            }
            else
            {
                //Good outcome
            }
        });
    }

    public void NextWeek()
    {
        DataHandler.Mood = Mathf.Max(0, DataHandler.Mood - .2f + DataHandler.MoodBonus);
        DataHandler.Nourishment = Mathf.Max(0, DataHandler.Nourishment - .2f + DataHandler.NourishmentBonus);
        DataHandler.Social = Mathf.Max(0, DataHandler.Social - .2f + DataHandler.SocialBonus);
        DataHandler.Fitness = Mathf.Max(0, DataHandler.Fitness - .2f + DataHandler.FitnessBonus);

        WeekNumber++;
        ActionPoints = 3;
        DataHandler.Money += 750 * (float)Mathf.FloorToInt(WeekNumber / 4);
    }

    public string Month(int week, bool shortcut)
    {
        string s = "";
        switch (Mathf.FloorToInt(week / 4))
        {
            case 0: s = "October"; break;
            case 1: s = "November"; break;
            case 2: s = "December"; break;

            default: s = "December"; break;
        }
        return shortcut ? s.Substring(0,3) : s;
    }

    public float Outcome()
    {
        int total = 0;
        int unlocked = 0;
        foreach(DataValues a in Data.Data)
        {
            foreach(ChoicesValue b in a.Choices)
            {
                total++;
                if (b.IsUnlocked) unlocked++;
            }
        }
        return (float)unlocked / (float)total;

    }
}
