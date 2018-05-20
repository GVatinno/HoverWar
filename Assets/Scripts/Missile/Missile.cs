﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

	[SerializeField]
	float m_selfGuidanceActivationDistance = 10.0f;
	float m_selfGuidanceActivationDistanceSqrd = 0.0f;
	TrailRenderer m_trailRenderer;
	bool m_selfGuidanceActivated = false;
	Vector3 m_target = Vector3.zero;
	Vector3 m_origin = Vector3.zero;
	float m_speed = 0.0f;
	float m_chasingSpeed = 0.0f;
	Vector3 m_velocity = Vector3.zero;

	public void Init( Vector3 origin, Vector3 direction, Vector3 target,  float speed, float chasingSpeed )
	{
		this.transform.rotation = Quaternion.FromToRotation (this.transform.forward, direction);
		this.transform.position = origin;
		m_trailRenderer = GetComponent<TrailRenderer> ();
		m_trailRenderer.Clear ();
		m_trailRenderer.enabled = false;
		m_speed = speed;
		m_target = target;
		m_origin = origin;
		m_chasingSpeed = chasingSpeed;
		m_selfGuidanceActivationDistanceSqrd = m_selfGuidanceActivationDistance * m_selfGuidanceActivationDistance;
	}

	void OnEnable()
	{
		StartCoroutine (ActivateTrailAfterDistance ());
	}

	void OnDisable()
	{
		StopAllCoroutines ();
	}

	IEnumerator ActivateTrailAfterDistance()
	{
		yield return new WaitUntil ( () => {
			if ((this.transform.position - m_origin).sqrMagnitude > m_selfGuidanceActivationDistanceSqrd) {
				m_selfGuidanceActivated = true;
				m_trailRenderer.enabled = true;
				return true;
			}
			return false;
		});
	
		
	}

	void OnDestroy()
	{
		StopAllCoroutines ();
	}

	void FixedUpdate () {
		if (m_selfGuidanceActivated) {
			this.transform.rotation = Quaternion.RotateTowards (
				this.transform.rotation,
				Quaternion.LookRotation (m_target - this.transform.position),
				Time.fixedDeltaTime * m_chasingSpeed);
		}
		this.transform.position += this.transform.forward * m_speed  * Time.fixedDeltaTime;
	}



	void OnTriggerEnter(Collider other) {
		PoolManager.Instance.returnPoolElement (PoolManager.PoolType.MISSILE, this.gameObject);
		GameObject explosion = PoolManager.Instance.GetPoolElement (PoolManager.PoolType.EXPLOSION);
		if (explosion) {
			explosion.transform.position = this.transform.position;
			explosion.SetActive (true);
		}

	}
}
