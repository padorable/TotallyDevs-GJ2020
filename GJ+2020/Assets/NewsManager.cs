using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    public NewsTab TabPrefab;

    public GameObject ContentObject;

    public List<NewsScriptable> News;

    public NewsTabManager NewsTab;

    private void Start()
    {
        GameManager.instance.NewWeek.AddListener(AddNews);
        AddNews();
    }
    public void AddNews()
    {
        if (News.Count - 1 < GameManager.instance.WeekNumber) return;
        Debug.Log("eh");
        NewsTab obj = Instantiate(TabPrefab);
        obj.OnOpenNews.AddListener(OpenNewsTab);
        obj.transform.SetParent(ContentObject.transform);
        obj.transform.SetSiblingIndex(1);
        obj.SetNewsTab(News[GameManager.instance.WeekNumber], GameManager.instance.WeekNumber);
        obj.transform.localScale = Vector3.one;
    }

    public void OpenNewsTab(NewsTab news)
    {
        NewsTab.SetNews(news);
        PhoneManager.instance.ShowNextScreen(NewsTab.gameObject);
    }
}
