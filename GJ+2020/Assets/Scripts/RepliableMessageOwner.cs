﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RepliableMessageOwner : MessageOwner
{
    public List<MessageChat> Messages;
    public Relationship currentPerson;

    private void Start()
    {
        currentPerson = DataHandler.Relationships.Find(x => x.Name == CurrentChat.Name);
        GameManager.instance.NewWeek.AddListener(() =>
        {
            if (currentPerson.LastWeekTalkedTo + 2 < GameManager.instance.WeekNumber) currentPerson.IsOnline = true;
            else currentPerson.IsOnline = false;
        });
    }

    public override void ShowMessages()
    {
        if (currentPerson == null)
        {
            Debug.Log("WRONG NAME");
            return;
        }
        int index = Mathf.Min(currentPerson.Level, Messages.Count - 1);
        MessengerManager.instance.ToBeAdded = Messages[index];
        int cost = 0;
        if (currentPerson.Level < 3)
            cost = GameManager.instance.Data.GetDataValue(Stat.Social).Choices[Mathf.Min(2,currentPerson.Level)].APCost;
        else
            cost = 1;

        if (currentPerson.IsOnline)
            MessengerManager.instance.ShowMessagesAndChat(PreviousChat, cost);
        else
            MessengerManager.instance.ShowMessagesAndHeartOnly(PreviousChat);

        MessengerManager.instance.OnUpdateMessages = new UpdateMessages();

        MessengerManager.instance.OnUpdateMessages.AddListener((x) =>
        {
            currentPerson.LastWeekTalkedTo = GameManager.instance.WeekNumber;
            currentPerson.IsOnline = false;
            PreviousChat.CurrentChat.AddRange(x);
            GameManager.instance.DecreaseActionPoints(cost);

            StatHandler.instance.SetStat(Stat.Social);
            float toFill = GameManager.instance.Data.GetDataValue(Stat.Social).Choices[currentPerson.Level].MeterFill;
            StatHandler.instance.SetBar(DataHandler.GetPercent(Stat.Social) + toFill);

            UpdatePreviousText();
            DataHandler.LevelUpRelationship(CurrentChat.Name);
            MessengerManager.instance.OnUpdateMessages.RemoveAllListeners();
        });
    }

    public void CheckIfOnline()
    {
        OnlineImage.color = currentPerson.IsOnline ? Color.green : Color.grey;
    }
}
