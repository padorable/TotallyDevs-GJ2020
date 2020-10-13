using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RepliableMessageOwner : MessageOwner
{
    public List<MessageChat> Messages;
    private Relationship currentPerson;

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
        int cost = Mathf.FloorToInt(currentPerson.Level / 2) + 1;
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

            UnityAction action = () =>
            {
                DataHandler.LevelUpRelationship(CurrentChat.Name);
                MessengerManager.instance.OnUpdateMessages.RemoveAllListeners();
            };
            GameManager.instance.ActionPoints -= cost;
            GameManager.instance.NewWeek.AddListener(action);
            StatHandler.instance.SetStat(Stat.Social);
            StatHandler.instance.SetBar(DataHandler.Social + .2f);
            GameManager.instance.NewWeek.AddListener(() => GameManager.instance.NewWeek.RemoveListener(action));
        });
    }
}
