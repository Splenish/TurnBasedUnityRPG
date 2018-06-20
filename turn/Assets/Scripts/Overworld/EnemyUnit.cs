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
		//remainingMovement = moveSpeed;
		currentUnit = this.gameObject;
		anim = currentUnit.GetComponent<Animator> ();
		grid = GameObject.Find ("A*");
		float combatTriggerSize = grid.GetComponent<GameGrid> ().nodeRadius * aggroRange * 2 - 3;
		aggroTrigger = gameObject.transform.Find ("CombatTrigger").gameObject;
		aggroTrigger.transform.localScale = new Vector3 (combatTriggerSize, 1, combatTriggerSize);
		moving = false;
		Debug.Log ("start");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		target = player.transform.position;
		aggroTrigger.transform.localRotation = Quaternion.Inverse (transform.rotation);
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;
		//Debug.Log ("gs enemyUnitis: " + gs);
		if (gs == GameManager.GameState.enemyTurn && gm.GetComponent<GameManager> ().currentUnit.gameObject == this.gameObject) {
			if (checkIfInAggrorange ()) {
				Debug.Log ("if in aggrorange");
				moving = true;
				if (firstMove) {
					//remainingMovement--;
					firstMove = false;
				}		

				FinishEnemyTurn ();
				MoveUnit ();
			} else {
				gm.GetComponent<GameManager> ().i++;
				remainingMovement = moveSpeed;
				Debug.Log ("GM:n i: " + gm.GetComponent<GameManager> ().i);
				if (gm.GetComponent<GameManager> ().i > enemyUnits - 1) {
					Debug.Log ("PLayerin vuorolle");
					gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
					player.GetComponent<Player> ().StartTurn ();
				}	
			}
		}
	}

	bool checkIfInAggrorange() {
		if (currentPath != null) {
			if (currentPath.Count <= aggroRange) {
				Debug.Log ("Aggrorange");
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
			if (remainingMovement <= 0) {
				gm.GetComponent<GameManager> ().i++;
				if (gm.GetComponent<GameManager> ().i > enemyUnits - 1) {
					Debug.Log ("koklet");
					gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
					player.GetComponent<Player> ().StartTurn ();
				}
			}
		}
	}

	public void StartEnemyTurn() {
		Debug.Log ("start enemyturn");
		Debug.Log ("minka enemyunit script: " + this.gameObject);
		remainingMovement = moveSpeed;
		firstMove = true;
	}
		

	public void PullTrigger(Collider c) {
		if (c.gameObject.tag == "Player") {
			gm.GetComponent<GameManager> ().StartCombat ();
		}
	}
}
