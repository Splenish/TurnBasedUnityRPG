using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

	public int aggroRange;

	public Vector3 target;

	GameObject player;

	GameObject aggroTrigger;

	public GameObject grid;


	int enemyUnits;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		gm = GameObject.Find ("GameManager");
		enemyUnits = gm.GetComponent<GameManager> ().enemyUnits.Length;
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
		if (gs == GameManager.GameState.enemyTurn && gm.GetComponent<GameManager> ().currentUnit.gameObject == this.gameObject) {
			//Debug.Log ("boblet");
			//Debug.Log (gm.GetComponent<GameManager> ().currentUnit + "EnemyUnitis");
			//Debug.Log ("EnemyUnit lateUpdate if gs == enemyturn");
	//		Debug.Log (currentPath.Count);
			if (checkIfInAggrorange ()) {
				//Debug.Log ("if aggrorange");
				moving = true;
				if (firstMove) {
					remainingMovement--;
					firstMove = false;
				}		

				FinishEnemyTurn ();
				MoveUnit ();
			} else {
				//FinishEnemyTurn ();
				//int enemyUnits = gm.GetComponent<GameManager>().enemyUnits.Length;
				gm.GetComponent<GameManager> ().i++;
				Debug.Log (gm.GetComponent<GameManager> ().i);
				if (gm.GetComponent<GameManager> ().i > enemyUnits - 1) {
					//Debug.Log ("koklet");
					gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
					player.GetComponent<Unit> ().StartTurn ();
				}	}
		}
	}

	bool checkIfInAggrorange() {
		//Debug.Log ("checkIfInAggrorange alku");
		//Debug.Log (currentPath);
		if (currentPath != null) {
			//Debug.Log ("checkIfInAggrorange if currentPath != null");
			if (currentPath.Count <= aggroRange) {
			//Debug.Log ("aggrorange == true");
				return true;
			} else {
			//Debug.Log ("aggrorange == false");
				return false;
			}
		} else {
			return false;
		}
	}

	void FinishEnemyTurn() {
		if (Vector3.Distance (transform.position, currentPath [0].worldPosition) < .02f) {
			if (remainingMovement >= 0) {
				gm.GetComponent<GameManager> ().i++;
				if (gm.GetComponent<GameManager> ().i > enemyUnits - 1) {
					//Debug.Log ("koklet");
					gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
					player.GetComponent<Unit> ().StartTurn ();
				}
				//gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
				//player.GetComponent<Unit> ().StartTurn ();
				//Debug.Log (gm.GetComponent<GameManager> ().CurrentGameState);
				//Debug.Log ("finish enemy turn");
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
