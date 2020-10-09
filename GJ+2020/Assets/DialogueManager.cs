using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private Text textRenderer;

    [SerializeField] private string defaultText = "Dev: ...";

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            if (instance != this)
                Destroy(this);
        }

        textRenderer = this.GetComponentInChildren<Text>();
    }

    public void SetDialogue(string text)
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        textRenderer.text = text;
        this.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SetTextToDefault()
    {
        textRenderer.text = defaultText;
    }

    public void SetChoices(Stat type)
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);
        this.transform.GetChild(1).GetComponent<ChoicesDialogue>().SetChoices(type, DataHandler.GetChoicesLevel(type));
    }
}
