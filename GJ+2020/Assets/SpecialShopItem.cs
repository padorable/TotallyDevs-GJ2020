using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialShopItem : ShopItem
{
    public float givePercent;

    public override void OnBuy()
    {
        DataHandler.SetBonusStat(stat, DataHandler.GetBonus(stat) + givePercent);
    }
}
