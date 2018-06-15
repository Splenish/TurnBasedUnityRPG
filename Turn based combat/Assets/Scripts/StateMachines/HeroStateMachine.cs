using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseHero hero;
    

    public enum TurnState
    {
        Processing,
        Addtolist,
        Waiting,
        Selecting,
        Action,
        Dead
    }

    public TurnState currentState;

    //ProgressBarille muuttujia
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;
    public GameObject Selector;
    //ienumerator
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10f;
    



    void Start()
    {
        startPosition = transform.position;
        cur_cooldown = Random.Range(0, 2f);
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.Processing;
    }

    void Update()
    {
        Debug.Log(currentState);
        switch(currentState)
        {
            case (TurnState.Processing):
                UpgradeProgressBar();
                break;
            case (TurnState.Addtolist):
                BSM.HeroesToManage.Add(this.gameObject);
                currentState = TurnState.Waiting;
                break;
            case (TurnState.Waiting):

                break;
            case (TurnState.Selecting):

                break;
            case (TurnState.Action):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.Dead):

                break;
        }

    }

    void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.Addtolist;
        }

    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // animaatio enemylle 
        Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 0.8f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(enemyPosition))
        {
            yield return null;
        }
        // oota hetki
        yield return new WaitForSeconds(1f);
        // tee dmg

        // animaatio takas startpositioniin
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition))
        {
            yield return null;
        }

        // poista performance BSM listasta
        BSM.PerformList.RemoveAt(0);

        // resettaa BSM -> Wait
        BSM.battleStates = BattleStateMachine.PerformAction.Wait;

        // lopeta coroutine
        actionStarted = false;

        // resettaa enemy state
        cur_cooldown = 0f;
        currentState = TurnState.Processing;
        //completeTurn = true;

    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

}
