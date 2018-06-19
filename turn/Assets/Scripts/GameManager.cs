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
		currentUnit = GameObject.Find ("Player");
        currentGameState = GameState.myTurn;
		player = GameObject.Find ("Player");


		rotation = Quaternion.identity;

		rotation.eulerAngles = new Vector3 (0, -90, 0);

		//grid = GetComponent<GameGrid> ().grid;
		grid = gridObj.GetComponent<GameGrid>().grid;

		for (int i = 0; i < grid.GetLength (0); i++) {
			for (int j = 0; j < grid.GetLength (1); j++) {
				spawnPoints.Add (grid [i, j].worldPosition); 
			}
		}

		int spawnPointIndex = Random.Range (0, spawnPoints.Count);


		Instantiate (enemy, spawnPoints [spawnPointIndex], rotation);

		enemyUnits = GameObject.FindGameObjectsWithTag ("Enemy");
		Debug.Log (enemyUnits.Length);
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
			Debug.Log ("Enemy Turn");
			currentUnit = enemyUnits [i];

			if (enemyTurnStart) {
				currentUnit.GetComponent<EnemyUnit> ().StartEnemyTurn ();
				enemyTurnStart = false;
				playerTurnStart = true;
			}
			break;
        }
    }

	public void StartCombat() {
		Debug.Log ("vomat");
		//SceneManager.LoadScene ("fighttoooscennoo");
	}
}


