﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDate : MonoBehaviour
{
    private Text dateText;
    // Start is called before the first frame update
    void Awake()
    {
        dateText = this.GetComponent<Text>();
    }

    private void Start()
    {
        GameManager.instance.NewWeek.AddListener(SetDate);
        SetDate();
    }

    private void SetDate()
    {
        int w = GameManager.instance.WeekNumber;
        string month = "";
        switch(Mathf.FloorToInt((float)w/4f))
        {
            case 0: month = "October"; break;
            case 1: month = "November"; break;
            case 2: month = "December"; break;
        }
        dateText.text = month + ", Week " + ((w % 4) + 1);
    }
}