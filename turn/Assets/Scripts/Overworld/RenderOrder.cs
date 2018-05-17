using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RenderOrder : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
		Material GridLines = gameObject.GetComponent<MeshRenderer> ().material;
		GridLines.renderQueue = 10;

	}
}
