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

    public Transform Spacer;
    

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    
	
	void Start () {
        battleStates = PerformAction.Wait;
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroInput = HeroGui.Activate;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);



        EnemyButtons();
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
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.Action;
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
                    AttackPanel.SetActive(true);
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

    public void Input1()//attack button
    {
        HeroChoise.Attacker = HeroesToManage[0].name;
        HeroChoise.AttackersGameObject = HeroesToManage[0];
        HeroChoise.Type = "Hero";

        //AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true); 
        
    }

    public void Input2(GameObject choosenEnemy)//enemy selection
    {
        HeroChoise.AttackersTarget = choosenEnemy;
        HeroInput = HeroGui.Done;
    }

    void HeroInputDone()
    {
        PerformList.Add(HeroChoise);
        EnemySelectPanel.SetActive(false);
        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGui.Activate;
    }

 
}
