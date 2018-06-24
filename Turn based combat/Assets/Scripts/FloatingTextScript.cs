using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour {

    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0, 2, 0);

	void Start () {
        Destroy(gameObject, DestroyTime);

        transform.position += Offset;
	}
	
	
}
