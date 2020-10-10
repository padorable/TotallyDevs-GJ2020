using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public ChoicesDialogue Choices;

    public Stat Type;

    public void ShowChoices()
    {
        DialogueManager.instance.SetChoices(Type);
    }
}
