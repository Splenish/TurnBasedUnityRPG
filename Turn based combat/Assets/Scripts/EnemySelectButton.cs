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

    public void HoverSelector()
    {
        if (showSelector)
        {
            EnemyPrefab.transform.Find("Selector").gameObject.SetActive(showSelector);

        }
    }
}
