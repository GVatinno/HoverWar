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
	[SerializeField]
	GameObject m_win = null;
	[SerializeField]
	GameObject m_lose = null;

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
		MessageBus.Instance.OnGameWon += OnGameWon;
		MessageBus.Instance.OnGameLost += OnGameLost;
	}

	void Start()
	{
		// instanciate target look and put it invisible
		Indicator point = Instantiate<Indicator>(m_targetLockPrefab, this.transform, false);
		point.name = "TargetLock";
		point.SetPosition (Vector3.zero);
		m_indicator[point.GetInstanceID()] = point;
		point.SetActive (false);
		m_targetLockInstanceId = point.GetInstanceID ();

		GameObject player = PlayerManager.Instance.GetPlayer ();
		Indicator playerIndicator = CreateIndicatorIn(m_healthIndicators, m_healthIndicatorPrefab, player.transform.position, player.GetInstanceID () );
		playerIndicator.SetLabel(player.GetComponent<Damageable>().m_health.ToString());

		m_win.SetActive (false);
		m_lose.SetActive (false);
	}

	Indicator CreateIndicatorIn(Dictionary<int, Indicator> dic, Indicator prefab, Vector3 position, int id)
	{
		Indicator indicator = Instantiate<Indicator>(prefab, this.transform, false);
		dic [id] = indicator;
		indicator.SetPosition (position);
		return indicator;
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
		MessageBus.Instance.OnGameWon -= OnGameWon;
		MessageBus.Instance.OnGameLost -= OnGameLost;
	}

	void OnEntityDamaged(GameObject obj, float health)
	{
		Indicator indicator = m_healthIndicators [obj.GetInstanceID ()];
		indicator.SetPosition (obj.transform.position);
		indicator.SetLabel (health.ToString ());
	}

	void OnUpdatePlayerIndicator()
	{
		GameObject player = PlayerManager.Instance.GetPlayer ();
		if ( m_healthIndicators.ContainsKey(player.GetInstanceID () ))
		{
			m_healthIndicators [player.GetInstanceID ()].SetPosition (player.transform.position);
		}
	}
		

	void OnEnemyCreated(Enemy enemy)
	{
		Indicator enemyIndicator = CreateIndicatorIn(m_indicator, m_intrestingPointPrefab, enemy.centerPoint, enemy.gameObject.GetInstanceID() );
		enemyIndicator.SetLabel ( enemy.label);

		Indicator health =	CreateIndicatorIn(m_healthIndicators, m_healthIndicatorPrefab, enemy.centerPoint, enemy.gameObject.GetInstanceID() );
		health.SetLabel (enemy.GetComponent<Damageable>().m_health.ToString());
	}

	void DestroyIndicator(Dictionary<int, Indicator> dic, int id)
	{
		Indicator point = dic [id];
		dic.Remove (id);
		Destroy (point.gameObject);
	}

	void OnEnemyDestroyed(Enemy enemy)
	{
		DestroyIndicator (m_indicator, enemy.gameObject.GetInstanceID ());
	}

	void OnEntityDestroyed(GameObject obj)
	{
		DestroyIndicator (m_healthIndicators, obj.GetInstanceID());
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
			point.SetPosition (Vector3.zero);
			point.SetActive (false);
		}
		else {
			point.SetPosition (enemy.centerPoint);
			point.SetActive (true);
		}
	}

	void OnGameWon()
	{
		m_win.SetActive (true);
	}

	void OnGameLost()
	{
		m_lose.SetActive (true);
	}
}
