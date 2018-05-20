using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowBehaviourData", menuName = "Data/FollowBehaviourData")]
public class FollowBehaviourData : ScriptableObject {
	[SerializeField]
	public float m_moveBackDistance = 0.0f;
	[SerializeField]
	public float m_fixedHeightDistance = 0.0f;
}