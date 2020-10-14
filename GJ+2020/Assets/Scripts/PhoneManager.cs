using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OpenNewScreen : UnityEvent<GameObject> { }

public class PhoneManager : MonoBehaviour
{
    public GameObject CurrentScreen;
    private GameObject NextScreen;
    private GameObject PrevScreen;

    [Space]
    public GameObject LockScreen;
    public GameObject HomeScreen;
    public GameObject StartGameScreen;
    public GameObject Buttons;

    public static PhoneManager instance;
    public OpenNewScreen OnOpenNewScreen = new OpenNewScreen();

    private List<GameObject> PreviouslyVisitedScreens = new List<GameObject>();
    private bool wait = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this);

        CurrentScreen = StartGameScreen;
        foreach(CanvasGroup g in this.GetComponentsInChildren<CanvasGroup>())
        {
            if(g.gameObject != StartGameScreen)
            {
                g.interactable = false;
                g.blocksRaycasts = false;
                g.alpha = 0;
            }
        }
    }

    private void Start()
    {
        GameManager.instance.NewWeek.AddListener(LockPhone);
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
        OnOpenNewScreen.Invoke(NextScreen);
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
        NextScreen = HomeScreen;
        StartCoroutine(FadeToNext());
        Buttons.SetActive(true);
        AudioManager.instance.PlayFX(1);
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
        AudioManager.instance.PlayFX(0);
    }

    public void InteractButtons(bool a)
    {
        foreach(Button b in Buttons.GetComponentsInChildren<Button>())
        {
            b.interactable = a;
        }
    }

    public bool IsStillLocked
    {
        get
        {
            return CurrentScreen == LockScreen;
        }
    }
}
