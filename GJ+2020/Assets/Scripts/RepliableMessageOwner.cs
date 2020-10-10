using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepliableMessageOwner : MessageOwner
{
    public List<MessageChat> Messages;
    
    public override void ShowMessages()
    {
        Relationship r = DataHandler.Relationships.Find(x => x.Name == CurrentChat.Name);
        if(r == null)
        {
            Debug.Log("WRONG NAME");
            return;
        }
        int index = r.Level;
        MessengerManager.instance.ToBeAdded = Messages[0];

        if (r.IsOnline)
            MessengerManager.instance.ShowMessagesAndChat(PreviousChat);
        else
            MessengerManager.instance.ShowMessagesAndHeartOnly(PreviousChat);

        MessengerManager.instance.OnUpdateMessages = new UpdateMessages();
        MessengerManager.instance.OnUpdateMessages.AddListener((x) => 
        {
            r.IsOnline = false;
            PreviousChat.CurrentChat.AddRange(x);
            GameManager.instance.NewWeek.AddListener(()=> DataHandler.LevelUpRelationship(CurrentChat.Name));
        });
    }
}
