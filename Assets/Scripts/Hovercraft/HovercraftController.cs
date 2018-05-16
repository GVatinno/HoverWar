using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftController : MonoBehaviour {

	private Rigidbody rigidBody;
	private float moveParam = 0.0f;
	private float turnParam = 0.0f;
	private float maxHeightTotheGround = 3.0f;
	private int groundLayerMask; 
	// TODO REFACTOR THIS IN SEPARATING INPUT FROM CONTROLLER
	// TODO USING SCRIPTABLE OBJECT

	void Awake () {
		groundLayerMask = ~(1 << LayerMask.NameToLayer("Ground"));
		rigidBody = GetComponent<Rigidbody> ();
	}

	void Update () {
		moveParam = Input.GetAxis ("Vertical");
		turnParam = Input.GetAxis ("Horizontal");
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
			float upForceFactor = (maxHeightTotheGround - groundHit.distance) / maxHeightTotheGround;
			rigidBody.AddForce (Vector3.up * upForceFactor * 60.0f);
		}
	}

	void ComputeMovementForces()
	{
		rigidBody.AddRelativeForce (transform.forward * moveParam);
		rigidBody.AddRelativeTorque (transform.up * turnParam);
	}
}
