using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingImage : MonoBehaviour
{
    private Image image;
    public List<Sprite> Endings;
    public int Ending = 0;

    public static EndingImage instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        image = GetComponent<Image>();
    }

    public void ShowImage(int i)
    {
        image.sprite = Endings[i];
        StartCoroutine(FadeIn());
        image.raycastTarget = true;
    }
    
    IEnumerator FadeIn()
    {
        float elapsedTime = 0, duration = 1.5f;
        while(elapsedTime < duration)
        {
            elapsedTime = Mathf.Min((elapsedTime + Time.deltaTime), duration);
            Color c = image.color;
            c.a = elapsedTime / duration;

            image.color = c;

            yield return null;
        }
        yield return null;
    }
}
