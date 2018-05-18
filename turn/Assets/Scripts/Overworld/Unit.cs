using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public int moveSpeed;
	int remainingMovement;

	//bool moving = false;

	public List<Node> currentPath = null;

	Vector3 startPos, endPos;

	GameObject currentUnit;

	// Use this for initialization
	void Start () {
		remainingMovement = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentPath != null) {


			if (Vector3.Distance (transform.position, currentPath [0].worldPosition) < 0.1f) {
				MoveToNextTile ();
				//moving = false;
			}

			transform.position = Vector3.Lerp (transform.position, currentPath [0].worldPosition, 0.1f);

		}
	}


	public void MoveToNextTile() {
		if (currentPath == null) 
			return;
		
		if (remainingMovement <= 0)
			return;
		//endPos = currentPath [0]. worldPosition;
	
		remainingMovement -= 1;



		//remove the old current tile
		currentPath.RemoveAt(0);
		if (currentPath.Count == 0) {
			currentPath = null;
		}
	}

	public void NextTurn() {
		
		while(currentPath != null && remainingMovement > 0) {
			//moving = true;
			MoveToNextTile();
		}

		remainingMovement = moveSpeed;
	}
}
