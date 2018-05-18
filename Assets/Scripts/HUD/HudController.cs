using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour {

	[SerializeField]
	IntrestingPoint m_intrestingPointPrefab = null;
	List<IntrestingPoint> m_intresingPointList = new List<IntrestingPoint>();

	// TODO perhaps find a way to update only on event
	// Add destruction

	void Awake () {
		MessageBus.Instance.OnEnemyCreated += CreateIntrestingPoint;
	}

	void Update()
	{
		foreach (IntrestingPoint p in m_intresingPointList)
		{
			p.OnUpdateIntrestingPointPositionUpdated ();
		}
	}

	void OnDestroy()
	{
		MessageBus.Instance.OnEnemyCreated -= CreateIntrestingPoint;
	}


	void CreateIntrestingPoint(Enemy enemy)
	{
		IntrestingPoint point = Instantiate<IntrestingPoint>(m_intrestingPointPrefab, this.transform, false);
		point.worldPosition = enemy.transform.position;
		point.label = enemy.label;
		point.OnUpdateIntrestingPointPositionUpdated ();
		m_intresingPointList.Add (point);
	}
}
