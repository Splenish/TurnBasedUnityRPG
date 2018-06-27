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

	int i;

	void Awake() {
		grid = GetComponent<GameGrid> ();
		//currentUnit = GameObject.Find ("Player");
		gm = GameObject.Find ("GameManager");
		currentUnit = gm.GetComponent<GameManager>().currentUnit;
	}

	void Update() {
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;
		//Debug.Log ("gs pathfinding: " + gs);
		if (gs == GameManager.GameState.myTurn) {
			if (currentUnit.name != "Player") {
				//grid.path = null;
				//currentUnit.GetComponent<Unit> ().currentPath = null;
				//grid.path = oldPath;
				//Debug.Log ("path = oldpath");
				currentUnit = GameObject.Find ("Player");
			}
			currentUnit = gm.GetComponent<GameManager>().currentUnit;
		}

		if (gs == GameManager.GameState.enemyTurn) {
			Debug.Log ("enemy turn");
			if (currentUnit.tag != "Enemy") {
				//oldPath = grid.path;
				i = gm.GetComponent<GameManager> ().i;
				currentUnit = gm.GetComponent<GameManager> ().currentUnit;
				Debug.Log ("current unit pathfindingisa " + currentUnit);
				//currentUnit = GameObject.Find ("SkeletonEnemy");
				FindPath (currentUnit.transform.position, currentUnit.GetComponent<EnemyUnit> ().target);
				currentUnit.GetComponent<Unit> ().currentPath = grid.path;
				//Debug.Log ("currentUnit gameobject kun asetetaan pathia" + currentUnit.gameObject);
				//Debug.Log ("currentUnit current path" + currentUnit.GetComponent<Unit> ().currentPath);
				//Debug.Log ("currentPath count" + currentUnit.GetComponent<Unit> ().currentPath.Count);
			}

			if (i < gm.GetComponent<GameManager> ().i) {

				currentUnit = gm.GetComponent<GameManager> ().currentUnit;
				Debug.Log ("current unit pathfindingisa " + currentUnit);
				//currentUnit = GameObject.Find ("SkeletonEnemy");
				FindPath (currentUnit.transform.position, currentUnit.GetComponent<EnemyUnit> ().target);
				currentUnit.GetComponent<Unit> ().currentPath = grid.path;
				//Debug.Log ("currentUnit gameobject kun asetetaan pathia" + currentUnit.gameObject);
				//Debug.Log ("currentUnit current path" + currentUnit.GetComponent<Unit> ().currentPath);
				//Debug.Log ("currentPath count" + currentUnit.GetComponent<Unit> ().currentPath.Count);
			}

			i = gm.GetComponent<GameManager> ().i;
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