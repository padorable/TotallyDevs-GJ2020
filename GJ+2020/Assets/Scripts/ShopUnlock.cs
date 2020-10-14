using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUnlock : MonoBehaviour
{
    public List<LockedItems> Locked;
    public Text Credits;
    public GameObject Content;

    private void Start()
    {
        PhoneManager.instance.OnOpenNewScreen.AddListener((x) =>
        {
            if (x != this.gameObject) return;

            AudioManager.instance.PlayFX(6);
            foreach(ShopItem item in Content.GetComponentsInChildren<ShopItem>())
            {
                if(item.gameObject.activeSelf)
                    item.Refresh();
            }

            ScrollToTop();
        });
    }

    public void checkItems()
    {
        foreach(Relationship r in DataHandler.Relationships)
        {
            for (int i = 0; i < r.Level; i++)
            {
                LockedItems locked = Locked.Find((x) => x.ItemStat == r.Unlock[i].stat && x.LevelToUnlock == r.Unlock[i].LevelToUnlock);
                if (locked != null)
                {
                    locked.Item.SetActive(true);
                    Locked.Remove(locked);
                }
            }
        }
    }

    private void Update()
    {
        Credits.text = "BHOPEE Credits: " + DataHandler.Money;
    }

    public void ScrollToTop()
    {
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
}

[System.Serializable]
public class LockedItems
{
    public GameObject Item;
    public Stat ItemStat;
    public int LevelToUnlock;
}