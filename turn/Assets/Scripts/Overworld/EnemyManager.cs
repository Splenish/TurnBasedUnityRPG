using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {


	public GameObject enemy;

	public List<Vector3> spawnPoints;

	public Node[,] grid;

	public GameObject gridObj;

	public Quaternion rotation;

	// Use this for initialization
	void Start () {

		rotation = Quaternion.identity;

		rotation.eulerAngles = new Vector3 (0, -90, 0);

		//grid = GetComponent<GameGrid> ().grid;
		grid = gridObj.GetComponent<GameGrid>().grid;

		Debug.Log (grid);


		for (int i = 0; i < grid.GetLength (0); i++) {
			for (int j = 0; j < grid.GetLength (1); j++) {
				if (grid [i, j].walkable) {
					spawnPoints.Add (grid [i, j].worldPosition); 
				}
			}
		}



		for (int i = 0; i < 20; i++) {
			Debug.Log ("paska");
			int spawnPointIndex = Random.Range (0, spawnPoints.Count);
			Instantiate (enemy, spawnPoints [spawnPointIndex], rotation);
		}


	}
	
	// Update is called once per frame
	void Update () {
		if (grid == null) {
			grid = gridObj.GetComponent<GameGrid> ().grid;
			Debug.Log (grid);
			for (int i = 0; i < grid.GetLength (0); i++) {
				for (int j = 0; j < grid.GetLength (1); j++) {
					spawnPoints.Add (grid [i, j].worldPosition); 
				}
			}

			int spawnPointIndex = Random.Range (0, spawnPoints.Count);


			Instantiate (enemy, spawnPoints [spawnPointIndex], rotation);
		}
	}
}
