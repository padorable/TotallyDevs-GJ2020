using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Chat")]
public class MessageChat : ScriptableObject
{
    public string Name;
    public Sprite Picture;
    public AudioClip FXSound;
    public List<Chat> CurrentChat;
}

[System.Serializable]
public struct Chat
{
    public bool isMe;
    [TextArea(1,4)]
    public string Message;
    public int Length;
}

