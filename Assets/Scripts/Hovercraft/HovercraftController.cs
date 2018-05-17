using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftController : MonoBehaviour {

	private Rigidbody rigidBody;
	private const float moveForceFactor = 30.0f;
	private const float turnForceFactor = 0.8f;
	private const float propellerForceFactor = 60.0f;
	private const float maxHeightToGround = 3.0f;


	private float moveForceMagnitude = 0.0f;
	private float turnForceMagnitude = 0.0f;


	public float MaxHeightToGround
	{
		get { return maxHeightToGround; }
	}

	public float PropellerForceFactor
	{
		get { return propellerForceFactor; }
	}


	// TODO REFACTOR THIS IN SEPARATING INPUT FROM CONTROLLER
	// TODO USING SCRIPTABLE OBJECT

	void Awake () {
		
		rigidBody = GetComponent<Rigidbody> ();
	}

	void Update () {
		moveForceMagnitude = Input.GetAxis ("Vertical") * moveForceFactor;
		turnForceMagnitude = Input.GetAxis ("Horizontal") * turnForceFactor;
	}

	void FixedUpdate()
	{
		rigidBody.AddRelativeForce (transform.forward * moveForceMagnitude);
		rigidBody.AddRelativeTorque (transform.up * turnForceMagnitude);
	}
}
