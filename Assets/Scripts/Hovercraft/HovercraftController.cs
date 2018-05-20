using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftController : MonoBehaviour {

	[SerializeField]
	GameObject m_missileSource = null;
	[SerializeField]
	HovercraftData m_data = null;

	private Rigidbody m_rigidBody;
	private float m_moveForceMagnitude = 0.0f;
	private float m_turnForceMagnitude = 0.0f;

	void Awake () {
		m_rigidBody = GetComponent<Rigidbody> ();
		PlayerManager.Instance.RegisterPlayer (this.gameObject);
		m_rigidBody.centerOfMass = Vector3.zero;
	}

	void OnDestroy()
	{
		PlayerManager.Instance.UnRegisterPlayer();
	}

	void Update () {
		m_moveForceMagnitude = Input.GetAxis ("Vertical") * m_data.m_moveForceFactor;
		m_turnForceMagnitude = Input.GetAxis ("Horizontal") *  m_data.m_turnForceFactor;
		if ( Input.GetKeyDown(KeyCode.Space) )
		{
			TargetLockManager.Instance.RequestChangeTarget ();
		}
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter)) {
			ShootMissile ();
		}
	}

	void FixedUpdate()
	{
		m_rigidBody.AddForce (transform.forward * m_moveForceMagnitude);
		m_rigidBody.AddRelativeTorque (Vector3.up * m_turnForceMagnitude);
	}

	void ShootMissile()
	{
		if (TargetLockManager.Instance.hasCurrentTarget) {
			GameObject missileGameObject = PoolManager.Instance.GetPoolElement (PoolManager.PoolType.MISSILE);
			if (missileGameObject != null) {
				Missile missile = missileGameObject.GetComponent<Missile> ();
				missile.Init (
					m_missileSource.transform.position, 
					m_missileSource.transform.forward,
					TargetLockManager.Instance.currentTarget.centerPoint,
					m_data.m_missileSpeed,
					m_data.m_missileChasingSpeed);
				missileGameObject.SetActive (true);
			}
		}
	}
}
