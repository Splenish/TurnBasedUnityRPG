using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectButton : MonoBehaviour
{

    public GameObject HeroPrefab;
    private bool showSelector;

    public void SelectHero()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input5(HeroPrefab);
    }

    public void HideSelector()
    {
        HeroPrefab.transform.Find("Selector3").gameObject.SetActive(false);

    }

    public void ShowSelector()
    {
        HeroPrefab.transform.Find("Selector3").gameObject.SetActive(true);
    }
}
