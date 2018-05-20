using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Projectile : MonoBehaviour {

	[SerializeField]
	float m_forceImpactMultiplier = 10.0f;
	Vector3 m_velocity = Vector3.zero;

	public void Init( Vector3 origin, Vector3 direction, float speed )
	{
		this.transform.position = origin;
		GetComponent<TrailRenderer> ().Clear ();
		m_velocity = direction * speed;
	}

	void FixedUpdate () {
		this.transform.position += m_velocity * Time.fixedDeltaTime;
	}
		
	void OnTriggerEnter(Collider other) {
		GameObject player = PlayerManager.Instance.GetPlayer ();
		if (other.tag == player.tag) {
			other.attachedRigidbody.AddForceAtPosition (m_velocity * m_forceImpactMultiplier, other.ClosestPoint(this.transform.position), ForceMode.Impulse);
		}
		PoolManager.Instance.returnPoolElement (PoolManager.PoolType.PROJECTILE, this.gameObject);
		GameObject hit = PoolManager.Instance.GetPoolElement (PoolManager.PoolType.HIT);
		if (hit) {
			hit.transform.position = this.transform.position;
			hit.SetActive (true);
		}
	}

}
