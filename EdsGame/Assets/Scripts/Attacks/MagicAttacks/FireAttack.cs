using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : BaseAttack
{
    public FireAttack()
    {
        attackName = "Fire Breath";
        attackDesc = "Powerful but Mana hungry spell.";
        attackDamage = 60f;
        attackMana = 25f;
    }
}
