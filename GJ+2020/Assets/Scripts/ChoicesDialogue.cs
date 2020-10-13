using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoicesDialogue : MonoBehaviour
{
    List<GameObject> Choices = new List<GameObject>();
    public List<StatColors> Colors;
    public GameObject Content;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < 3; i++)
            Choices.Add(Content.transform.GetChild(i).gameObject);
    }

    public void SetChoices(Stat type)
    {
        StatHandler.instance.SetStat(type);

        float current = DataHandler.GetPercent(type);

        for(int i = 0; i < 3; i++)
        {
            ChoicesValue c = GameManager.instance.Data.GetDataValue(type).Choices[i];
            if (!c.IsUnlocked || c.Amount == 0)
            {
                Choices[i].SetActive(false);
                continue;
            }
            Choices[i].transform.GetChild(0).GetComponent<Text>().text = c.ChoiceText + " (Cost: " + c.APCost + ")";
            if (GameManager.instance.ActionPoints < c.APCost)
            {
                Choices[i].GetComponent<Button>().interactable = false;
                Choices[i].GetComponent<EventTrigger>().triggers.Clear();
                Choices[i].GetComponent<Button>().onClick.RemoveAllListeners();
                continue;
            }

            Choices[i].SetActive(true);
            Choices[i].GetComponent<Image>().color = Colors.Find(x => x.stat == type).color;
            float toAdd = GameManager.instance.Data.GetDataValue(type).Choices[i].MeterFill;
            EventTrigger e = Choices[i].GetComponent<EventTrigger>();
            e.triggers.Clear();

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
                callback = new EventTrigger.TriggerEvent()
            };

            entry.callback.AddListener((data) =>
            {
                StatHandler.instance.ReturnAssistBar();
                StatHandler.instance.SetStat(type);
                StatHandler.instance.SetAssistBar(current + toAdd);
            });

            e.triggers.Add(entry);
            entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit,
                callback = new EventTrigger.TriggerEvent()
            };

            entry.callback.AddListener((data) => StatHandler.instance.ReturnAssistBar());

            e.triggers.Add(entry);

            Button b = Choices[i].GetComponent<Button>();
            int d = i;
            b.interactable = true;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => {
                StatHandler.instance.SetBar(current + toAdd);
                GameManager.instance.ActionPoints -= c.APCost;
                ActionDone.instance.DoAction(type, d);

                if (type == Stat.Nourishment)
                    GameManager.instance.UseItem(type, d);

            });
        }
    }
}

[System.Serializable]
public struct StatColors
{
    public Stat stat;
    public Color color;
}