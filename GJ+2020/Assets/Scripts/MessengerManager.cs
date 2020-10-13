using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    public static MessengerManager instance;
    [HideInInspector] public UpdateMessages OnUpdateMessages;

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
                Debug.Log("AAAAAAA");
                foreach(RepliableMessageOwner r in PeopleContent.GetComponentsInChildren<RepliableMessageOwner>())
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
        OnlineImage.color = DataHandler.Relationships.Find(x => x.Name == chat.Name).IsOnline ? Color.green : Color.grey;

        Viewport.offsetMin = new Vector2(4, 58);
        ChatBox.SetActive(true);
        ChatBox.GetComponent<Button>().interactable = GameManager.instance.ActionPoints >= cost;
        CurrentMessaging = chat;
        SetMessageBoxes();
        ChatBoxHeart.SetActive(true);
        updateHearts(DataHandler.GetRelationshipLevel(chat.Name));
        ChatBox.GetComponentInChildren<Text>().text = "Chat (Cost: " + cost + " AP)";
    }

    private void updateHearts(int amount)
    {
 
        for (int i = 0; i < 3; i++)
        {
            ChatBoxHeart.transform.GetChild(i).GetComponent<Image>().color = Color.gray;
        }

        for (int i = 0; i < Mathf.Clamp(amount, 0, 3); i++)
        {
            ChatBoxHeart.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
        }
    }

    public void ShowMessagesAndHeartOnly(MessageChat chat)
    {
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
        obj.GetComponent<Chatbox>().SetMessage(chat);
    }

    public void NewMessages(MessageChat chat)
    {
        StartCoroutine(LiveSend(chat));
    }

    IEnumerator LiveSend(MessageChat chat)
    {
        PhoneManager.instance.InteractButtons(false);
        CurrentMessaging = chat;
        foreach (Chat c in chat.CurrentChat)
        {
            yield return new WaitForSeconds(Random.value + 1.5f);
            AddMessage(c);

            Viewport.transform.parent.GetComponent<ScrollRect>().velocity = new Vector2(0, 200f);
        }
        PhoneManager.instance.InteractButtons(true);
    }

    public void ShowConfirmation()
    {
        NewMessages(ToBeAdded);
        OnUpdateMessages.Invoke(ToBeAdded.CurrentChat);
        Viewport.offsetMin = new Vector2(4, 30);
        ChatBox.SetActive(false);
        updateHearts(DataHandler.GetRelationshipLevel(CurrentMessaging.Name));
    }
}
