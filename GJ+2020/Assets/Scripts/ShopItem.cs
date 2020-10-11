using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Stat stat;
    public int LevelToUnlock;
    public int Cost;
    public bool IsBought = false;
    private Button button;
    private Text buttonText;

    private void Start()
    {
        button = this.GetComponentInChildren<Button>();
        buttonText = button.transform.GetChild(0).GetComponent<Text>();
        buttonText.text = "Buy " + Cost;
    }

    public void Buy()
    {
        if(Cost <= DataHandler.Money)
        {
            DataHandler.Money -= Cost;
            GameManager.instance.Data.GetDataValue(stat).Choices[LevelToUnlock].IsUnlocked = true;
            IsBought = true;
            this.GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
            this.GetComponentInChildren<UnityEngine.UI.Button>().transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Bought";
        }
    }
}
