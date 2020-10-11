using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryEntryHolder : MonoBehaviour
{
    public DiaryEntry DiaryEntryObject;

    private void Awake()
    {
        DiaryEntryObject = Instantiate(DiaryEntryObject);
    }

    public void OpenDiary()
    {
        DiaryManager.instance.StartEntry(DiaryEntryObject);
        PhoneManager.instance.ShowNextScreen(DiaryManager.instance.gameObject);
    }
}
