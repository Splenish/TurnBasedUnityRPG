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
		enemyUnits = GameObject.FindGameObjectsWithTag ("Enemy");
		player = GameObject.Find ("Player");
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


