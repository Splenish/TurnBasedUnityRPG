using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverWorldUI : MonoBehaviour {

	public Text moveText;


	GameObject currentUnit;

	GameObject gm;

	Button nextTurnButton;

	Button moveUnitButton;

	GameObject player;

	// Use this for initialization
	void Awake () {
		this.enabled = true;
		gm = GameObject.Find ("GameManager");
		currentUnit = gm.GetComponent<GameManager>().currentUnit;
		moveText.text = currentUnit.GetComponent<Unit>().remainingMovement.ToString() + "/" + currentUnit.GetComponent<Unit>().moveSpeed.ToString();
		nextTurnButton = GameObject.Find ("NextTurn").GetComponent<Button>();
		nextTurnButton.onClick.AddListener (NextTurn);
		moveUnitButton = GameObject.Find ("Move").GetComponent<Button>();
		player = GameObject.Find ("Player");
		moveUnitButton.onClick.AddListener (player.GetComponent<Player> ().MoveUnitButton);
		Debug.Log ("OpenWorldUi start");
	}

	// Update is called once per frame
	void Update () {
		this.enabled = true;
		if (this.enabled == false) {
			Debug.Log ("watafak");
			this.enabled = true;
		}
		//Debug.Log (nextTurnButton.onClick.GetPersistentEventCount());
		//Debug.Log ("pasaaaa");
	}

	public void NextTurn() {
		Debug.Log ("Nextrun");
		GameManager.GameState gs = gm.GetComponent<GameManager> ().CurrentGameState;

		if (gs == GameManager.GameState.myTurn && currentUnit.GetComponent<Unit>().moving != true) {

			gm.GetComponent<GameManager> ().CurrentGameState = GameManager.GameState.enemyTurn;

			//currentUnit.GetComponent<Unit> ().anim = currentUnit.GetComponent<Animator> ();

			//currentUnit.GetComponent<EnemyUnit> ().StartEnemyTurn ();

		}

	}
}