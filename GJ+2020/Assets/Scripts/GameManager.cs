using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DataManagement Data;
    private int _weekNumber = -1;
    private int _actionPoints = 2;

    public UnityEvent NewWeek = new UnityEvent();
    public UnityEvent OnChangedActionPoints = new UnityEvent();
    public UnityEvent OnItemChanged = new UnityEvent();

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
        Stat[] stats = { Stat.Nourishment, Stat.Mood, Stat.Fitness, Stat.Social };
        foreach(Stat s in stats)
        {
            DataHandler.DataTypes.Add(new DataType
            {
                stat = s,
                Meter = Data.GetDataValue(s).InitialValue,
                Bonus = 0
            });
        };

        DataHandler.Relationships = Data.Relationships;
    }

    private void Start()
    {
        NewWeek.AddListener(() =>
        {
            if (WeekNumber >= 12)
            {
                Stat[] stats = { Stat.Nourishment, Stat.Mood, Stat.Fitness, Stat.Social };
                foreach (Stat s in stats)
                {
                    DataHandler.SetStat(s, 1);
                };

                float p = PercentDone();
                if (p < .3f)
                {
                    Entries.instance.ShowButton(1);
                    //bad outcome
                }
                else if (p < .5f)
                {
                    Entries.instance.ShowButton(2);
                    // okay outcome
                }
                else
                {
                    Entries.instance.ShowButton(3);
                    //Good outcome
                }
            }
        });

        TransitionManager.instance.BetweenTransition.AddListener(() => ActionPoints = 3 + (Mathf.FloorToInt((float)WeekNumber / 4f)));
    }

    public void NextWeek()
    {
        if(MouseCheck.instance != null)
            if (!MouseCheck.instance.enabled) return;
        Stat[] stats = { Stat.Mood, Stat.Nourishment, Stat.Social, Stat.Fitness };
        foreach(Stat s in stats)
        {
            DataHandler.AddStat(s, DataHandler.GetBonus(s) - .2f);
        }

        WeekNumber++;
        
        DataHandler.Money += 750 + (1500 * (Mathf.FloorToInt((float)WeekNumber / 4f)));
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

    public float PercentDone()
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

    public void UnlockItem(Stat stat, int lvl)
    {
        Data.GetDataValue(stat).Choices[lvl].IsUnlocked = true;
        Data.GetDataValue(stat).Choices[lvl].Amount++;

        OnItemChanged?.Invoke();
    }

    public void UseItem(Stat stat, int lvl)
    {
        Data.GetDataValue(stat).Choices[lvl].Amount--;
        OnItemChanged?.Invoke();
    }

    public bool IsDanger
    {
        get
        {
            bool isInDanger = false;
            foreach (DataType dt in DataHandler.DataTypes)
            {
                if (dt.Meter < .35)
                {
                    isInDanger = true;
                    break;
                }
            }
            return isInDanger;
        }
    }

    public bool StatInDanger(Stat stat)
    {
        return DataHandler.DataTypes.Find(x => x.stat == stat).Meter < .35f;
    }
}
