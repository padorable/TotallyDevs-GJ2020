using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChecker : MonoBehaviour
{
    public List<Checking> ToCheck;
    public static SpriteChecker ah;

    public void Check(Stat type, int lvl)
    {
        Checking c = ToCheck.Find((x) => x.type == type && x.unlockLevel == lvl);
        if(c != null)
        {
            c.obj.SetActive(true);
            ToCheck.Remove(c);
        }
    }
}

public class Checking
{
    public GameObject obj;
    public Stat type;
    public int unlockLevel;
}
