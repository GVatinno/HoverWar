using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

	[SerializeField]
	public float m_health = 100.0f;

	public Action onKilled = delegate {};

	public void AddDamage(float damage)
	{
		m_health -= damage;
		if (m_health <= 0.0f) {
			MessageBus.Instance.OnEntityDead (this.gameObject);
			onKilled ();
		}
	}
}
