using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseEnemy enemy;

    public enum TurnState
    {
        Processing,
        ChooseAction,
        Waiting,
        Action,
        Dead
    }

    public TurnState currentState;

    //ProgressBarille muuttujia
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;

    // animaatiota  varten
    private Vector3 startPosition;

    void Start()
    {
        currentState = TurnState.Processing;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startPosition = transform.position;
    }

    void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case (TurnState.Processing):
                UpdateProgress();
                break;
            case (TurnState.ChooseAction):
                ChooseAction();
                currentState = TurnState.Waiting;
                break;
            case (TurnState.Waiting):

                break;

            case (TurnState.Action):

                break;
            case (TurnState.Dead):

                break;
        }

    }

    void UpdateProgress()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ChooseAction;
        }

    }
    
    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.name;
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
        BSM.CollectActions(myAttack);
    }
}
