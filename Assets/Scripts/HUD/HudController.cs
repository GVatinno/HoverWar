using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour {

	// Rename intresting point script to HUDindicator
	[SerializeField]
	Indicator m_intrestingPointPrefab = null;
	[SerializeField]
	Indicator m_targetLockPrefab = null;
	[SerializeField]
	Indicator m_healthIndicatorPrefab = null;

	Dictionary<int, Indicator> m_indicator = new Dictionary<int, Indicator>();
	Dictionary<int, Indicator> m_healthIndicators = new Dictionary<int, Indicator>();
	int m_targetLockInstanceId = 0;

	void Awake () {
		MessageBus.Instance.OnEnemyCreated += OnEnemyCreated;
		MessageBus.Instance.OnEnemyDestroyed += OnEnemyDestroyed;
		MessageBus.Instance.OnEnemyChangedVisibility += OnEnemyChangedVisibility;
		MessageBus.Instance.OnTargetLockedChanged += OnTargetLockedChanged;
		MessageBus.Instance.OnPlayerCameraMoved += OnUpdatePlayerIndicator;
		MessageBus.Instance.OnEntityDamaged += OnEntityDamaged;
		MessageBus.Instance.OnEntityDead += OnEntityDestroyed;
	}

	void Start()
	{
		// instanciate target look and put it invisible
		Indicator point = Instantiate<Indicator>(m_targetLockPrefab, this.transform, false);
		point.name = "TargetLock";
		point.Init (Vector3.zero);
		m_indicator[point.GetInstanceID()] = point;
		point.SetActive (false);
		m_targetLockInstanceId = point.GetInstanceID ();
		Indicator playerHealth = Instantiate<Indicator>(m_healthIndicatorPrefab, this.transform, false);
		playerHealth.name = "HealthIndicators";
		GameObject player = PlayerManager.Instance.GetPlayer ();
		m_healthIndicators [player.GetInstanceID ()] = playerHealth;
		playerHealth.Init (player.transform.position, player.GetComponent<Damageable>().m_health.ToString());
	}

	void OnDestroy()
	{
		MessageBus.Instance.OnEnemyCreated -= OnEnemyCreated;
		MessageBus.Instance.OnEnemyDestroyed -= OnEnemyDestroyed;
		MessageBus.Instance.OnEnemyChangedVisibility -= OnEnemyChangedVisibility;
		MessageBus.Instance.OnTargetLockedChanged -= OnTargetLockedChanged;
		MessageBus.Instance.OnPlayerCameraMoved -= OnUpdatePlayerIndicator;
		MessageBus.Instance.OnEntityDamaged -= OnEntityDamaged;
		MessageBus.Instance.OnEntityDead -= OnEntityDestroyed;
	}

	void OnEntityDamaged(GameObject obj, float health)
	{
		m_healthIndicators [obj.GetInstanceID ()].Init (obj.transform.position, health.ToString());
	}

	void OnUpdatePlayerIndicator()
	{
		GameObject player = PlayerManager.Instance.GetPlayer ();
		if ( m_healthIndicators.ContainsKey(player.GetInstanceID () ))
		{
			m_healthIndicators [player.GetInstanceID ()].Init (player.transform.position);
		}
	}
		

	void OnEnemyCreated(Enemy enemy)
	{
		Indicator point = Instantiate<Indicator>(m_intrestingPointPrefab, this.transform, false);
		point.Init (enemy.centerPoint, enemy.label);
		m_indicator[enemy.gameObject.GetInstanceID()] = point;
		Indicator health = Instantiate<Indicator>(m_healthIndicatorPrefab, this.transform, false);
		health.Init (enemy.centerPoint, enemy.GetComponent<Damageable>().m_health.ToString());
		m_healthIndicators[enemy.gameObject.GetInstanceID()] = health;
	}

	void OnEnemyDestroyed(Enemy enemy)
	{
		Indicator point = m_indicator [enemy.gameObject.GetInstanceID()];
		m_indicator.Remove (enemy.gameObject.GetInstanceID());
		Destroy (point.gameObject);
	}

	void OnEntityDestroyed(GameObject obj)
	{
		Indicator health = m_healthIndicators [obj.GetInstanceID()];
		m_healthIndicators.Remove (obj.GetInstanceID());
		Destroy (health.gameObject);
	}

	void OnEnemyChangedVisibility(Enemy enemy, bool visibility)
	{
		m_indicator [enemy.gameObject.GetInstanceID()].SetActive (visibility);
		m_healthIndicators [enemy.gameObject.GetInstanceID()].SetActive (visibility);
		if (enemy == TargetLockManager.Instance.currentTarget) {
			m_indicator [m_targetLockInstanceId].SetActive (visibility);
		}
	}

	void OnTargetLockedChanged(Enemy enemy)
	{
		Indicator point = m_indicator [m_targetLockInstanceId];
		if (enemy == null) {
			point.Init (Vector3.zero);
			point.SetActive (false);
		}
		else {
			point.Init (enemy.centerPoint);
			point.SetActive (true);
		}
	}
}
