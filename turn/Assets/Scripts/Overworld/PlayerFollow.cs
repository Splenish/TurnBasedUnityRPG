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

	// Use this for initialization
	void Start () {
        _cameraOffset = transform.position - PlayerTransform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (RotateAroundPlayer && Input.GetMouseButton(1))
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);

            _cameraOffset = camTurnAngle * _cameraOffset;
        }
        
        if (LookAtPlayer || RotateAroundPlayer)
            transform.LookAt(PlayerTransform);
	}
}
