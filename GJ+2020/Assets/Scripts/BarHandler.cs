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

    public Color SafeMeter;
    public Color AlertMeter;

    private void Awake()
    {
        barAssist = this.transform.GetChild(1).GetComponent<Image>();
        bar = barAssist.transform.GetChild(0).GetComponent<Image>();
        Debug.Log(bar);
    }

    IEnumerator moveBar(Image b, float percent)
    {
        elapsedTime = 0;
        float initialPercent = b.fillAmount;

        while(elapsedTime < duration)
        {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, duration);
            b.fillAmount = Mathf.Lerp(initialPercent, percent, elapsedTime / duration);

            if (b == bar)
            {
                if (b.fillAmount < .34)
                {
                    b.color = AlertMeter;
                }
                else
                {
                    b.color = SafeMeter;
                }
            }

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
        if (!this.gameObject.activeSelf) return;

        barAssist.fillAmount = percent;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(moveBar(bar, percent));
    }

    public void SetBarFast(float percent)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        barAssist.fillAmount = percent;
        bar.fillAmount = percent;
    }

    public void ReturnAssistBar()
    {
        if (barAssist.fillAmount == bar.fillAmount) return;

        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        barAssist.fillAmount = barAssistPercent;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(moveBar(barAssist, bar.fillAmount));
    }
}
