using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour {
    public GameObject CombatMenu;
    public GameObject AttackMenu;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowAttackMenu()
    {
        Debug.Log("paska");
        CombatMenu.SetActive(false);
        AttackMenu.SetActive(true);
    }
}
