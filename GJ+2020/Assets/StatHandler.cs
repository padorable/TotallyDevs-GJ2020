using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    public static StatHandler instance;
    public Stat CurrentStatToChange;
    private List<BarHandler> handlers = new List<BarHandler>();
    private BarHandler toChange;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);

        foreach(BarHandler handler in this.GetComponentsInChildren<BarHandler>())
        {
            handlers.Add(handler);
        }
    }

    private void Start()
    {
        RefreshBars();
    }
    public void SetAssistBar(float percent)
    {
        toChange.SetAssistBar(percent);
    }

    public void ReturnAssistBar()
    {
        toChange.ReturnAssistBar();
    }

    public void SetBar(float percent)
    {
        DataHandler.SetStat(CurrentStatToChange,percent);
        toChange.SetBar(percent);
    }

    public void SetStat(Stat stat)
    {
        CurrentStatToChange = stat;
        toChange = handlers.Find(x => x.CurrentStat == stat);
    }

    public void RefreshBars()
    {
        foreach(BarHandler h in handlers)
        {
            h.SetBar(DataHandler.GetPercent(h.CurrentStat));
        }
    }
}
