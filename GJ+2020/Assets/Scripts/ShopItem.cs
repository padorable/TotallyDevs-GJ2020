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
    public bool IsBought = false;
    private Button button;
    private Text buttonText;
    public GameObject SpriteToUnlock;

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
            GameManager.instance.Data.GetDataValue(stat).Choices[LevelToUnlock].IsUnlocked = true;
            IsBought = true;
            SpriteToUnlock.SetActive(true);
            this.GetComponentInChildren<Button>().interactable = false;
            this.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Bought";
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
