﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

	public int aggroRange;

	public Vector3 target;

	GameObject player;

	GameObject aggroTrigger;

	public GameObject grid;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		gm = GameObject.Find ("GameManager");
		remainingMovement = moveSpeed;
		currentUnit = this.gameObject;
		anim = currentUnit.GetComponent<Animator> ();
		grid = GameObject.Find ("A*");
		float combatTriggerSize = grid.GetComponent<GameGrid> ().nodeRadius * aggroRange * 2 - 3;
		aggroTrigger = gameObject.transform.Find ("CombatTrigger").gameObject;
		aggroTrigger.transform.localScale = new Vector3 (combatTriggerSize, 1, combatTriggerSize);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		target = player.transform.position;
		aggroTrigger.transform.localRotation = Quaternion.Inverse (transform.rotation);
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;
		if (gs == GameManager.GameState.enemyTurn) {
			if (checkIfInAggrorange ()) {
				moving = true;
				if (firstMove) {
					remainingMovement--;
					firstMove = false;
				}		

				FinishEnemyTurn ();
				MoveUnit ();
			} else {
				//FinishEnemyTurn ();
				gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
				player.GetComponent<Unit> ().StartTurn ();
			}
		}
	}

	bool checkIfInAggrorange() {
		if (currentPath != null) {
			if (currentPath.Count <= aggroRange) {
				
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	void FinishEnemyTurn() {
		if (Vector3.Distance (transform.position, currentPath [0].worldPosition) < .02f) {
			if (remainingMovement >= 0) {
				gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
				player.GetComponent<Unit> ().StartTurn ();
				Debug.Log (gm.GetComponent<GameManager> ().CurrentGameState);
				Debug.Log ("finish enemy turn");
			}
		}

	}

	public void StartEnemyTurn() {
		remainingMovement = moveSpeed;
		firstMove = true;
	}
		

	public void PullTrigger(Collider c) {
		if (c.gameObject.tag == "Player") {
			gm.GetComponent<GameManager> ().StartCombat ();
		}
	}
}