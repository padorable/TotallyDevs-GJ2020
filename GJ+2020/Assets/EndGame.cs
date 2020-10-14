using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    bool isStart = true;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        if(!isStart)
            yield return new WaitForSeconds(3.0f);

        Image image = GetComponent<Image>();
        Debug.Log("A");
        float elapsedTime = 0, duration = 0;
        if (isStart)
            duration = 1.5f;
        else
            duration = 5f;

        while (elapsedTime < duration)
        {
            elapsedTime = Mathf.Min((elapsedTime + Time.deltaTime), duration);
            Color c = image.color;

            if (isStart)
                c.a = 1 - (elapsedTime / duration);
            else
                c.a = elapsedTime / duration;

            image.color = c;

            yield return null;
        }
        yield return null;

        if(!isStart)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        else
        {
            isStart = false;
            this.gameObject.SetActive(false);
        }
    }
}
