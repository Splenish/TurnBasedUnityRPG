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
    public bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 10f;
    Animator anim;
    public GameObject Selector2;
    public GameObject FloatingTextPrefab;
    private bool alive = true;


    
    void Start()
    {
        anim = GetComponent<Animator>();
        Selector2.SetActive(false);
        currentState = TurnState.Processing;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startPosition = transform.position;
    }

    void Update()
    {
        //Debug.Log(currentState);
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
                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag
                    this.gameObject.tag = "DeadEnemy";
                    //not attackable by enemy
                    BSM.EnemiesInBattle.Remove(this.gameObject);
                    //not manageable
                    BSM.EnemiesInBattle.Remove(this.gameObject);
                    //deactivate the selector
                    Selector2.SetActive(false);

                    //remove item from performlist
                    for (int i = 0; i < BSM.PerformList.Count; i++)
                    {
                        if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BSM.PerformList.Remove(BSM.PerformList[i]);
                        }
                    }
                    //change color  / play animation
                    anim.SetTrigger("isDead");
                    //reset heroinput


                    alive = false;
                    break;
                }
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
        myAttack.Attacker = enemy.theName;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];

        int num = Random.Range(0, enemy.Attacks.Count);
        myAttack.choosenAttack = enemy.Attacks[num];
        //Debug.Log(this.gameObject.name + "has choosen" + myAttack.choosenAttack.attackName + "and does" + myAttack.choosenAttack.attackDamage + "damage");

        BSM.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        //if (completeTurn == true)
        //{
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // animaatio enemylle 
        Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x - 0.8f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
        while (MoveTowardsEnemy(heroPosition))
        {
            anim.SetBool("isAttacking", true);
            yield return null;
        }
        // oota hetki
        yield return new WaitForSeconds(1.2f);
        // tee dmg
        DoDamage();
        // animaatio takas startpositioniin
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition))
        {
            anim.SetBool("isAttacking", false);

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
        // completeTurn = false;
        // }

    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void DoDamage()
    {
        float calc_damage = enemy.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);
        if (FloatingTextPrefab)
        {
            Vector3 HeroTextPosition = new Vector3(HeroToAttack.transform.position.x, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
            var go = Instantiate(FloatingTextPrefab, HeroTextPosition, Quaternion.identity);

            go.GetComponent<TextMesh>().text = "" + calc_damage;
        }


    }
   
    public void TakeDamage(float getDamageAmount)
    {
        enemy.curHP -= getDamageAmount;
        if(enemy.curHP <= 0)
        {
            enemy.curHP = 0;
            currentState = TurnState.Dead;
        }
    }
}
