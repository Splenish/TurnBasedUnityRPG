using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseEnemy{

    public string name;


    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;
    public float baseDEF;
    public float curDEF;

    public enum Type
    {
        Grass,
        Fire,
        Water,
        Electric
    }

    public enum Rarity
    {
        common,
        uncommon,
        rare,
        superrare
    }

    public Type EnemyType;
    public Rarity rarity;




	
}
