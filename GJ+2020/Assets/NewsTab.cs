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
    public Text Date;
    public int NewsWeekReleasedOn;

    public void SetNewsTab(NewsScriptable news, int newsWeek)
    {
        CurrentNews = news;
        NewsWeekReleasedOn = newsWeek;
        Title.text = CurrentNews.NewsTitle;
        Date.text = GameManager.instance.Month(NewsWeekReleasedOn, true) + ", Week " + (NewsWeekReleasedOn % 4 + 1);
    }
    public void Open()
    {
        OnOpenNews.Invoke(this);
    }
}
