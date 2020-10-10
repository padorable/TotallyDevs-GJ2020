﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUnlock : MonoBehaviour
{
    public List<LockedItems> Locked;
    public UnityEngine.UI.Text Credits;
    public void checkItems()
    {
        foreach(Relationship r in DataHandler.Relationships)
        {
            for (int i = 0; i < r.Level; i++)
            {
                LockedItems locked = Locked.Find((x) => x.ItemStat == r.Unlock[i].stat && x.LevelToUnlock == r.Unlock[i].LevelToUnlock);
                if (locked != null)
                {
                    locked.Item.SetActive(true);
                    Locked.Remove(locked);
                }
            }
        }
    }

    private void Update()
    {
        Credits.text = "Credits: " + DataHandler.Money;
    }
}

[System.Serializable]
public class LockedItems
{
    public GameObject Item;
    public Stat ItemStat;
    public int LevelToUnlock;
}