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

    public void NextWeek()
    {
        DataHandler.Mood = Mathf.Max(0, DataHandler.Mood - .2f + DataHandler.MoodBonus);
        DataHandler.Nourishment = Mathf.Max(0, DataHandler.Nourishment - .2f + DataHandler.NourishmentBonus);
        DataHandler.Social = Mathf.Max(0, DataHandler.Social - .2f + DataHandler.SocialBonus);
        DataHandler.Fitness = Mathf.Max(0, DataHandler.Fitness - .2f + DataHandler.FitnessBonus);

        WeekNumber++;
    }
}
