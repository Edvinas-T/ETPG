using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleEnemy
{
    public string name;
    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC,
        LEGENDARY
    }
    public Rarity rarity;


  
    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public int strength;
    public int intellect;
    public int agilty;

    public float baseATK;
    public float curATK;
    public float baseDEF;
    public float curDEF;

}
