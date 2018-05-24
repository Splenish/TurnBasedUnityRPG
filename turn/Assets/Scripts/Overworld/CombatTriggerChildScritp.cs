using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTriggerChildScritp : MonoBehaviour {

	void OnTriggerEnter(Collider c) {
		gameObject.GetComponentInParent<EnemyUnit> ().PullTrigger (c);
	}

}
