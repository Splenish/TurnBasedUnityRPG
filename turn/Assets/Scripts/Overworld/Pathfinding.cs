using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pathfinding : MonoBehaviour {


	public Transform seeker, target;

	public GameObject currentUnit;

	GameObject gm;

	GameGrid grid;

	public List<Node> oldPath;

	void Awake() {
		grid = GetComponent<GameGrid> ();
		currentUnit = GameObject.Find ("Player");
		gm = GameObject.Find ("GameManager");
	}

	void Update() {
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;

		if (gs == GameManager.GameState.myTurn) {
			if (currentUnit.name != "Player") {
				grid.path = null;
				grid.path = oldPath;
				currentUnit = GameObject.Find ("Player");
			}
		}

		if (gs == GameManager.GameState.enemyTurn) {
			if (currentUnit.name != "SkeletonEnemy") {
				oldPath = grid.path;
				currentUnit = GameObject.Find ("SkeletonEnemy");
				Debug.Log (currentUnit);
				FindPath (currentUnit.transform.position, currentUnit.GetComponent<EnemyUnit>().target);
				currentUnit.GetComponent<Unit> ().currentPath = grid.path;
			}
		}
			 
		if (Input.GetMouseButtonDown(0) && !currentUnit.GetComponent<Unit>().moving  && !EventSystem.current.IsPointerOverGameObject() && gs == GameManager.GameState.myTurn) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Grid")) && hit.transform.name=="GridLines"){
				FindPath (seeker.position, hit.point);
				currentUnit.GetComponent<Unit> ().currentPath = grid.path;
				//currentUnit.GetComponent<Unit> ().firstMove = true;
			}
		}

	}

	void FindPath(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		List<Node> openSet = new List<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();

		openSet.Add (startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet [0];
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet [i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
					currentNode = openSet [i];
				}
			}

			openSet.Remove (currentNode);
			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				RetracePath (startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains (neighbour)) {
					continue;
				}
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance (neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains (neighbour)) {
						openSet.Add (neighbour);
					}
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}

		path.Reverse ();

		grid.path = path;

		//Debug.Log (path.Count);

	}

	int GetDistance(Node nodeA, Node nodeB) {
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY) {
			return 14 * distY + 10 * (distX - distY);
		}

		return 14 * distX + 10 * (distY - distX);

	}



}