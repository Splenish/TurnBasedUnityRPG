using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {


	public static Player Instance;

	void Awake () {

		if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;

			remainingMovement = moveSpeed;
			anim = this.gameObject.GetComponentInChildren<Animator> ();
			gm = GameObject.Find ("GameManager");
		} else if(Instance != this) {
			Destroy(gameObject);
		}


		//currentUnit = gm.GetComponent<GameManager>().currentUnit;
		//currentUnit = this.gameObject;


		moveText.text = remainingMovement.ToString() + "/" + moveSpeed.ToString();
	}


	public void MoveUnitButton() {
		Debug.Log ("moveinit button");
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;
		if (gs == GameManager.GameState.myTurn) {
			if (remainingMovement > 0) {
				if (!moving && currentPath != null) {
					if (firstMove) {
						if (currentPath.Count != 1) {
							//remainingMovement--;
							moveText.text = remainingMovement.ToString () + "/" + moveSpeed.ToString ();
						}
						firstMove = false;
					}
					moving = true;
				}
			}
		}
	}

	public void StartTurn() {
		remainingMovement = moveSpeed;
		//anim = currentUnit.GetComponentInChildren<Animator> ();

		if (moveText == null)
			moveText = GameObject.Find ("MoveText").GetComponent<UnityEngine.UI.Text>();

		moveText.text = remainingMovement.ToString () + "/" + moveSpeed.ToString ();
		gm.GetComponent<GameManager> ().i = 0;
		currentPath = null;
		if(currentPath == null)
			firstMove = true;
	}

}
