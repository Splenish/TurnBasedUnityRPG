﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn{

    // Hyökkääjän nimi
    public string Attacker;

    // kuka hyökkää
    public GameObject AttackersGameObject;

    // kehen hyökätään
    public GameObject AttackersTarget;
    
    // mikä hyökkäys tehdään

}