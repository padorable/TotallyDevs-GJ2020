using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsTabManager : MonoBehaviour
{
    public Text Title;
    public Text Date;

    public Text News;

    public void SetNews(NewsTab news)
    {
        Title.text = news.CurrentNews.NewsTitle;

        Date.text = GameManager.instance.Month(news.NewsWeekReleasedOn, false) + ",\nWeek " + (news.NewsWeekReleasedOn % 4 + 1);
        News.transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        News.text = news.CurrentNews.ActualNews;

        StartCoroutine(ta());
    }

    IEnumerator ta()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        News.transform.parent.GetComponent<ContentSizeFitter>().enabled = true;
    }
}
