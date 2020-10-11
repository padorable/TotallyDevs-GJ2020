﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDone : MonoBehaviour
{

    public static ActionDone instance;

    public List<ActionPicture> pictures;
    public Image img;
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
        StartCoroutine(a(t,l));
    }

    IEnumerator fade(bool fadeIn)
    {
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
    }

    IEnumerator a(Stat t, int l)
    {
        ActionPicture ap = pictures.Find((x) => x.type == t && x.level == l);

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

    public List<Sprite> sprites;
}