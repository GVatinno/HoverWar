using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftPropeller : MonoBehaviour {

	private Rigidbody rigidBody;
	private HovercraftController hovercraft;
	private int groundLayerMask;


	void Awake () {
		
		hovercraft = GetComponentInParent<HovercraftController> ();
		rigidBody = hovercraft.GetComponent<Rigidbody> ();
		groundLayerMask = (1 << LayerMask.NameToLayer("Ground"));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		RaycastHit groundHit;
		Ray ray = new Ray (transform.position, Vector3.down);
		bool isCollingWithGround = Physics.Raycast (ray, out groundHit, hovercraft.MaxHeightToGround, groundLayerMask, QueryTriggerInteraction.Ignore);

		if (isCollingWithGround) {
			float upForceDistanceFactor = 1.0f - groundHit.distance / hovercraft.MaxHeightToGround;
			rigidBody.AddForceAtPosition (Vector3.up * upForceDistanceFactor * hovercraft.PropellerForceFactor, transform.position);
		}
	}
}
