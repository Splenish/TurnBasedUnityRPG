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
    private Image ProgressBar;
    public GameObject Selector;
    //ienumerator
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10f;
    //dead/alive
    private bool alive = true;
    public bool isDead = false;
    Animator anim;
    //heropanel
    private HeroPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer;
    public GameObject FloatingTextPrefab;
    
    private bool isBasicAttack = false;
    private bool isSlash = false;
    private bool isHeal = false;
    private bool attack = false;
    
    public BattleStateMachine attackBool;
    





    void Start()
    {
        //find spacer
        HeroPanelSpacer = GameObject.Find("Battlecanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
        //create panel, fill in info
        CreateHeroPanel();
        
        
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f);
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.Processing;
    }

 

    void Update()
    {
        if (attackBool.victoryBool)
        {
            anim.SetBool("isVictory", true);
            
        }
        else
        {

            //Debug.Log(currentState);
            switch (currentState)
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
                    if (!alive)
                    {
                        return;
                    }
                    else
                    {
                        //change tag
                        this.gameObject.tag = "DeadHero";
                        //not attackable by enemy
                        BSM.HeroesInBattle.Remove(this.gameObject);
                        //not manageable
                        BSM.HeroesToManage.Remove(this.gameObject);
                        //deactivate the selector
                        Selector.SetActive(false);
                        //reset ui
                        BSM.ActionPanel.SetActive(false);
                        BSM.EnemySelectPanel.SetActive(false);
                        //remove item from performlist
                        if (BSM.HeroesInBattle.Count > 0)
                        {


                            for (int i = 0; i < BSM.PerformList.Count; i++)
                            {
                                if (i != 0)
                                {


                                    if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                                    {
                                        BSM.PerformList.Remove(BSM.PerformList[i]);
                                    }

                                    if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                                    {
                                        BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
                                    }
                                }
                            }
                        }
                        //change color  / play animation
                        anim.SetBool("isDead", true);
                        //reset heroinput
                        BSM.battleStates = BattleStateMachine.PerformAction.Checkalive;
                        alive = false;

                    }
                    break;
            }
        }

    }

    /*void asdasd()
    {
        anim.SetBool("isBasicAttack", false);
            
    }
    void MagicAttack()
    {
        anim.SetBool("isSlash", false);
    }
    */
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
            anim.SetBool("isBasicAttack", true);
           
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
            anim.SetBool("isSlash", false);
            anim.SetBool("isBasicAttack", false);
  
            yield return null;

        }
        // lopeta coroutine
        actionStarted = false;
        // poista performance BSM listasta
        BSM.PerformList.RemoveAt(0);
        if (BSM.battleStates != BattleStateMachine.PerformAction.Win && BSM.battleStates != BattleStateMachine.PerformAction.Lose)
        {


            // resettaa BSM -> Wait
            BSM.battleStates = BattleStateMachine.PerformAction.Wait;
            cur_cooldown = 0f;
            currentState = TurnState.Processing;
        }
        else
        {
            
            currentState = TurnState.Waiting;
        }
        attackBool.basicAttack = false;
        attackBool.magicAttack = false;
        // resettaa enemy state
       

    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    public void TakeDamage(float getDamageAmount)
    {
        anim.SetTrigger("Hit");
        StartCoroutine(TakeHit());
        hero.curHP -= getDamageAmount;
      
        if(hero.curHP <= 0)
        {
            hero.curHP = 0;
            currentState = TurnState.Dead;
        }
        UpdateHeroPanel();
    }

    public void MagicCost()
    {
        hero.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
        UpdateHeroPanel();
    }

    void DoDamage()
    {
        float calc_damage = hero.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        float calc_magicCost = hero.curMP - BSM.PerformList[0].choosenAttack.attackCost;
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
        MagicCost();
        
        if (FloatingTextPrefab)
        {
            Vector3 HeroTextPosition = new Vector3(EnemyToAttack.transform.position.x, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
            var go = Instantiate(FloatingTextPrefab, HeroTextPosition, Quaternion.identity);

            go.GetComponent<TextMesh>().text = "" + calc_damage;
        }
        
        
    }
    

    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject;
        stats = HeroPanel.GetComponent<HeroPanelStats>();
        stats.HeroName.text = hero.theName;
        stats.HeroHP.text = "" + hero.curHP + "/" + hero.baseHP;
        stats.HeroMP.text = "" + hero.curMP + "/" + hero.baseMP;
        ProgressBar = stats.ProgressBar;
        HeroPanel.transform.SetParent(HeroPanelSpacer, false);
        
    }

    void UpdateHeroPanel()
    {
        stats.HeroHP.text = "" + hero.curHP + "/" + hero.baseHP;
        stats.HeroMP.text = "" + hero.curMP + "/" + hero.baseMP;
    }

    IEnumerator TakeHit()
    {
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Idle");
    }

     /*private IEnumerator BasicAttack()
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
            anim.SetBool("isBasicAttack", true);
            yield return null;
        }
        // oota hetki
        yield return new WaitForSeconds(1.2f);
        // tee dmg
        float calc_damage = hero.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
        if (FloatingTextPrefab)
        {
            Vector3 HeroTextPosition = new Vector3(EnemyToAttack.transform.position.x, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
            var go = Instantiate(FloatingTextPrefab, HeroTextPosition, Quaternion.identity);

            go.GetComponent<TextMesh>().text = "" + calc_damage;
        }


        // animaatio takas startpositioniin
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition))
        {
            
           // anim.SetBool("isBasicAttack", false);
            attackBool.basicAttack = false;
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
        
    }
    */
   /* private IEnumerator MagicAttack()
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
            anim.SetBool("isSlash", true);
            yield return null;
        }
        // oota hetki
        yield return new WaitForSeconds(1.2f);
        // tee dmg
        float calc_damage = hero.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
        if (FloatingTextPrefab)
        {
            Vector3 HeroTextPosition = new Vector3(EnemyToAttack.transform.position.x, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
            var go = Instantiate(FloatingTextPrefab, HeroTextPosition, Quaternion.identity);

            go.GetComponent<TextMesh>().text = "" + calc_damage;
        }


        // animaatio takas startpositioniin
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition))
        {
            anim.SetBool("isSlash", false);
            attackBool.magicAttack = false;
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
        
    }*/

}
