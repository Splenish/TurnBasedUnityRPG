using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public int moveSpeed;

	public List<Node> currentPath = null;

	GameObject currentUnit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void MoveToNextTile() {
		int remainingMovement = moveSpeed;
		while (remainingMovement > 0) {
			if (currentPath == null) {
				return;
			}
			remainingMovement -= 1;

			//remove the old current tile
			currentPath.RemoveAt(0);
			if (currentPath.Count == 1) {
				currentPath = null;
			}
		}
	}
}
