using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField]
	string m_label = "";
	[SerializeField]
	float m_sightRadius = 10.0f;
	[SerializeField]
	float m_shootingIntervalSec = 2.0f;
	[SerializeField]
	float m_projectileSpeed = 100.0f;
	[SerializeField]
	GameObject m_shootingHead = null;
	[SerializeField]
	Projectile m_projectilePrefab = null;

	Collider m_Collider;
	WaitForSeconds m_waitForShootingIterval;
	float m_sightRadiusSqr = 0.0f;
	int m_playerAndGroundLayerMask = 0;

	// TODO DO LAYER MASK ENUM

	public string label
	{
		get { return m_label; }
	}

	public Vector3 centerPoint
	{
		get { return m_Collider.bounds.center;  }
	}

	void Awake()
	{
		m_sightRadiusSqr = m_sightRadius * m_sightRadius ;
		m_Collider = GetComponent<Collider> ();
		m_waitForShootingIterval = new WaitForSeconds (m_shootingIntervalSec);
		m_playerAndGroundLayerMask = (1 << LayerMask.NameToLayer ("Ground")) | (1 << LayerMask.NameToLayer ("Player"));
	}

	void Start () {
		MessageBus.Instance.OnEnemyCreated (this);
		StartCoroutine (AttemptToShootRepeatetly());
	}

	void OnDestroy () {
		MessageBus.Instance.OnEnemyDestroyed (this);
		StopAllCoroutines ();
	}

	void OnDrawGizmos()
	{
		if (m_Collider != null) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere (centerPoint, m_sightRadius);
		}
	}

	IEnumerator AttemptToShootRepeatetly()
	{
		while (true) {
			AttemptToShoot ();
			yield return m_waitForShootingIterval; 
		}
	}

	void AttemptToShoot()
	{
		if ( !CanShoot ())
			return;
		ShootProjectile ();
	}


	bool CanShoot()
	{
		// Is player In Range
		GameObject player = PlayerManager.Instance.GetPlayer ();
		Vector3 enemyPlayerVector = player.transform.position - m_shootingHead.transform.position;
		if ( enemyPlayerVector.sqrMagnitude > m_sightRadiusSqr ) {
			return false;
		}

		// has line of sight with player
		{
			RaycastHit info;
			if (Physics.Raycast (m_shootingHead.transform.position, enemyPlayerVector.normalized, out info, enemyPlayerVector.sqrMagnitude, m_playerAndGroundLayerMask, QueryTriggerInteraction.Ignore)) {
				if (info.collider.tag != player.tag)
					return false;
			}
		}
		return true;
	}

	void ShootProjectile()
	{
		Vector3 predictedPosition = Vector3.zero;
		GetPredictPlayerPosition (out predictedPosition);

		// TODO swap for a pool
		Projectile projectile = Instantiate<Projectile>(m_projectilePrefab);
		Vector3 origin = m_shootingHead.transform.position;

		projectile.Init (m_shootingHead.transform.position, (predictedPosition - origin).normalized, m_projectileSpeed  );
	}

	void GetPredictPlayerPosition(out Vector3 predictedPlayerPosition)
	{
		GameObject player = PlayerManager.Instance.GetPlayer ();
		predictedPlayerPosition = player.transform.position;
	}




}
