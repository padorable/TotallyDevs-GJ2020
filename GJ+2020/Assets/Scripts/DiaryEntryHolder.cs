using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiaryEntryHolder : MonoBehaviour
{
    public DiaryEntry DiaryEntryObject;
    public UnityEvent OnOpen = new UnityEvent();
    private void Awake()
    {
        DiaryEntryObject = Instantiate(DiaryEntryObject);
    }

    public void OpenDiary()
    {
        if (OnOpen != null)
            OnOpen.Invoke();

        DiaryManager.instance.StartEntry(DiaryEntryObject);
        PhoneManager.instance.ShowNextScreen(DiaryManager.instance.gameObject);
    }
}
