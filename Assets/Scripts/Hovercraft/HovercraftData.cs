using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HovercraftData", menuName = "Data/HovercraftData")]
public class HovercraftData : ScriptableObject {
	[SerializeField]
	public float m_missileSpeed = 100.0f;
	[SerializeField]
	public float m_moveForceFactor = 7000.0f;
	[SerializeField]
	public float m_turnForceFactor = 800.0f;
	[SerializeField]
	public float m_propellerForceFactor = 1500.0f;
	[SerializeField]
	public float m_maxHeightToGround = 5.0f;
}