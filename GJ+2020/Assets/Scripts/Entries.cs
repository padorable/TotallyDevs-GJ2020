using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entries : MonoBehaviour
{
    public static Entries instance;
    public List<DiaryEntryHolder> Diaries;
    public GameObject ButtonObject;
    public GameObject Content;

    private int index = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this);

        ShowButton(0);
    }

    public void ShowButton(int n)
    {
        index = n;
        ButtonObject.SetActive(true);
        ButtonObject.GetComponent<Button>().onClick.RemoveAllListeners();
        ButtonObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            DiaryEntryHolder holder = Instantiate(Diaries[index]);
            holder.transform.SetParent(Content.transform);
            holder.transform.localScale = Vector3.one;
            ButtonObject.SetActive(false);

            if (GameManager.instance.WeekNumber >= 12)
            {
                holder.OnOpen.AddListener(() =>
                {
                    EndingImage.instance.ShowImage(n - 1);
                });
            }
        });
    }
}
