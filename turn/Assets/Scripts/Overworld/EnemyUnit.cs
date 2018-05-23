using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

	public int aggroRange;

	public Vector3 target;

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		gm = GameObject.Find ("GameManager");
		remainingMovement = moveSpeed;
		currentUnit = this.gameObject;
		anim = currentUnit.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		target = player.transform.position;

		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;
		if (gs == GameManager.GameState.enemyTurn) {
			if (checkIfInAggrorange ()) {
				moving = true;
				//Debug.Log ("REEEEEEEEEEEEEEE");
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

}
