using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : MonoBehaviour 
{
	[SerializeField] 
	GameObject m_target = null;
	[SerializeField]
	FollowBehaviourData m_data = null;

	Vector3 m_prevPosition = Vector3.zero;
	Quaternion m_prevRotation = Quaternion.identity;
	Vector3 m_velocity = Vector3.zero;

	void LateUpdate() {

		Vector3 moveBackVector = Vector3.Slerp (this.transform.forward, m_target.transform.forward, Time.deltaTime);
		moveBackVector *= -m_data.m_moveBackDistance;
		moveBackVector.y = -m_data.m_fixedHeightDistance;

		Vector3 target = m_target.transform.position - moveBackVector;
		this.transform.position = target;
		this.transform.LookAt (m_target.transform);

		if (this.transform.position != m_prevPosition || m_prevRotation != this.transform.rotation)
			MessageBus.Instance.OnPlayerCameraMoved ();

		m_prevPosition = this.transform.position;
		m_prevRotation = this.transform.rotation;
	}
}
