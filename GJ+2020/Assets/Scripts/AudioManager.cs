using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioVolume> BG; public List<AudioVolume> FX;
    private AudioSource source;

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
        source.clip = BG[0].Clip;
        source.volume = BG[0].Volume;
        source.Play();
        GameManager.instance.NewWeek.AddListener(() =>
        {
            if (GameManager.instance.WeekNumber % 4 == 0 && GameManager.instance.WeekNumber != 0)
            {
                index = Mathf.Min(index + 1, BG.Count - 1);
                StartCoroutine(switchAudio(BG[index]));
            }
        });

        GameManager.instance.OnChangedActionPoints.AddListener(() => source.PlayOneShot(FX[1].Clip,FX[1].Volume));
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

    public void playfx(int i)
    {
        AudioSource.PlayClipAtPoint(FX[i].Clip, Vector3.zero,FX[i].Volume);
    }
}

[System.Serializable]
public struct AudioVolume
{
    public AudioClip Clip;
    [Range(0,1)]
    public float Volume;
}