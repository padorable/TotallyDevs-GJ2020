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
    const int LengthOfTextToAdjust = 26;
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
        float size = Mathf.FloorToInt((float)chat.Message.Length / (float)LengthOfTextToAdjust) + 1;
        MessageBox.sizeDelta = initSize * new Vector2(1, size);
    }
}
