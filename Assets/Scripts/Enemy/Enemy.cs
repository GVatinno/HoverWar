using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField]
	EnemyData m_data;
	[SerializeField]
	GameObject m_shootingHead = null;

	Collider m_Collider;
	WaitForSeconds m_waitForShootingIterval;
	float m_sightRadiusSqr = 0.0f;
	int m_playerAndGroundLayerMask = 0;
	bool m_enemyBehindPlayerCamera = false;


	public string label
	{
		get { return m_data.m_label; }
	}

	public Vector3 centerPoint
	{
		get { return m_Collider.bounds.center;  }
	}

	public bool isEnemyBehindThePlayer
	{
		get { return m_enemyBehindPlayerCamera;  }
	}

	void Awake()
	{
		m_sightRadiusSqr = m_data.m_sightRadius * m_data.m_sightRadius ;
		m_Collider = GetComponent<Collider> ();
		m_waitForShootingIterval = new WaitForSeconds (m_data.m_shootingIntervalSec);
		m_playerAndGroundLayerMask = (1 << LayerMask.NameToLayer ("Ground")) | (1 << LayerMask.NameToLayer ("Player"));
		MessageBus.Instance.OnPlayerCameraMoved += CheckEnemBehindPlayerCamera;
		EnemyManager.Instance.RegisterEnemy (this);
	}

	void Start () {
		MessageBus.Instance.OnEnemyCreated (this);
		StartCoroutine (AttemptToShootRepeatetly());
		CheckEnemBehindPlayerCamera ();
	}

	void OnDestroy () {
		EnemyManager.Instance.UnRegisterEnemy (this);
		MessageBus.Instance.OnPlayerCameraMoved -= CheckEnemBehindPlayerCamera;
		MessageBus.Instance.OnEnemyDestroyed (this);
		StopAllCoroutines ();
	}

	void OnDrawGizmos()
	{
		if (m_Collider != null) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere (centerPoint, m_data.m_sightRadius);
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
		ComputePredictPlayerPosition (out predictedPosition);

		// TODO swap for a pool
		Projectile projectile = Instantiate<Projectile>(m_data.m_projectilePrefab);
		Vector3 origin = m_shootingHead.transform.position;

		projectile.Init (m_shootingHead.transform.position, (predictedPosition - origin).normalized, m_data.m_projectileSpeed  );
	}

	void ComputePredictPlayerPosition(out Vector3 predictedPlayerPosition)
	{
		GameObject player = PlayerManager.Instance.GetPlayer ();
		Rigidbody playerRigidBody = PlayerManager.Instance.GetPlayerRigidBody ();
		// using the equation of ray and sphere intersection to predict
		// the hitpoint of the player P moving with costant velocity V
		// from an Enemy E shooting a projectile with constant velocity s
		// gives us the parametric equation for t
		// (P + tV - E )^2   = (st)^2

		Vector3 P = player.transform.position;
		Vector3 E = m_shootingHead.transform.position;
		Vector3 V = playerRigidBody.velocity;
		float s = m_data.m_projectileSpeed;
		Vector3 W = P - E;

		// which become a quadratic equation t^2V*V + 2W*V + W*W = st^2
		// of the type at^2 + 2ab + c = 0

		float a = Vector3.Dot (V, V) - (s*s);
		float b = 2.0f * Vector3.Dot (W, V);
		float c = Vector3.Dot (W, W);

		float t0 = 0.0f;
		float t1 = 0.0f;
		if (!MathUtils.computeQuadraticSolution (a, b, c, out t0, out t1)) {
			// if no solution are available return the player position
			predictedPlayerPosition = player.transform.position;
			return;
		}

		float t = t0;
		if (t < 0.0f)
			t = t1;

		predictedPlayerPosition = P + V * t;
	}

	void CheckEnemBehindPlayerCamera()
	{
		Camera playerCamera = CameraManager.Instance.GetPlayerCamera();
		Vector3 eyeTargetVector = centerPoint - playerCamera.transform.position;
		bool enemyBehindCamera = Vector3.Dot (playerCamera.transform.forward, eyeTargetVector) < 0;
		if (m_enemyBehindPlayerCamera != enemyBehindCamera) {
			MessageBus.Instance.OnEnemyChangedVisibility (this, !enemyBehindCamera);
		}
		m_enemyBehindPlayerCamera = enemyBehindCamera;
	}




}
