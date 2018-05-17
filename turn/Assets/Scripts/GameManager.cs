using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //public GameObject PlayerPbject;


    public enum GameState
    {
        myTurn,
        enemyTurn
    }
    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
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
                //PlayerPbject.GetComponent<move>().moveShit();
                break;

            case GameState.enemyTurn:
                //enemy.move;
                break;
        }
    }
}
