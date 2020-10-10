using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatbox : MonoBehaviour
{
    public HorizontalLayoutGroup horizontalGroup;
    public Text Message;
    public RectTransform MessageBox;
    public Image picture;
    Vector2 initSize;
    private void Awake()
    {
        initSize = MessageBox.sizeDelta;
    }

    public void SetMessage(Chat chat)
    {
        if (chat.isMe)
        {
            horizontalGroup.reverseArrangement = true;
            horizontalGroup.childAlignment = TextAnchor.LowerRight;
            Message.alignment = TextAnchor.MiddleRight;
        }
        else
        {
            horizontalGroup.reverseArrangement = false;
            horizontalGroup.childAlignment = TextAnchor.LowerLeft;
            Message.alignment = TextAnchor.MiddleLeft;

        }
        Message.text = chat.Message;
        MessageBox.sizeDelta = initSize * new Vector2(1,chat.Length);
    }
}
