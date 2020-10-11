using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> BG; public List<AudioClip> FX;
    private AudioSource source;

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = BG[0];
        source.Play();
        GameManager.instance.NewWeek.AddListener(() =>
        {
            if (GameManager.instance.WeekNumber % 4 == 0)
            {
                index = Mathf.Min(index + 1, BG.Count - 1);
                StartCoroutine(switchAudio(BG[index]));
            }
        });

        GameManager.instance.OnChangedActionPoints.AddListener(() => source.PlayOneShot(FX[1]));
    }

    IEnumerator switchAudio(AudioClip clip)
    {
        float d = 0, a = 2f;

        while(a > d)
        {
            d = Mathf.Min(d + Time.deltaTime, a);
            source.volume = (1 - d / a);
            yield return null;
        }

        source.clip = clip;

        d = 0;
        while (a > d)
        {
            d = Mathf.Min(d + Time.deltaTime, a);
            source.volume =  d / a;
            yield return null;
        }

        source.Play();
    }

    public void playfx(int i)
    {
        AudioSource.PlayClipAtPoint(FX[i], Vector3.zero);
    }
}
