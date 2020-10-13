using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageOwner : MonoBehaviour
{
    const int LENGTH = 35;
    public Text OwnerName;
    public Text PreviousText;
    public Image Image;
    public Image OnlineImage;

    [Space]
    public MessageChat CurrentChat;
    protected MessageChat PreviousChat;

    private void Awake()
    {
        PreviousChat = Instantiate(CurrentChat);
        
        OwnerName.text = CurrentChat.Name;
        string t = CurrentChat.CurrentChat[CurrentChat.CurrentChat.Count - 1].Message;

        UpdatePreviousText();

        if (CurrentChat.Picture != null)
            Image.sprite = CurrentChat.Picture;
    }

    private string shorterString(string s, int n)
    {
        if (s.Length < n)
            return s;
        else
           return s.Substring(0, n) + "...";
    }

    public void UpdatePreviousText()
    {
        Chat chat = PreviousChat.CurrentChat[PreviousChat.CurrentChat.Count - 1];

        if (chat.isMe)
            PreviousText.text = "Me: " + shorterString(chat.Message, LENGTH - 4);
        else
            PreviousText.text = shorterString(chat.Message, LENGTH);
    }

    public virtual void ShowMessages()
    {
        MessengerManager.instance.ShowMessages(PreviousChat);
    }
}
