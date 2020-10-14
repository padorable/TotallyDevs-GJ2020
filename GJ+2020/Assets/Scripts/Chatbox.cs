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
    public Sprite Me;
    Vector2 initSize;
    const int LengthOfTextToAdjust = 23;
    private void Awake()
    {
        initSize = MessageBox.sizeDelta;
    }

    public void SetMessage(Chat chat, Sprite i)
    {
        if (chat.isMe)
        {
            horizontalGroup.reverseArrangement = true;
            horizontalGroup.childAlignment = TextAnchor.LowerRight;
            Message.alignment = TextAnchor.MiddleRight;
            picture.sprite = Me;
        }
        else
        {
            horizontalGroup.reverseArrangement = false;
            horizontalGroup.childAlignment = TextAnchor.LowerLeft;
            Message.alignment = TextAnchor.MiddleLeft;
            picture.sprite = i;
        }

        Message.text = chat.Message;
        float size = Mathf.Max(1, Mathf.FloorToInt((float)chat.Message.Length / (float)LengthOfTextToAdjust) + 1);
        MessageBox.sizeDelta = initSize * new Vector2(1, size);
    }
}
