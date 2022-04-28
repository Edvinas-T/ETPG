using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttack : BaseAttack
{
    public PoisonAttack()
    {
        attackName = "Poison Mist";
        attackDesc = "Damage over time spell";
        attackDamage = 20f;
        attackMana = 15f;
    }
}
