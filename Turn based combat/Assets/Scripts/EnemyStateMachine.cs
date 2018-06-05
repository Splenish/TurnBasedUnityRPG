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
    //timeforaction juttuja
    private bool actionStarted = false;
    private GameObject HeroToAttack;
    private float animSpeed = 5f;


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
                StartCoroutine(TimeForAction());
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

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // animaatio enemylle 
        Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x-1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
        while(MoveTowardsEnemy(heroPosition))
        {
            yield return null;
        }
        // oota hetki
        // tee dmg

        // animaatio takas startpositioniin

        // poista performance BSM listasta

        // resettaa BSM -> Wait

        actionStarted = false;
        // resettaa enemy state
        cur_cooldown = 0f;
        currentState = TurnState.Processing;

    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
}
