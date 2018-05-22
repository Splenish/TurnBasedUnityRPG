using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public int moveSpeed;
	int remainingMovement;

	public bool moving = false;

	public List<Node> currentPath = null;

	Vector3 tilePosToMoveTo;

	GameObject currentUnit;


	public bool firstMove = true;

	// Use this for initialization
	void Start () {
		remainingMovement = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentPath != null && moving) {

			if (Vector3.Distance (transform.position, currentPath [0].worldPosition) < .2f) {
				//moving = false;
				Debug.Log("distan");
				MoveToNextTile ();
			}

			if(currentPath != null)
				tilePosToMoveTo = currentPath [0].worldPosition;
				
			//transform.position = Vector3.Lerp (transform.position, tilePosToMoveTo, .4f);
			transform.position = Vector3.MoveTowards(transform.position,tilePosToMoveTo,0.2f);

		}
	}


	public void MoveToNextTile() {

		Debug.Log ("MoveToNextTile alku");

		//moving = true;

		if (currentPath == null) {
			Debug.Log ("currentPath = null");			
			return;
		}
		
		if (remainingMovement <= 0) {
			Debug.Log ("remainingMovement = 0");
			moving = false;
			return;
		}

		Debug.Log (remainingMovement);

		remainingMovement -= 1;



		transform.position = currentPath [0].worldPosition;

		//remove the old current tile
		currentPath.RemoveAt(0);
		if (currentPath.Count == 0) {
			Debug.Log ("currentPath count = 0");
			moving = false;
			firstMove = true;
			currentPath = null;
		}
	}

	public void NextTurn() {
		
		//while(currentPath != null && remainingMovement > 0) {			
			//MoveToNextTile();
		if (!moving) {
			moving = true;
			remainingMovement = moveSpeed;

			if (firstMove) {
				remainingMovement--;
				firstMove = false;
			}
			
		}
		//}
		//remainingMovement = moveSpeed;
	}
}
