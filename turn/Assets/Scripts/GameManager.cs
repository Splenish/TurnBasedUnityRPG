using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public GameObject currentUnit;


	GameObject battleEnemy;

	bool enemyTurnStart = true;

	bool playerTurnStart = true;

	public int i = 0;

	//public GameObject[] enemyUnits;

	public List<GameObject> enemyUnits;

	public List<GameObject> battleEnemies;

	GameObject player;

	public GameObject enemy;

	public List<Vector3> spawnPoints;

	public Node[,] grid;

	public GameObject gridObj;

	public Quaternion rotation;


	public static GameManager Instance;
	public enum GameState
	{
		myTurn,
		enemyTurn
	}
	private GameState currentGameState;
	public GameState CurrentGameState
	{
		get { return currentGameState; }
		set { currentGameState = value; }
	}
	// Use this for initialization
	void Awake()
	{



		if (currentGameState == GameState.myTurn)
			currentUnit = player;


		Debug.Log (currentUnit);
		Debug.Log (currentGameState);

		if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;

			currentUnit = GameObject.Find ("Player");
			currentGameState = GameState.myTurn;
			player = GameObject.Find ("Player");


			rotation = Quaternion.identity;

			rotation.eulerAngles = new Vector3 (0, -90, 0);

			//grid = GetComponent<GameGrid> ().grid;
			grid = gridObj.GetComponent<GameGrid>().grid;

			Debug.Log (grid);


			SpawnEnemies ();


			//enemyUnits = GameObject.FindGameObjectsWithTag ("Enemy");

			GameObject[] tempUnits = GameObject.FindGameObjectsWithTag ("Enemy");

			foreach (GameObject unit in tempUnits) {
				enemyUnits.Add (unit);
			}

			Debug.Log (enemyUnits.Count);

		} else if(Instance != this) {
			Destroy(gameObject);
		}


	}

	// Update is called once per frame
	void Update()
	{

		switch (currentGameState)
		{
		case GameState.myTurn:
			currentUnit = player;

			if (playerTurnStart) {
				currentUnit.GetComponent<Player> ().StartTurn ();
				playerTurnStart = false;
				enemyTurnStart = true;
			}
			break;

		case GameState.enemyTurn:
			if(i < enemyUnits.Count)
				currentUnit = enemyUnits [i];
			
			if (currentUnit == null) {
				Debug.Log ("skidaadle");
				//currentGameState = GameState.myTurn;
				//i++;
			}
			Debug.Log ("current unit gm: " + currentUnit);
			if (enemyTurnStart) {
				Debug.Log ("if enemyturnstart");
				currentUnit.GetComponent<EnemyUnit> ().StartEnemyTurn ();
				enemyTurnStart = false;
				playerTurnStart = true;
			}
			break;
		}
	}

	public void StartCombat(GameObject triggeredEnemy) {

		//battleEnemy = triggeredEnemy;

		battleEnemies.Add (triggeredEnemy);

		Debug.Log ("vomat");
		player.SetActive (false);

		for (int j = 0; j < enemyUnits.Count; j++) {
			enemyUnits [j].SetActive (false);
		}

		SceneManager.LoadScene ("TestScene");
	}

	void SpawnEnemies() {
		for (int k = 0; k < grid.GetLength (0); k++) {
			for (int j = 0; j < grid.GetLength (1); j++) {
				if(grid[k,j].walkable && !(k < 8))
					spawnPoints.Add (grid [k, j].worldPosition); 
			}
		}


		for (int j = 0; j < 5; j++) {
			int spawnPointIndex = Random.Range (0, spawnPoints.Count);
			Instantiate (enemy, spawnPoints [spawnPointIndex], rotation);
			spawnPoints.RemoveAt (spawnPointIndex);
		}

		Debug.Log ("skeltas spawned");

	}
		
	public void ActivateUnits() {
		//Destroy (enemyUnits [i]);


		for (int j = 0; j < battleEnemies.Count; j++) {
			enemyUnits.Remove(battleEnemies[j]);
			Destroy (battleEnemies[j]);
		}
		//enemyUnits.Remove(battleEnemy);

		//Destroy (battleEnemy);

		//enemyUnits.RemoveAt (i);

		Debug.Log(enemyUnits.Count);

		player.SetActive (true);

		for (int j = 0; j < enemyUnits.Count; j++) {
			enemyUnits [j].SetActive (true);
		}
	}


}