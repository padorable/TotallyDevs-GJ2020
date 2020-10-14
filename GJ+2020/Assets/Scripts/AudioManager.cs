using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioVolume> BG; public List<AudioVolume> FX;
    public AudioSource source;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        source = this.GetComponent<AudioSource>();
    }

    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = BG[6].Clip;
        source.volume = BG[6].Volume;
        source.Play();
        GameManager.instance.NewWeek.AddListener(() =>
        {
            PlayFX(5);
            if (GameManager.instance.WeekNumber % 4 == 0 && GameManager.instance.WeekNumber != 0)
            {
                if (GameManager.instance.WeekNumber != 12)
                {
                    index = Mathf.Min(index + 1, BG.Count - 1);
                    StartCoroutine(switchAudio(BG[index]));
                }
            }
        });

        //GameManager.instance.OnChangedActionPoints.AddListener(() => source.PlayOneShot(FX[1].Clip,FX[1].Volume));
    }

    IEnumerator switchAudio(AudioVolume clip)
    {
        float d = 0, a = 2f,initVol = source.volume;
        while(a > d)
        {
            d = Mathf.Min(d + Time.deltaTime, a);
            source.volume = (1 - d / a) * initVol;
            yield return null;
        }

        source.clip = clip.Clip;

        d = 0;
        while (a > d)
        {
            d = Mathf.Min(d + Time.deltaTime, a);
            source.volume =  (d / a) * clip.Volume;
            yield return null;
        }

        source.Play();
    }

    public void PlayFX(int i)
    {
        AudioSource.PlayClipAtPoint(FX[i].Clip, Vector3.zero,FX[i].Volume);
    }

    public void PlayBG(int i)
    {
        StartCoroutine(switchAudio(BG[i]));
    }
}

[System.Serializable]
public struct AudioVolume
{
    public AudioClip Clip;
    [Range(0,1)]
    public float Volume;
}