using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoicesDialogue : MonoBehaviour
{
    List<GameObject> Choices = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 1; i <= 3; i++)
            Choices.Add(this.transform.GetChild(i).gameObject);
    }

    public void SetChoices(Stat type, int count)
    {
        StatHandler.instance.SetStat(type);

        float current = DataHandler.GetPercent(type);
        for(int i = 0; i <= count; i++)
        {
            Choices[i].SetActive(true);
            float toAdd = GameManager.instance.Data.GetDataValue(type).MeterFill[i];
            EventTrigger e = Choices[i].GetComponent<EventTrigger>();
            e.triggers.Clear();

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
                callback = new EventTrigger.TriggerEvent()
            };

            entry.callback.AddListener((data) => StatHandler.instance.SetAssistBar(current + toAdd));

            e.triggers.Add(entry);
            entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit,
                callback = new EventTrigger.TriggerEvent()
            };

            entry.callback.AddListener((data) => StatHandler.instance.ReturnAssistBar());

            e.triggers.Add(entry);

            Button b = Choices[i].GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => { StatHandler.instance.SetBar(current + toAdd); SetChoices(type, count); });
        }

        for(int i = count + 1; i < Choices.Count; i++)
        {
            Choices[i].SetActive(false);
        }
    }
}
