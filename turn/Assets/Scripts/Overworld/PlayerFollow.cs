using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour {

    public Transform PlayerTransform;

    private Vector3 _cameraOffset;

    public bool LookAtPlayer = false;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool RotateAroundPlayer = true;

    public float RotationSpeed = 5.0f;

	GameObject gm;

	// Use this for initialization
	void Start () {
        _cameraOffset = transform.position - PlayerTransform.position;
		gm = GameObject.Find ("GameManager");
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (gm.GetComponent<GameManager> ().currentUnit != null) {
			if (gm.GetComponent<GameManager> ().currentUnit.GetComponent<Unit> ().moving || gm.GetComponent<GameManager> ().CurrentGameState == GameManager.GameState.myTurn)
				PlayerTransform = gm.GetComponent<GameManager> ().currentUnit.transform;
		}

		Vector3 newPos;
		if (PlayerTransform != null) {
			newPos = PlayerTransform.position + _cameraOffset;
			transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
		}
        

        if (RotateAroundPlayer && Input.GetMouseButton(1))
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);

            _cameraOffset = camTurnAngle * _cameraOffset;
        }
        
        if (LookAtPlayer || RotateAroundPlayer)
            transform.LookAt(PlayerTransform);
	}
}
