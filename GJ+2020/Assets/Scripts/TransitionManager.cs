using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Text CurrentMonth;
    public Text NextMonth;
    public Text CurrentWeek;
    public Text NextWeek;

    public Animator animator;

    public static TransitionManager instance;

    public UnityEvent BeforeTransition = new UnityEvent();
    public UnityEvent AfterTransition = new UnityEvent();
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        canvasGroup = this.GetComponent<CanvasGroup>();
    }
    public void SetDate()
    {
        StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        if (BeforeTransition != null)
            BeforeTransition.Invoke();

        CurrentMonth.text = ReturnMonth(GameManager.instance.WeekNumber - 1);
        NextMonth.text = ReturnMonth(GameManager.instance.WeekNumber);

        CurrentWeek.text = ((GameManager.instance.WeekNumber - 1) % 4 + 1).ToString();
        NextWeek.text = ((GameManager.instance.WeekNumber ) % 4 + 1).ToString();

        if (((GameManager.instance.WeekNumber) % 4) == 0)
        {
            animator.Play("TransitionAnimation", -1, 0);
        }
        else
        {
            animator.Play("TransitionWeek", -1, 0);
        }

        float duration = .5f, elapsedTime = 0;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0;
        while(elapsedTime < duration)
        {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, duration);
            canvasGroup.alpha = elapsedTime / duration;
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, duration);
            canvasGroup.alpha = 1 - (elapsedTime / duration);
            yield return null;
        }

        if (AfterTransition != null)
            AfterTransition.Invoke();
        canvasGroup.blocksRaycasts = false;
    }

    private string ReturnMonth(int week)
    {
        switch(Mathf.FloorToInt(week/4))
        {
            case 0: return "OCT";
            case 1: return "NOV";
            case 2: return "DEC";

            default: return "DEC";
        }
    }
}
