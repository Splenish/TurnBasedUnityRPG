using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class BattleController : MonoBehaviour {

    public Text EventText;
    public Text PlayerUnit1Text;
    public Text EnemyUnit1Text;
    public GameObject PlayerObject;
    public GameObject EnemyObject;

    Animation anim;

    int PlayerUnit1Health = 1000;
    int EnemyUnit1Health = 1000;

    bool PlayerTurn = true;

    void Start()
    {
        StartPlayerTurn();
       PlayerObject = GameObject.Find("PlayerUnit(Clone)");
       EnemyObject =  GameObject.Find("SkeletonEnemy(Clone)");
       
    }

    void Update()
    {
        if(PlayerObject == null)
        {
            PlayerObject = GameObject.Find("PlayerUnit(Clone)");
        }
        if(EnemyObject == null)
        {
            EnemyObject = GameObject.Find("SkeletonEnemy(Clone)");
        }
        if (PlayerTurn && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerFight();
            SwitchPlayers();
        }
    }

    void StartPlayerTurn()
    {
        EventText.text = "Your turn.. Choose an action";
    }

    void PlayerFight()
    {
        int damage = Random.Range(250, 350);
        EnemyUnit1Health -= damage;

        if (EnemyUnit1Health <= 0)
        {
            EnemyUnit1Health = 0;
            Destroy(EnemyObject);

        }
    }


    void SwitchPlayers()
    {
        PlayerUnit1Text.text = "Health: " + PlayerUnit1Health;
        EnemyUnit1Text.text = "Health: " + EnemyUnit1Health;
        PlayerTurn = !PlayerTurn;

        if (PlayerTurn)
        {
            StartPlayerTurn();
        }
        else
        {
            StartAiTurn();
        }
    }

    void StartAiTurn()
    {
        EventText.text = "Opponent turn.. Please wait..";
            StartCoroutine(EnemyAiTurn());
    }
    IEnumerator EnemyAiTurn()
    {
        yield return new WaitForSeconds(Random.Range(2, 5));
        EnemyAiFight();
        SwitchPlayers();
    }

    void EnemyAiFight()
    {
        int damage = Random.Range(250, 350);
        PlayerUnit1Health -= damage;

        if (PlayerUnit1Health <= 0)
        {
            PlayerUnit1Health = 0;
            GameObject.Destroy(PlayerObject);
        }
    }

}
