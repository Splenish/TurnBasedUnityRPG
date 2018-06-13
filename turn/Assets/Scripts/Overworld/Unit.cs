using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {

	public int moveSpeed;
	public int remainingMovement;

	public bool moving = false;

	public List<Node> currentPath = null;

	Vector3 tilePosToMoveTo, direction;

	Quaternion rotation;

	public GameObject currentUnit;

	public GameObject gm;

	public Animator anim;

	public float rotationSpeed;

	public bool firstMove = true;

	public Text moveText;

	public float animationMoveSpeed;

	// Use this for initialization
	void Start () {
		remainingMovement = moveSpeed;
		//currentUnit = gm.GetComponent<GameManager>().currentUnit;
		currentUnit = this.gameObject;
		anim = currentUnit.GetComponentInChildren<Animator> ();
		gm = GameObject.Find ("GameManager");
		moveText.text = remainingMovement.ToString() + "/" + moveSpeed.ToString();
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
			return;
		}

		remainingMovement -= 1;
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;
		if(gs == GameManager.GameState.myTurn)
			moveText.text = remainingMovement.ToString() + "/" + moveSpeed.ToString();;

		transform.position = currentPath [0].worldPosition;

		//remove the old current tile
		currentPath.RemoveAt(0);
		if (currentPath.Count == 0) {
			moving = false;
			currentPath = null;
		}
	}

	public void MoveUnitButton() {
		if (remainingMovement > 0) {
			if (!moving && currentPath != null) {
				if (firstMove) {
					if (currentPath.Count != 1) {
						remainingMovement--;
						moveText.text = remainingMovement.ToString () + "/" + moveSpeed.ToString ();
					}
					firstMove = false;
				}

				moving = true;
			}
		}
	}

	/*
	public void NextTurn() {
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;

		switch (gs) {
		case GameManager.GameState.myTurn:
			Debug.Log ("Playerin vuoro loppuu");
			gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.enemyTurn;
			return;
		case GameManager.GameState.enemyTurn:
			Debug.Log ("Enemyn vuoro loppuu");
			gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.myTurn;
			remainingMovement = moveSpeed;
			firstMove = true;
			moveText.text = remainingMovement.ToString () + "/" + moveSpeed.ToString ();
			break;
		}
	}
	*/

	public void MoveUnit() {
		if (currentPath != null && moving ) {

			//Debug.Log(Vector3.Distance (transform.position, currentPath [0].worldPosition));
			//Debug.Log (transform.position);
			//Debug.Log (currentPath[0].worldPosition);

			if (Vector3.Distance (transform.position, currentPath [0].worldPosition) < .2f) {
				MoveToNextTile ();
			}

			if(currentPath != null)
				tilePosToMoveTo = currentPath [0].worldPosition;

			direction = tilePosToMoveTo - transform.position;
			direction = Vector3.RotateTowards (transform.forward, direction, rotationSpeed*Time.deltaTime, 0.0f);

			transform.rotation = Quaternion.LookRotation (direction);

			transform.position = Vector3.MoveTowards(transform.position,tilePosToMoveTo,animationMoveSpeed);

		}


		if (moving)
			anim.SetBool ("isMoving", true);
		if (!moving)
			anim.SetBool ("isMoving", false);
	}

	public void StartTurn() {
		remainingMovement = moveSpeed;
		//anim = currentUnit.GetComponentInChildren<Animator> ();
		moveText.text = remainingMovement.ToString () + "/" + moveSpeed.ToString ();
		gm.GetComponent<GameManager> ().i = 0;
		if(currentPath == null)
			firstMove = true;
	}

}