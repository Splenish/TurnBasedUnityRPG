using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverWorldUI : MonoBehaviour {

	public Text moveText;


	GameObject currentUnit;

	GameObject gm;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager");
		currentUnit = GameObject.Find ("A*").GetComponent<Pathfinding> ().currentUnit;
		moveText.text = currentUnit.GetComponent<Unit>().remainingMovement.ToString() + "/" + currentUnit.GetComponent<Unit>().moveSpeed.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NextTurn() {
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;

		if (gs == GameManager.GameState.myTurn) {

			gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.enemyTurn;

			//currentUnit.GetComponent<Unit> ().anim = currentUnit.GetComponent<Animator> ();

			//currentUnit.GetComponent<EnemyUnit> ().StartEnemyTurn ();

		}

	}
}
