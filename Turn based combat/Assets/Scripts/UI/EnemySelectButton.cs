using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectButton : MonoBehaviour {

    public GameObject EnemyPrefab;
    private bool showSelector;
    
    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input2(EnemyPrefab);
    }

    public void HideSelector()
    {
        EnemyPrefab.transform.Find("Selector2").gameObject.SetActive(false);

    }

    public void ShowSelector()
    {
        EnemyPrefab.transform.Find("Selector2").gameObject.SetActive(true);
    }
}
