using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(3.0f);

        Image image = GetComponent<Image>();

        float elapsedTime = 0, duration = 5.0f;
        while (elapsedTime < duration)
        {
            elapsedTime = Mathf.Min((elapsedTime + Time.deltaTime), duration);
            Color c = image.color;
            c.a = elapsedTime / duration;

            image.color = c;

            yield return null;
        }
        yield return null;

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
