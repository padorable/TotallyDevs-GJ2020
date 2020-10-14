using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DiaryManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject PrefabEntry;
    public DiaryEntry CurrentDiaryEntry;
    public GameObject ButtonEntry;

    public UnityEvent OnFinishedReading;

    private int index = -1;
    private List<GameObject> disabled = new List<GameObject>();
    private List<GameObject> active = new List<GameObject>();

    private Vector2 initSize = Vector2.zero;

    public static DiaryManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void NextEntry()
    {
        index++;
        if(index < CurrentDiaryEntry.Entries.Count)
        {
            SpawnEntry(CurrentDiaryEntry.Entries[index]);

            if(index + 1 >= CurrentDiaryEntry.Entries.Count)
            {
                PhoneManager.instance.InteractButtons(true);
                CurrentDiaryEntry.IsDoneReading = true;
                OnFinishedReading?.Invoke();
                ButtonEntry.SetActive(false);
                Content.transform.parent.GetComponent<RectTransform>().offsetMin = new Vector2(0, 6);
            }
        }
    }

    public void StartEntry(DiaryEntry entry)
    {
        CurrentDiaryEntry = entry;
        for(int i = active.Count - 1; i >= 0; i --)
        {
            active[i].SetActive(false);
            disabled.Add(active[i]);
            active.RemoveAt(i);
        }

        if(entry.IsDoneReading)
        {
            ButtonEntry.SetActive(false);
            Content.transform.parent.GetComponent<RectTransform>().offsetMin = new Vector2(0, 6);
            foreach (Entry e in entry.Entries)
            {
                SpawnEntry(e);
            }
        }
        else
        {
            PhoneManager.instance.InteractButtons(false);
            ButtonEntry.SetActive(true);
            Content.transform.parent.GetComponent<RectTransform>().offsetMin = new Vector2(0, 36);
            index = -1;
        }
    }

    private void SpawnEntry(Entry entry)
    {
        GameObject obj = null;
        if (disabled.Count == 0)
        {
            obj = Instantiate(PrefabEntry);
            obj.transform.SetParent(Content.transform);
            obj.transform.localScale = Vector3.one;
            active.Add(obj);
        }
        else
        {
            obj = disabled[0];
            obj.SetActive(true);
            disabled.RemoveAt(0);
            active.Add(obj);
            obj.transform.SetAsLastSibling();
        }

        obj.GetComponentInChildren<Text>().text = entry.EntryText;
        if (initSize == Vector2.zero)
            initSize = obj.GetComponent<RectTransform>().sizeDelta;

        int height = Mathf.Max(2, Mathf.CeilToInt((float)obj.GetComponentInChildren<Text>().text.Length / 35f));
        obj.GetComponent<RectTransform>().sizeDelta = initSize * new Vector3(1f, height * 0.5f);
    }
    
}
