using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour {

	GameObject gm;

	GameObject pathLine;
	LineRenderer lr;

	public Material pathLineMat;

	public Transform player;

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	public GameObject playerObject;

	//int playerSpeed;

	void Awake() {
		CreatePathLine ();

		//playerSpeed = playerObject.GetComponent<Unit> ().remainingMovement;


		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid ();

		gm = GameObject.Find ("GameManager");
	}

	void LateUpdate() {
		//GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;

		//if(gs == GameManager.GameState.enemyTurn)
		//	path = null;

		if (lr == null) {
			CreatePathLine ();
		}


		CreateGrid ();
		DrawPath ();
	}

	void CreateGrid() {
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;


		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
				grid [x, y] = new Node (walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node> ();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add (grid [checkX, checkY]);
				}
			}
		}
		return neighbours;
	}


	/*
	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		//float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		//float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f;
		float percentY = (worldPosition.z - transform.position.z) / gridWorldSize.y + 0.5f;﻿
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);
		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid [x, y];
	}
	*/
	public Node NodeFromWorldPoint(Vector3 _worldPosition)
	{
		float posX = ((_worldPosition.x - transform.position.x) + gridWorldSize.x * 0.5f) / nodeDiameter;
		float posY = ((_worldPosition.z - transform.position.z) + gridWorldSize.y * 0.5f) / nodeDiameter;

		posX = Mathf.Clamp(posX, 0, gridWorldSize.x - 1);
		posY = Mathf.Clamp(posY, 0, gridWorldSize.y - 1);

		int x = Mathf.FloorToInt(posX);
		int y = Mathf.FloorToInt(posY);

		return grid[x, y];
	}﻿

	public List<Node> path;


	void DrawPath() {
		Node n;
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;
		if (gs == GameManager.GameState.myTurn && playerObject.GetComponent<Unit>().currentPath != null) {
			if (lr != null) {
				lr.gameObject.SetActive(true);
				if (path != null) {
					//lr.positionCount = playerSpeed + 1;
					lr.positionCount = path.Count + 1;
					lr.SetPosition (0, player.position);
				}
			}
			if (grid != null) {
				if (path != null) {

					for (int i = 0; i < path.Count; i++) {
						n = path [i];
						lr.SetPosition (i + 1, n.worldPosition);
					}
				}
			}
		} else {
			lr.gameObject.SetActive(false);
		}
	}


	void CreatePathLine() {
		pathLine = new GameObject ();
		pathLine.transform.position = player.position;
		pathLine.AddComponent<LineRenderer> ();
		lr = pathLine.GetComponent<LineRenderer> ();

		lr.material = pathLineMat;

		lr.startWidth = 0.2f;
		lr.endWidth = 0.2f;
		lr.useWorldSpace = true;
	}

}