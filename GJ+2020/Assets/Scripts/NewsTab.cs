using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OpenNews : UnityEvent<NewsTab> { }

public class NewsTab : MonoBehaviour
{
    public NewsScriptable CurrentNews;

    public OpenNews OnOpenNews = new OpenNews();
    public Text Title;
    public int NewsWeekReleasedOn;

    public void SetNewsTab(NewsScriptable news, int newsWeek)
    {
        CurrentNews = news;
        NewsWeekReleasedOn = newsWeek;
        Title.text = CurrentNews.NewsTitle;
    }
    public void Open()
    {
        OnOpenNews.Invoke(this);
    }
}
