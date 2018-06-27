using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MongoloidScript : MonoBehaviour {


	GameObject gm;

	void Start() {
		gm = GameObject.Find ("GameManager");
	}

	public void OnMongoloidPress() {
		gm.GetComponent<GameManager> ().ActivateUnits ();	
		SceneManager.LoadScene ("OverWorld");
	}

}