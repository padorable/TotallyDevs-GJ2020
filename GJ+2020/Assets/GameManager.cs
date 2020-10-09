using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DataManagement Data;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            if (instance != this)
                Destroy(this);

        DataHandler.Money = Data.StartingMoney;
        DataHandler.Mood = Data.GetDataValue(Stat.Mood).InitialValue;
        DataHandler.Nourishment = Data.GetDataValue(Stat.Nourishment).InitialValue;
        DataHandler.Social = Data.GetDataValue(Stat.Social).InitialValue;
        DataHandler.Fitness = Data.GetDataValue(Stat.Fitness).InitialValue;

        List<Relationship> relationships = new List<Relationship>();
        foreach(string a in Data.Relationships)
        {
            relationships.Add(new Relationship
            {
                Name = a,
                Level = 0,
                MaxLevelReached = 0
            });
        }
        DataHandler.Relationships = relationships;
    }

}
