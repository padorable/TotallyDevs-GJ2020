using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialHelper> Objects;

    private int currentPhase = 0;

    private void Start()
    {
        TransitionManager.instance.BetweenTransition.AddListener(() =>
        {
            if(GameManager.instance.WeekNumber == 0 || GameManager.instance.WeekNumber >= 12)
            {
                NextPhase();
            }
        });

        GameManager.instance.OnChangedActionPoints.AddListener(() =>
        {
            if(GameManager.instance.ActionPoints == 0 && GameManager.instance.WeekNumber < 0)
            {
                StartCoroutine(DelayNext(4.0f));
            }
        });

        MessengerManager.instance.OnEndUpdateMessages.AddListener(() =>
        {
            if (GameManager.instance.WeekNumber < 0)
            {
                NextPhase();
            }
        });
    }

    IEnumerator DelayNext(float n)
    {
        yield return new WaitForSeconds(n);
        NextPhase();
    }

    public void RunPhase()
    {        
        TutorialHelper th = Objects[currentPhase];
        DialogueManager.instance.SetDialogue(th.DialogueText);
        foreach (GameObject o in th.ToDisable)
        {
            o.SetActive(false);
        }

        foreach(Button b in th.SetToNotInteractable)
        {
            b.interactable = false;
        }

        foreach(GameObject o in th.ToEnable)
        {
            o.SetActive(true);
        }

        foreach (Button b in th.SetToInteractable)
        {
            b.interactable = true;
        }
    }

    public void NextPhase()
    {
        currentPhase++;
        RunPhase();
    }
}

[System.Serializable]
public class TutorialHelper
{
    [TextArea(1,3)]
    public string DialogueText;
    public List<GameObject> ToDisable;
    public List<Button> SetToNotInteractable;
    public List<GameObject> ToEnable;
    public List<Button> SetToInteractable;
}