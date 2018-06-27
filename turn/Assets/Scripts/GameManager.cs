using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public GameObject currentUnit;


	bool enemyTurnStart = true;

	bool playerTurnStart = true;

	public int i = 0;

	public GameObject[] enemyUnits;

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


			enemyUnits = GameObject.FindGameObjectsWithTag ("Enemy");
			Debug.Log (enemyUnits.Length);

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
			currentUnit = enemyUnits [i];
			if (currentUnit == null) {
				i++;
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

	public void StartCombat() {
		Debug.Log ("vomat");
		player.SetActive (false);

		for (int j = 0; j < enemyUnits.Length; j++) {
			enemyUnits [j].SetActive (false);
		}

		SceneManager.LoadScene ("TestScene");
	}

	void SpawnEnemies() {
		for (int i = 0; i < grid.GetLength (0); i++) {
			for (int j = 0; j < grid.GetLength (1); j++) {
				if(grid[i,j].walkable && !(i < 8))
					spawnPoints.Add (grid [i, j].worldPosition); 
			}
		}


		for (int i = 0; i < 20; i++) {
			int spawnPointIndex = Random.Range (0, spawnPoints.Count);
			Instantiate (enemy, spawnPoints [spawnPointIndex], rotation);
			spawnPoints.RemoveAt (spawnPointIndex);
		}

		Debug.Log ("skeltas spawned");

	}
		
	public void ActivateUnits() {
		Destroy (enemyUnits [i]);



		player.SetActive (true);

		for (int j = 0; j < enemyUnits.Length; j++) {
			enemyUnits [j].SetActive (true);
		}
	}


}