using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public GameObject currentUnit;


	bool paskanaama = true;

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
			paskanaama = true;
			break;

		case GameState.enemyTurn:
			/*for (int i = 0; i < enemyUnits.Length; i++) {
				currentUnit = enemyUnits [i];
				if (paskanaama) {
					Debug.Log (currentUnit + "game managerisa");
					currentUnit.GetComponent<EnemyUnit> ().StartEnemyTurn ();
					//paskanaama = false;
				}
			}
			*/

			currentUnit = enemyUnits [i];
			//Debug.Log("enemyUnits[i] = " + enemyUnits [i] + " i = " + i);

			if (paskanaama) {
				//Debug.Log (currentUnit + "game managerisa");
				currentUnit.GetComponent<EnemyUnit> ().StartEnemyTurn ();
				//paskanaama = false;
			}

			//currentGameState = GameState.myTurn;
			//player.GetComponent<Unit> ().StartTurn ();
			//Debug.Log ("playerturn start");
			break;
        }
    }

	public void StartCombat() {
		Debug.Log ("vomat");
		//SceneManager.LoadScene ("fighttoooscennoo");
	}
}


