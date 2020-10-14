using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDone : MonoBehaviour
{
    public static ActionDone instance;

    public List<ActionPicture> pictures;
    public Image img;

    public GameObject WallObject;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void DoAction(Stat t, int l)
    {
        ActionPicture ap = pictures.Find((x) => x.type == t && x.level == l);
        AudioManager.instance.source.PlayOneShot(ap.clip);
        DialogueManager.instance.SetDialogue(ap.Dialogue[Mathf.FloorToInt(GameManager.instance.WeekNumber/4)]);
        StartCoroutine(a(ap));
    }

    IEnumerator fade(bool fadeIn)
    {
        if (fadeIn)
            WallObject.SetActive(true);

        float d = .4f, b = 0;
        Color c = img.color;
        while (b < d)
        {
            b += Time.deltaTime;
            if(fadeIn)
                c.a = Mathf.Min(b / d, 1);
            else
                c.a = Mathf.Max(1 - (b / d), 0);
            img.color = c;
            yield return null;
        }
        if(!fadeIn)
            WallObject.SetActive(false);
    }

    IEnumerator a(ActionPicture ap)
    {
        StartCoroutine(fade(true));
        if (ap != null)
        {
            float duration = .3f;
            int amountOfTimes = 10;
            bool o = true;
            for(int i = 0; i < amountOfTimes; i++)
            {
                if(o)
                {
                    img.sprite = ap.sprites[0];
                }
                else
                {
                    img.sprite = ap.sprites[1];
                }
                o = !o;
                yield return new WaitForSeconds(duration);
            }

        }
        StartCoroutine(fade(false));

    }
}

[System.Serializable]
public class ActionPicture
{
    public Stat type;
    public int level;
    public AudioClip clip;

    public List<Sprite> sprites;
    public List<string> Dialogue;
}
