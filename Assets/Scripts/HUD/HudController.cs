using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour {

	// Rename intresting point script to HUDindicator
	[SerializeField]
	Indicator m_intrestingPointPrefab = null;
	[SerializeField]
	Indicator m_targetLockPrefab = null;

	Dictionary<int, Indicator> m_indicator = new Dictionary<int, Indicator>();
	int m_targetLockInstanceId = 0;

	void Awake () {
		MessageBus.Instance.OnEnemyCreated += OnEnemyCreated;
		MessageBus.Instance.OnEnemyChangedVisibility += OnEnemyChangedVisibility;
		MessageBus.Instance.OnTargetLockedChanged += OnTargetLockedChanged;
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
	}

	void OnDestroy()
	{
		MessageBus.Instance.OnEnemyCreated -= OnEnemyCreated;
		MessageBus.Instance.OnEnemyChangedVisibility -= OnEnemyChangedVisibility;
	}
		

	void OnEnemyCreated(Enemy enemy)
	{
		Indicator point = Instantiate<Indicator>(m_intrestingPointPrefab, this.transform, false);
		point.Init (enemy.centerPoint, enemy.label);
		m_indicator[enemy.GetInstanceID()] = point;
	}

	void OnEnemyDestroyed(Enemy enemy)
	{
		Indicator point = m_indicator [enemy.GetInstanceID ()];
		m_indicator.Remove (enemy.GetInstanceID ());
		Destroy (point.gameObject);
	}

	void OnEnemyChangedVisibility(Enemy enemy, bool visibility)
	{
		m_indicator [enemy.GetInstanceID ()].SetActive (visibility);
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
