using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour {

	// Rename intresting point script to HUDindicator
	[SerializeField]
	IntrestingPoint m_intrestingPointPrefab = null;
	[SerializeField]
	IntrestingPoint m_targetLockPrefab = null;

	Dictionary<int, IntrestingPoint> m_intresingPointDict = new Dictionary<int, IntrestingPoint>();
	int m_targetLockInstanceId = 0;
	// TODO perhaps find a way to update only on event
	// Add destruction

	void Awake () {
		MessageBus.Instance.OnEnemyCreated += OnEnemyCreated;
		MessageBus.Instance.OnEnemyChangedVisibility += OnEnemyChangedVisibility;
		MessageBus.Instance.OnPlayerCameraMoved += OnPlayerCameraMoved;
		MessageBus.Instance.OnTargetLockedChanged += OnTargetLockedChanged;
	}

	void Start()
	{
		// instanciate target look and put it invisible
		IntrestingPoint point = Instantiate<IntrestingPoint>(m_targetLockPrefab, this.transform, false);
		point.name = "TargetLock";
		point.Init (Vector3.zero);
		m_intresingPointDict[point.GetInstanceID()] = point;
		point.SetActive (false);
		m_targetLockInstanceId = point.GetInstanceID ();
	}

	void OnPlayerCameraMoved()
	{
		foreach(KeyValuePair<int, IntrestingPoint> entry in m_intresingPointDict)
		{
			entry.Value.OnUpdateIntrestingPointPositionUpdated ();
		}
	}

	void OnDestroy()
	{
		MessageBus.Instance.OnEnemyCreated -= OnEnemyCreated;
		MessageBus.Instance.OnEnemyChangedVisibility -= OnEnemyChangedVisibility;
		MessageBus.Instance.OnPlayerCameraMoved -= OnPlayerCameraMoved;
	}
		

	void OnEnemyCreated(Enemy enemy)
	{
		IntrestingPoint point = Instantiate<IntrestingPoint>(m_intrestingPointPrefab, this.transform, false);
		point.Init (enemy.centerPoint, enemy.label);
		m_intresingPointDict[enemy.GetInstanceID()] = point;
	}

	void OnEnemyDestroyed(Enemy enemy)
	{
		IntrestingPoint point = m_intresingPointDict [enemy.GetInstanceID ()];
		m_intresingPointDict.Remove (enemy.GetInstanceID ());
		Destroy (point.gameObject);
	}

	void OnEnemyChangedVisibility(Enemy enemy, bool visibility)
	{
		m_intresingPointDict [enemy.GetInstanceID ()].SetActive (visibility);
		if (enemy == TargetLockManager.Instance.currentTarget) {
			m_intresingPointDict [m_targetLockInstanceId].SetActive (visibility);
		}
	}

	void OnTargetLockedChanged(Enemy enemy)
	{
		IntrestingPoint point = m_intresingPointDict [m_targetLockInstanceId];
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
