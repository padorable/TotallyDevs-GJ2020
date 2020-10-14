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
                int good = 0, normal = 0;
                Stat[] stats = { Stat.Nourishment, Stat.Mood, Stat.Fitness, Stat.Social };
                foreach (Stat s in stats)
                {
                    int o = Mathf.FloorToInt(DataHandler.GetPercent(s) * 100);
                    if (o > 60)
                        good++;
                    if (o > 40)
                        normal++;
                };

                float p = PercentDone();
                if (good == 4)
                {
                    // Good Outcome
                    AudioManager.instance.PlayBG(5);
                    Entries.instance.ShowButton(3);
                }
                else if (normal >= 3)
                {
                    // Normal Outcome
                    AudioManager.instance.PlayBG(4);
                    Entries.instance.ShowButton(2);
                }
                else
                {
                    // Bad Outcome
                    AudioManager.instance.PlayBG(3);
                    Entries.instance.ShowButton(1);
                }
            }
        });

        TransitionManager.instance.BetweenTransition.AddListener(RefillActionPoints);
    }

    public void DecreaseActionPoints(int i)
    {
        ActionPoints -= i;
        if (ActionPoints > 0)
            AudioManager.instance.PlayFX(4);
        else
            AudioManager.instance.PlayFX(3);
    }

    public void RefillActionPoints()
    {
        ActionPoints = 3 + (Mathf.FloorToInt((float)WeekNumber / 4f));
    }

    public void NextWeek()
    {
        if(MouseCheck.instance != null)
            if (!MouseCheck.instance.enabled) return;
        Stat[] stats = { Stat.Mood, Stat.Nourishment, Stat.Social, Stat.Fitness };

        if (WeekNumber < 12)
        {
            foreach (Stat s in stats)
            {
                Debug.Log(DataHandler.GetPercent(s));
                if (Mathf.FloorToInt(DataHandler.GetPercent(s) * 100) <= 35)
                {
                    continue;
                }

                DataHandler.AddStat(s, DataHandler.GetBonus(s) - .2f);
            }
        }

        WeekNumber++;
        
        DataHandler.Money += 750 + (1500 * (Mathf.FloorToInt((float)WeekNumber / 4f)));
        DialogueManager.instance.SetDialogue(". . .");
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
                if (dt.Meter < .30)
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            WeekNumber = 12;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Stat[] s = { Stat.Fitness, Stat.Mood, Stat.Nourishment, Stat.Social };
            foreach(Stat a in s)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    DataHandler.SetStat(a, 1);
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    DataHandler.SetStat(a, .5f);
                else
                    DataHandler.SetStat(a, .35f);
            }
            WeekNumber = 12;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
