using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Stat stat;
    public int LevelToUnlock;
    public int Cost;
    private Button button;
    private Text buttonText;

    private string origText;

    private void Start()
    {
        button = this.GetComponentInChildren<Button>();
        buttonText = button.transform.GetChild(0).GetComponent<Text>();
        buttonText.text = "Buy " + Cost;
        origText = buttonText.text;
    }

    public void Buy()
    {
        if(Cost <= DataHandler.Money)
        {
            DataHandler.Money -= Cost;

            if(LevelToUnlock >= 0)
                GameManager.instance.UnlockItem(stat, LevelToUnlock);

            //SpriteToUnlock.SetActive(true);
            this.GetComponentInChildren<Button>().interactable = false;

            if (stat != Stat.Nourishment)
            {
                this.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Text>().text = "Bought";
            }
            else
            {
                this.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Text>().text = "Out of Stock";
            }

            OnBuy();

            //if (stat == Stat.Nourishment)
            //{
            //    GameManager.instance.Data.GetDataValue(stat).Choices[LevelToUnlock].Amount++;

            //    UnityAction action = () =>
            //    {
            //        this.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Text>().text = origText;
            //        this.GetComponentInChildren<Button>().interactable = true;
            //    };

            //    GameManager.instance.NewWeek.AddListener(action);
            //    GameManager.instance.NewWeek.AddListener(() => GameManager.instance.NewWeek.RemoveListener(action));
            //}
        }
    }

    public virtual void OnBuy()
    {

    }
}
