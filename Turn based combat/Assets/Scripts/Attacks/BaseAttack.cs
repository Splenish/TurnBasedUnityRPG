using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseAttack : MonoBehaviour{

    public string attackName;//attacks name
    public string attackDescription;
    public float attackDamage;//Base dmg 15(melee attack) without items/level. lvl 10 strenght 35 = basedmg + (strenght - (lvl/2)
    public float healAmount;//heal amount
    public float attackCost;//Manacost


}
