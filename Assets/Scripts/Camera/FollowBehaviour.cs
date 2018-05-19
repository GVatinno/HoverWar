using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : MonoBehaviour 
{
	[SerializeField] 
	GameObject m_target = null;
	[SerializeField] 
	float m_yOffset = 0.0f;
	[SerializeField] 
	float m_zOffset = 0.0f;

	Vector3 m_prevPosition = Vector3.zero;
	Quaternion m_prevRotation = Quaternion.identity;

	void LateUpdate() {
		
		//this.transform.position = Vector3.SmoothDamp(transform.position, target, ref m_currentVelocity, Time.deltaTime);
		Vector3 moveBackVector = m_target.transform.rotation * Vector3.up;

		moveBackVector *= -m_zOffset;
		moveBackVector.y = -m_yOffset;

		Vector3 target = m_target.transform.position - moveBackVector;
		this.transform.position = target;
		this.transform.LookAt (m_target.transform);

		if (this.transform.position != m_prevPosition || m_prevRotation != this.transform.rotation)
			MessageBus.Instance.OnPlayerCameraMoved ();

		m_prevPosition = this.transform.position;
		m_prevRotation = this.transform.rotation;

	}
}
