using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private Text textRenderer;

    [SerializeField] private string defaultText = "...";

    private bool running = false;

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

        this.transform.GetChild(1).gameObject.SetActive(false);

        StartCoroutine(typing(text));
    }

    public void SetTextToDefault()
    {
        textRenderer.text = defaultText;
    }

    public void SetChoices(Stat type)
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);
        this.transform.GetChild(1).GetComponent<ChoicesDialogue>().SetChoices(type);
    }

    IEnumerator typing(string dialogue)
    {
        running = true;
        string start = "<color=#ffffffff>";
        string middle = "</color><color=#ffffff00>";
        string end = "</color>";

        textRenderer.text = "<color=#ffffff00>" + dialogue + "</color>";

        for (int i = 0; i < dialogue.Length; i++)
        {
            string a = dialogue.Substring(0, i), b = dialogue.Substring(i, dialogue.Length - i);

            textRenderer.text = start + a + middle + b + end;
            yield return null;
        }
        textRenderer.text = "<color=#ffffffff>" + dialogue + "</color>";
        running = false;
    }

    IEnumerator typingMany(List<string> dialogues)
    {
        for(int i = 0; i < dialogues.Count; i++)
        {
            Coroutine e = StartCoroutine(typing(dialogues[i]));
            yield return new WaitUntil(() => !running);
            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetDialogue("this is a test if it will work");
        }
    }
}
