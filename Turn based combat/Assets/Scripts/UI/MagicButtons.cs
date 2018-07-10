using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicButtons : MonoBehaviour {

    public BaseAttack MagicAttackToPerform;

    public void CastMagicAttack()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input4(MagicAttackToPerform);
    }
}
