using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftPropeller : MonoBehaviour {

	[SerializeField]
	HovercraftData m_data;

	private Rigidbody m_rigidBody;
	private HovercraftController m_hovercraft;
	private int m_groundLayerMask;
	private float m_propulsorYOffset;

	void Awake () {
		m_hovercraft = GetComponentInParent<HovercraftController> ();
		m_rigidBody = m_hovercraft.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		
		RaycastHit groundHit;
		Ray ray = new Ray (transform.position, Vector3.down);
		bool isCollingWithGround = Physics.Raycast (ray, out groundHit, m_data.m_maxHeightToGround, LayerMaskUtils.GROUND_LAYER_MASK, QueryTriggerInteraction.Ignore);

		if (isCollingWithGround)
		{
			float upForceDistanceFactor = 1.0f - groundHit.distance / m_data.m_maxHeightToGround;
			m_rigidBody.AddForceAtPosition (Vector3.up * upForceDistanceFactor * m_data.m_propellerForceFactor, transform.position);
		
		} else
		{
			float directionFactor = m_hovercraft.transform.position.y > transform.position.y ? 1.0f : -1.0f;
			m_rigidBody.AddForceAtPosition (transform.up * directionFactor * m_data.m_propellerForceFactor, transform.position);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawRay (transform.position, transform.up);
	}
}
