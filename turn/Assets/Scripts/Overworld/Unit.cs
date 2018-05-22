﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public int moveSpeed;
	int remainingMovement;

	public bool moving = false;

	public List<Node> currentPath = null;

	Vector3 tilePosToMoveTo, direction;

	Quaternion rotation;

	GameObject currentUnit;

	GameObject gm;

	Animator anim;

	public float rotationSpeed;

	public bool firstMove = true;

	// Use this for initialization
	void Start () {
		remainingMovement = moveSpeed;
		currentUnit = GameObject.Find ("Player");
		anim = currentUnit.GetComponentInChildren<Animator> ();
		gm = GameObject.Find ("GameManager");
	}

	// Update is called once per frame
	void Update () {
		MoveUnit ();		
	}


	public void MoveToNextTile() {

		//moving = true;

		if (currentPath == null) {			
			return;
		}

		if (remainingMovement <= 0) {
			moving = false;
			Debug.Log ("movement loppu");
			Debug.Log (remainingMovement);
			return;
		}

		remainingMovement -= 1;

		transform.position = currentPath [0].worldPosition;

		//remove the old current tile
		currentPath.RemoveAt(0);
		if (currentPath.Count == 0) {
			moving = false;
			currentPath = null;
		}
	}

	public void MoveUnitButton() {
		Debug.Log (remainingMovement);
		//while(currentPath != null && remainingMovement > 0) {			
		//MoveToNextTile();
		if (remainingMovement > 0) {
			if (!moving && currentPath != null) {
				if (firstMove) {
					if(currentPath.Count != 1)
						remainingMovement--;
					firstMove = false;
				}

				moving = true;

				//remainingMovement = moveSpeed;
			}
		}
	}


	public void NextTurn() {
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;

		switch (gs) {
		case GameManager.GameState.myTurn:
			Debug.Log ("Playerin vuoro loppuu");
			gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.enemyTurn;
			Debug.Log ("Playerin vuoro loppuu");
			return;
		case GameManager.GameState.enemyTurn:
			Debug.Log ("Enemyn vuoro loppuu");
			gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
			remainingMovement = moveSpeed;
			firstMove = true;
			break;
		}
	}


	public void MoveUnit() {
		if (currentPath != null && moving ) {

			if (Vector3.Distance (transform.position, currentPath [0].worldPosition) < .2f) {
				MoveToNextTile ();
			}

			if(currentPath != null)
				tilePosToMoveTo = currentPath [0].worldPosition;


			//transform.position = Vector3.Lerp (transform.position, tilePosToMoveTo, .4f);
			direction = tilePosToMoveTo - transform.position;
			direction = Vector3.RotateTowards (transform.forward, direction, rotationSpeed*Time.deltaTime, 0.0f);

			transform.rotation = Quaternion.LookRotation (direction);

			transform.position = Vector3.MoveTowards(transform.position,tilePosToMoveTo,0.2f);

		}

		if(moving)
			anim.SetBool ("isMoving", true);
		if(!moving)
			anim.SetBool ("isMoving", false);
	}

}