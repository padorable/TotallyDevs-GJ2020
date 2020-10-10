using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    [TextArea(15, 20)]
    public List<string> ListOfDialogues;

    private void Awake()
    {
    }
    public void ChosenDialogue(int n)
    {
        if(n >= ListOfDialogues.Count || n <0)
            DialogueManager.instance.SetDialogue(ListOfDialogues[0]);
        else
            DialogueManager.instance.SetDialogue(ListOfDialogues[n]);
    }
}
