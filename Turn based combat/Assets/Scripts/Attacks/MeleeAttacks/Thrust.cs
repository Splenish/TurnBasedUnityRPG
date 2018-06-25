using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust :  BaseAttack {

    public Thrust()
    {
        attackName = "Thrust";
        attackDescription = "Thrusts with a weapon. More powerful than Basic Attack";
        attackDamage = 300f;
        attackCost = 0f;
    }
	
}
