using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BaseAttack {

    public Slash()
    {
        attackName = "Slash";
        attackDescription = "Slash target for 500 damage";
        attackCost = 150f;
        attackDamage = 500f;
    }
	
}
