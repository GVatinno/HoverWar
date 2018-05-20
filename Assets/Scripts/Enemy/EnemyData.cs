using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject {
	[SerializeField]
	public string m_label = "";
	[SerializeField]
	public float m_sightRadius = 80.0f;
	[SerializeField]
	public float m_shootingIntervalSec = 2.0f;
	[SerializeField]
	public float m_projectileSpeed = 100.0f;

	[SerializeField]
	public Projectile m_projectilePrefab = null;
}