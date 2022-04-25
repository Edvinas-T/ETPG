using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleEnemy: BaseClass
{

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC,
        LEGENDARY
    }
    public Rarity rarity;


}
