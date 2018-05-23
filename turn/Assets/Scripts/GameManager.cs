using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public GameObject currentUnit;


	bool paskanaama = true;

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
    void Start()
    {
        currentGameState = GameState.myTurn;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentGameState)
        {
		case GameState.myTurn:
			currentUnit = GameObject.Find ("Player");
			paskanaama = true;
			break;

		case GameState.enemyTurn:
			currentUnit = GameObject.Find ("SkeletonEnemy");
			if (paskanaama) {
				currentUnit.GetComponent<EnemyUnit> ().StartEnemyTurn ();
				Debug.Log ("start paskaa");
				paskanaama = false;
			}
			break;
        }
    }
}
