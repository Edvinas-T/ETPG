using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack:MonoBehaviour
{
    public string attackName;
    public string attackDesc;
    public float attackDamage;
    public float attackMana; //if attack uses mana
}
