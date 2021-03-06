﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UpdateMessages : UnityEvent<List<Chat>> { }

public class MessengerManager : MonoBehaviour
{
    public GameObject MessagingPrefab;

    public MessageChat CurrentMessaging;
    public GameObject MessageContent;
    public GameObject PeopleContent;
    public Confirmation ConfirmWindow;
    private List<GameObject> activeObjects = new List<GameObject>();
    private List<GameObject> disabledObjects = new List<GameObject>();
    private List<MessageChat> SavedChats;

    [Space]
    public MessageChat ToBeAdded;
    public RectTransform Viewport;
    public GameObject ChatBox;
    public Text ChatBoxText;
    public Image ChatBoxPerson;
    public GameObject ChatBoxHeart;
    public Image OnlineImage;
    public CanvasGroup Calling;

    public static MessengerManager instance;
    [HideInInspector] public UpdateMessages OnUpdateMessages;
    [HideInInspector] public UnityEvent OnEndUpdateMessages = new UnityEvent();

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            if (instance != this)
                Destroy(instance);
        }
    }

    private void Start()
    {
        PhoneManager.instance.OnOpenNewScreen.AddListener(x =>
        {
            if(x == this.gameObject)
            {
                RepliableMessageOwner[] messagers = PeopleContent.GetComponentsInChildren<RepliableMessageOwner>();
                foreach (RepliableMessageOwner r in messagers)
                {
                    r.CheckIfOnline();
                }
            }
        });
    }

    public void ShowMessages(MessageChat chat)
    {
        Viewport.offsetMin = new Vector2 (4, 30);
        ChatBox.SetActive(false);
        CurrentMessaging = chat;
        SetMessageBoxes();
        ChatBoxHeart.SetActive(false);
    }

    public void ShowMessagesAndChat(MessageChat chat, int cost)
    {
        AudioManager.instance.source.PlayOneShot(chat.FXSound);
        Relationship r = DataHandler.Relationships.Find(x => x.Name == chat.Name);
        OnlineImage.color = r.IsOnline ? Color.green : Color.grey;
        Viewport.offsetMin = new Vector2(4, 58);
        ChatBox.SetActive(true);

        ChatBox.GetComponent<Button>().interactable = GameManager.instance.ActionPoints >= cost;

        if (GameManager.instance.ActionPoints >= cost)
        {
            EventTrigger e = ChatBox.GetComponent<EventTrigger>();
            if (e == null) e = ChatBox.AddComponent<EventTrigger>();

            e.triggers.Clear();

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
                callback = new EventTrigger.TriggerEvent()
            };

            float toFill = GameManager.instance.Data.GetDataValue(Stat.Social).Choices[Mathf.Min(2,r.Level)].MeterFill;

            entry.callback.AddListener((data) =>
            {
                StatHandler.instance.ReturnAssistBar();
                StatHandler.instance.SetStat(Stat.Social);
                StatHandler.instance.SetAssistBar(DataHandler.GetPercent(Stat.Social) + toFill);
            });

            e.triggers.Add(entry);
            entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit,
                callback = new EventTrigger.TriggerEvent()
            };

            entry.callback.AddListener((data) => StatHandler.instance.ReturnAssistBar());

            e.triggers.Add(entry);
        }

        CurrentMessaging = chat;
        SetMessageBoxes();
        ChatBoxHeart.SetActive(true);
        updateHearts(DataHandler.GetRelationshipLevel(chat.Name));
        if(r.Level < 3)
            ChatBox.GetComponentInChildren<Text>().text = "Chat (Cost: " + cost + " AP)";
        else
            ChatBox.GetComponentInChildren<Text>().text = "Call (Cost: 1AP)";
    }

    private void updateHearts(int amount)
    {
 
        for (int i = 0; i < 3; i++)
        {
            ChatBoxHeart.transform.GetChild(i).GetComponent<Image>().color = Color.gray;
        }

        for (int i = 0; i < Mathf.Clamp(amount, 0, 3); i++)
        {
            ChatBoxHeart.transform.GetChild(i).GetComponent<Image>().color = Color.red;
        }
    }

    public void ShowMessagesAndHeartOnly(MessageChat chat)
    {
        AudioManager.instance.source.PlayOneShot(chat.FXSound);
        Relationship r = DataHandler.Relationships.Find(x => x.Name == chat.Name);
        OnlineImage.color = r.IsOnline ? Color.green : Color.grey;

        Debug.Log("Is Online?: " + r.IsOnline);
        Viewport.offsetMin = new Vector2(4, 30);
        ChatBox.SetActive(false);
        CurrentMessaging = chat;
        SetMessageBoxes();
        ChatBoxHeart.SetActive(true);
        updateHearts(DataHandler.GetRelationshipLevel(chat.Name));
    }

    private void SetMessageBoxes()
    {
        ChatBoxText.text = CurrentMessaging.Name;
        ChatBoxPerson.sprite = CurrentMessaging.Picture;
        
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            disabledObjects.Add(activeObjects[i]);
            activeObjects[i].SetActive(false);
            activeObjects.RemoveAt(i);
        }

        for (int i = 0; i < CurrentMessaging.CurrentChat.Count; i++)
        {
            AddMessage(CurrentMessaging.CurrentChat[i]);
        }

        this.transform.parent.GetComponent<PhoneManager>().ShowNextScreen(MessageContent.transform.parent.parent.gameObject);
    }

    private void AddMessage(Chat chat)
    {
        GameObject obj = null;

        if (disabledObjects.Count == 0)
        {
            obj = Instantiate(MessagingPrefab);
            obj.transform.SetParent(MessageContent.transform);
            obj.transform.localScale = Vector3.one;
        }
        else
        {
            obj = disabledObjects[0];
            obj.SetActive(true);
            obj.transform.SetAsLastSibling();
            disabledObjects.RemoveAt(0);
        }

        activeObjects.Add(obj);
        obj.GetComponent<Chatbox>().SetMessage(chat, CurrentMessaging.Picture);
    }

    public void NewMessages(MessageChat chat)
    {
        StartCoroutine(LiveSend(chat));
    }

    IEnumerator LiveSend(MessageChat chat)
    {
        if(MouseCheck.instance != null)
            MouseCheck.instance.enabled = false;

        PhoneManager.instance.InteractButtons(false);
        CurrentMessaging = chat;
        foreach (Chat c in chat.CurrentChat)
        {
            yield return new WaitForSeconds(Random.value + 1.5f);
            AddMessage(c);
            AudioManager.instance.PlayFX(10);
            Viewport.transform.parent.GetComponent<ScrollRect>().velocity = new Vector2(0, 200f);
        }
        PhoneManager.instance.InteractButtons(true);

        if (MouseCheck.instance != null)
            MouseCheck.instance.enabled = true;

        if (OnEndUpdateMessages != null)
            OnEndUpdateMessages.Invoke();
    }

    public void ShowConfirmation()
    {
        if (GameManager.instance.IsDanger)
        {
            if (!GameManager.instance.StatInDanger(Stat.Social))
            {
                DialogueManager.instance.SetDialogue("I don't feel like doing this...");
                return;
            }
        }

        Relationship r = DataHandler.Relationships.Find(x => x.Name == CurrentMessaging.Name);

        if (r.Level < 3)
        {
            NewMessages(ToBeAdded);
            OnUpdateMessages.Invoke(ToBeAdded.CurrentChat);
            updateHearts(DataHandler.GetRelationshipLevel(CurrentMessaging.Name));
        }
        else
        {
            float toFill = GameManager.instance.Data.GetDataValue(Stat.Social).Choices[Mathf.Min(2, r.Level)].MeterFill;
            GameManager.instance.DecreaseActionPoints(1);
            StatHandler.instance.SetStat(Stat.Social);
            StatHandler.instance.SetBar(DataHandler.GetPercent(Stat.Social) + toFill);
            StartCoroutine(Call());
        }
        Viewport.offsetMin = new Vector2(4, 30);
        ChatBox.SetActive(false);

    }

    IEnumerator Call()
    {
        Calling.GetComponentInChildren<Text>().text = "Your call with " + CurrentMessaging.Name + " lasted for\n\n" + Random.Range(30, 300) + " minutes!";
        float d = 0, a = 1.5f, p = 3.0f;

        while(d < a)
        {
            d = Mathf.Min(a, d + Time.deltaTime);
            Calling.alpha = d / a;
            yield return null;
        }

        yield return new WaitForSeconds(p);
        d = 0;

        while (d < a)
        {
            d = Mathf.Min(a, d + Time.deltaTime);
            Calling.alpha = 1 - (d / a);
            yield return null;
        }
    }
}