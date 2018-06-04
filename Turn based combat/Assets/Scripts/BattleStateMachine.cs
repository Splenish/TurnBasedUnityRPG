using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {
        Wait,
        TakeAction,
        PerformAction
    }

    public PerformAction battleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> HeroesInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();
	
	void Start () {
        battleStates = PerformAction.Wait;
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
	}
	
	
	void Update () {

        switch(battleStates)
        {
            case (PerformAction.Wait):

                break;
            case (PerformAction.TakeAction):

                break;
            case (PerformAction.PerformAction):

                break;
        }
	}

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }
}
