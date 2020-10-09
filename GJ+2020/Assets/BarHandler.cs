using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum Stat
{
    Social,
    Mood,
    Fitness,
    Nourishment
}

public class BarHandler : MonoBehaviour
{
    const float duration = .1f;

    private Image bar;
    private Image barAssist;
    private float barAssistPercent = 0;
    private float elapsedTime = 0;
    private Coroutine currentCoroutine;
    public Stat CurrentStat;

    private void Awake()
    {
        barAssist = this.transform.GetChild(1).GetComponent<Image>();
        bar = barAssist.transform.GetChild(0).GetComponent<Image>();
        Debug.Log(bar);
    }

    IEnumerator moveBar(Image bar, float percent)
    {
        elapsedTime = 0;
        float initialPercent = bar.fillAmount;
        while(elapsedTime < duration)
        {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, duration);
            bar.fillAmount = Mathf.Lerp(initialPercent, percent, elapsedTime / duration);
            yield return null;
        }
    }

    public void SetAssistBar(float percent)
    {
        barAssistPercent = percent;
        barAssist.fillAmount = bar.fillAmount;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(moveBar(barAssist, percent));
    }

    public void SetBar(float percent)
    {
        barAssist.fillAmount = percent;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(moveBar(bar, percent));
    }

    public void ReturnAssistBar()
    {
        StopCoroutine(currentCoroutine);
        barAssist.fillAmount = barAssistPercent;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(moveBar(barAssist, bar.fillAmount));
    }
}
