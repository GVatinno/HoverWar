using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : MonoBehaviour 
{
	[SerializeField] 
	GameObject m_target = null;
	[SerializeField] 
	Vector3 m_offset = Vector3.zero;


	void LateUpdate() {
		this.transform.position = m_target.transform.position + m_offset;
		this.transform.LookAt (m_target.transform.position);
	}
}
