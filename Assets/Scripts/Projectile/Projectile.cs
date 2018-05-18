﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	Vector3 m_velocity = Vector3.zero;

	public void Init( Vector3 origin, Vector3 direction, float speed )
	{
		this.transform.position = origin;
		m_velocity = direction * speed;
	}

	void Update () {
		this.transform.position += m_velocity * Time.deltaTime;
	}

	void OnCollisionEnter(Collision collision)
	{
		

	}

	void OnTriggerEnter(Collider other) {
		GameObject player = PlayerManager.Instance.GetPlayer ();
		if (other.tag == player.tag) {
			other.attachedRigidbody.AddForceAtPosition (m_velocity * 10.0f, other.ClosestPoint(this.transform.position), ForceMode.Impulse);
		}
		Destroy(this.gameObject);
	}

}