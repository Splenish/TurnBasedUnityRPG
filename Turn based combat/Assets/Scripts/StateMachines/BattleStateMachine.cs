using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public enum HeroGui
    {
        Activate,
        Waiting,
        Input1,
        Input2,
        Done
    }

    public HeroGui HeroInput;

    public List<GameObject> HeroesToManage = new List<GameObject>();
    private HandleTurn HeroChoise;

    public GameObject enemyButton;
    public GameObject heroButton;

    public Transform Spacer;
    public Transform Spacer2;
    

    public GameObject ActionPanel;
    public GameObject EnemySelectPanel;
    public GameObject MagicPanel;
    public GameObject HeroSelectPanel;

    //magic attack
    public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButton;
    public GameObject magicButton;
    private List<GameObject> actBtns = new List<GameObject>();
    public bool basicAttack = false;
    public bool magicAttack = false;
    Animator anim;
    


    void Start () {

        anim = GetComponent<Animator>();
        battleStates = PerformAction.Wait;
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroInput = HeroGui.Activate;
        HeroSelectPanel.SetActive(false);
        ActionPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        MagicPanel.SetActive(false);



        EnemyButtons();
        HeroButtons();
	}
	
	
	void Update () {

        switch(battleStates)
        {
            case (PerformAction.Wait):
                if(PerformList.Count > 0)
                {
                    battleStates = PerformAction.TakeAction;
                }
                break;

            case (PerformAction.TakeAction):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if(PerformList[0].Type == "Enemy")
                {
                    StartCoroutine(AiWait());
                   /* EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.Action;*/
                }
                if (PerformList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.Action;
                }
                battleStates = PerformAction.PerformAction;
                break;

            case (PerformAction.PerformAction):

                break;
        }

        switch(HeroInput)
        {
            case (HeroGui.Activate):
                if(HeroesToManage.Count > 0)
                {
                    HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoise = new HandleTurn();
                    ActionPanel.SetActive(true);
                    //populate buttons
                    CreateAttackButtons();
                    HeroInput = HeroGui.Waiting;
                }
                break;

            case (HeroGui.Waiting):

                break;

            case (HeroGui.Done):
                HeroInputDone();
                break;

        }
	}

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }

    IEnumerator AiWait()
    {
        yield return new WaitForSeconds(1);

        GameObject performer = GameObject.Find(PerformList[0].Attacker);
        EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
        for (int i = 0; i<HeroesInBattle.Count; i++)
        {
            if(PerformList[0].AttackersTarget == HeroesInBattle[i])
            {
                ESM.HeroToAttack = PerformList[0].AttackersTarget;
                ESM.currentState = EnemyStateMachine.TurnState.Action;
                break;
            }
            else
            {
                PerformList[0].AttackersTarget = HeroesInBattle[Random.Range(0, HeroesInBattle.Count)];
                ESM.HeroToAttack = PerformList[0].AttackersTarget;
                ESM.currentState = EnemyStateMachine.TurnState.Action;
            }
        }
        
    }
    void EnemyButtons()
    {
        foreach(GameObject enemy in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.theName;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer,false);
        }
    }

    void HeroButtons()
    {
        foreach(GameObject hero in HeroesInBattle)
        {
            GameObject newButton = Instantiate(heroButton) as GameObject;
            HeroSelectButton button = newButton.GetComponent<HeroSelectButton>();

            HeroStateMachine cur_hero = hero.GetComponent<HeroStateMachine>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_hero.hero.theName;

            button.HeroPrefab = hero;

            newButton.transform.SetParent(Spacer2, false);
            
        }
    }
    public void Input1()//attack button
    {
        HeroChoise.Attacker = HeroesToManage[0].name;
        HeroChoise.AttackersGameObject = HeroesToManage[0];
        HeroChoise.Type = "Hero";
        HeroChoise.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.Attacks[0];

        ActionPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
        HeroSelectPanel.SetActive(false);
        basicAttack = true;
        
            
        
    }

    public void Input2(GameObject choosenEnemy)//enemy selection
    {
        HeroChoise.AttackersTarget = choosenEnemy;
        HeroInput = HeroGui.Done;
    }
    public void Input5(GameObject choosenHero)//hero selection
    {
        HeroChoise.AttackersTarget = choosenHero;      
        
        HeroInput = HeroGui.Done;
    }


    void HeroInputDone()
    {
        PerformList.Add(HeroChoise);
        EnemySelectPanel.SetActive(false);
        HeroSelectPanel.SetActive(false);
        foreach(GameObject actBtn in actBtns)
        {
            Destroy(actBtn);
        }
        actBtns.Clear();
        ActionPanel.SetActive(false);
        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGui.Activate;
        
    }

    public void CreateAttackButtons()
    {
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        actBtns.Add(AttackButton);

        GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;
        Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        MagicAttackButtonText.text = "Magic";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        MagicAttackButton.transform.SetParent(actionSpacer, false);
        actBtns.Add(MagicAttackButton);

        if(HeroesToManage[0].GetComponent<HeroStateMachine>().hero.Magics.Count>0)
        {
            foreach(BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.Magics)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName;
                MagicButtons MB = MagicButton.GetComponent<MagicButtons>();
                MB.MagicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicSpacer, false);
                actBtns.Add(MagicButton);

            }
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }
    
    public void Input4(BaseAttack choosenMagic)//choosen magic attack
    {
        HeroChoise.Attacker = HeroesToManage[0].name;
        HeroChoise.AttackersGameObject = HeroesToManage[0];
        HeroChoise.Type = "Hero";

        HeroChoise.choosenAttack = choosenMagic;
        MagicPanel.SetActive(false);
        HeroSelectPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
        magicAttack = true;
        
        
        
    }

    public void Input3()//switching to magic attacks
    {
        ActionPanel.SetActive(false);
        MagicPanel.SetActive(true);
    }
}
