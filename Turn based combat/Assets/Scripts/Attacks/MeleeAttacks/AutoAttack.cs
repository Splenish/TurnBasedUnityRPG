using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : BaseAttack {

	public AutoAttack()
    {
        attackName = "BasicAttack";
        attackDescription = "Basic Attack";
        attackDamage = 250f;
        attackCost = 0f;
    }

}
