using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    public GameObject CurrentScreen;
    private GameObject NextScreen;
    private GameObject PrevScreen;

    [Space]
    public GameObject LockScreen;
    public GameObject HomeScreen;
    public GameObject Buttons;

    public static PhoneManager instance;
    private List<GameObject> PreviouslyVisitedScreens = new List<GameObject>();
    private bool wait = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this);

        CurrentScreen = LockScreen;
        foreach(CanvasGroup g in this.GetComponentsInChildren<CanvasGroup>())
        {
            if(g.gameObject != CurrentScreen)
            {
                g.interactable = false;
                g.blocksRaycasts = false;
                g.alpha = 0;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LockPhone();
    }

    public void ShowNextScreen(GameObject obj)
    {
        if (wait) return;
        if(CurrentScreen != LockScreen)
            PreviouslyVisitedScreens.Add(CurrentScreen);
        NextScreen = obj;
        StartCoroutine(FadeToNext());
    }

    IEnumerator FadeToNext()
    {
        wait = true;
        CanvasGroup c = CurrentScreen.GetComponent<CanvasGroup>();
        CanvasGroup n = NextScreen.GetComponent<CanvasGroup>();

        c.interactable = false;
        c.blocksRaycasts = false;

        float duration = .1f, elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, duration);
            c.alpha = 1 - elapsedTime / duration;
            n.alpha = elapsedTime / duration;

            yield return null;
        }

        n.interactable = true;
        n.blocksRaycasts = true;
        CurrentScreen = NextScreen;
        wait = false;
    }

    public void ResetPhone()
    {
        PrevScreen = null;
        NextScreen = LockScreen;
        StartCoroutine(FadeToNext());
    }

    public void LockPhone()
    {
        Buttons.SetActive(false);
        PrevScreen = CurrentScreen;
        NextScreen = LockScreen;
        StartCoroutine(FadeToNext());
        PreviouslyVisitedScreens.Clear();
    }

    public void UnlockPhone()
    {
        if(PrevScreen == null)
        {
            NextScreen = HomeScreen;
            StartCoroutine(FadeToNext());
        }
        else
        {
            NextScreen = PrevScreen;
            StartCoroutine(FadeToNext());
        }

        Buttons.SetActive(true);
    }

    public void Back()
    {
        if (PreviouslyVisitedScreens.Count == 0)
        {
            return;
        }
        int index = PreviouslyVisitedScreens.Count - 1;
        NextScreen = PreviouslyVisitedScreens[index];
        StartCoroutine(FadeToNext());
        PreviouslyVisitedScreens.RemoveAt(index);
    }

    public void Home()
    {
        if (CurrentScreen == HomeScreen) return;

        PreviouslyVisitedScreens.Clear();
        NextScreen = HomeScreen;
        StartCoroutine(FadeToNext());
    }

    public void InteractButtons(bool a)
    {
        foreach(Button b in Buttons.GetComponentsInChildren<Button>())
        {
            Debug.Log("a");
            b.interactable = a;
        }
    }
}
