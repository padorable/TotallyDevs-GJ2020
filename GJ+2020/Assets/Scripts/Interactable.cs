using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public ChoicesDialogue Choices;

    public Stat Type;
    public string TextIfUnavailable = "Hmm... I can't do that right now";
    public List<GameObject> Objects;

    private void Start()
    {
        GameManager.instance.OnItemChanged.AddListener(UpdateList);
    }

    public void UpdateList()
    {
        DataValues d = GameManager.instance.Data.GetDataValue(Type);
        for(int i = 0; i < 3; i++)
        {
            if (Objects[i] == null) continue;
            Objects[i].SetActive(d.Choices[i].IsUnlocked && d.Choices[i].Amount > 0);
        }
    }

    public void ShowChoices()
    {
        if(MouseCheck.instance != null)
            if (!MouseCheck.instance.enabled) return;

        if (GameManager.instance.IsDanger)
        {
            if (GameManager.instance.StatInDanger(Type))
            {
                if(IsAvailable())
                    DialogueManager.instance.SetChoices(Type);
                else
                    DialogueManager.instance.SetDialogue(TextIfUnavailable);
            }
            else
                DialogueManager.instance.SetDialogue("I don't feel like doing this...");
        }
        else if (IsAvailable())
            DialogueManager.instance.SetChoices(Type);
        else
            DialogueManager.instance.SetDialogue(TextIfUnavailable);
    }

    private bool IsAvailable()
    {
        if (GameManager.instance.ActionPoints == 0) return false;

        DataValues d = GameManager.instance.Data.GetDataValue(Type);

        bool available = false;
        foreach (ChoicesValue c in d.Choices)
        {
            if (c.IsUnlocked && c.Amount > 0)
            {
                available = true;
                break;
            }
        }

        return available;
    }
}
