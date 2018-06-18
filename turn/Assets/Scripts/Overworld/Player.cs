using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {


	void Start () {
		remainingMovement = moveSpeed;
		//currentUnit = gm.GetComponent<GameManager>().currentUnit;
		currentUnit = this.gameObject;
		anim = currentUnit.GetComponentInChildren<Animator> ();
		gm = GameObject.Find ("GameManager");
		moveText.text = remainingMovement.ToString() + "/" + moveSpeed.ToString();
	}


	public void MoveUnitButton() {
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

	public void StartTurn() {
		remainingMovement = moveSpeed;
		//anim = currentUnit.GetComponentInChildren<Animator> ();
		moveText.text = remainingMovement.ToString () + "/" + moveSpeed.ToString ();
		gm.GetComponent<GameManager> ().i = 0;
		currentPath = null;
		if(currentPath == null)
			firstMove = true;
	}

}


