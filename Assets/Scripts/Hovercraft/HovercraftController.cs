using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftController : MonoBehaviour {

	private Rigidbody rigidBody;
	private const float moveForceFactor = 5000.0f;
	private const float turnForceFactor = 500.0f;
	private const float propellerForceFactor = 1500.0f;
	private const float maxHeightToGround = 4.0f;


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
	// TODO USE m_ FOR PRIVATE MEMBERS AND LOWER CASE PROPERTY

	void Awake () {
		rigidBody = GetComponent<Rigidbody> ();
		PlayerManager.Instance.RegisterPlayer (this.gameObject);
		rigidBody.centerOfMass = Vector3.zero;
	}

	void OnDestroy()
	{
		PlayerManager.Instance.UnRegisterPlayer();
	}

	void Update () {
		moveForceMagnitude = Input.GetAxis ("Vertical") * moveForceFactor;
		turnForceMagnitude = Input.GetAxis ("Horizontal") * turnForceFactor;
		if ( Input.GetKeyDown(KeyCode.Space) )
		{
			TargetLockManager.Instance.RequestChangeTarget ();
		}
	}

	void FixedUpdate()
	{
		rigidBody.AddForce (transform.forward * moveForceMagnitude);
		rigidBody.AddRelativeTorque (Vector3.up * turnForceMagnitude);
	}
}
