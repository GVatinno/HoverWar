using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftController : MonoBehaviour {

	private Rigidbody rigidBody;
	private const float moveForceFactor = 30.0f;
	private const float turnForceFactor = 0.8f;
	private const float upForceFactor = 60.0f;
	private const float maxHeightTotheGround = 3.0f;

	private float moveForceMagnitude = 0.0f;
	private float turnForceMagnitude = 0.0f;

	private int groundLayerMask;

	// TODO REFACTOR THIS IN SEPARATING INPUT FROM CONTROLLER
	// TODO USING SCRIPTABLE OBJECT

	void Awake () {
		groundLayerMask = (1 << LayerMask.NameToLayer("Ground"));
		rigidBody = GetComponent<Rigidbody> ();
	}

	void Update () {
		moveForceMagnitude = Input.GetAxis ("Vertical") * moveForceFactor;
		turnForceMagnitude = Input.GetAxis ("Horizontal") * turnForceFactor;
	}

	void FixedUpdate()
	{
		ComputeHoverForce ();
		ComputeMovementForces ();
	}

	void ComputeHoverForce()
	{
		RaycastHit groundHit;
		Ray ray = new Ray (transform.position, Vector3.down);
		bool isCollingWithGround = Physics.Raycast (ray, out groundHit, maxHeightTotheGround, groundLayerMask, QueryTriggerInteraction.Ignore);
		if (isCollingWithGround) {
			float upForceDistanceFactor = (maxHeightTotheGround - groundHit.distance) / maxHeightTotheGround;
			rigidBody.AddForce (Vector3.up * upForceDistanceFactor * upForceFactor);
		}
	}

	void ComputeMovementForces()
	{
		rigidBody.AddRelativeForce (transform.forward * moveForceMagnitude);
		rigidBody.AddRelativeTorque (transform.up * turnForceMagnitude);
	}
}
