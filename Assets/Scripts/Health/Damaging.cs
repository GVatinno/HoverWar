using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour {

	[SerializeField]
	float m_damage = 10.0f;

	public void Damage(GameObject obj)
	{
		Damageable damageable = obj.GetComponent<Damageable> ();
		if (!addDamage (damageable)) {
			damageable = obj.GetComponentInParent<Damageable> ();
			addDamage (damageable);
		}
	}

	bool addDamage( Damageable damageable )
	{
		if (damageable != null) {
			damageable.AddDamage (m_damage);
			return true;
		}
		return false;
	}
}
