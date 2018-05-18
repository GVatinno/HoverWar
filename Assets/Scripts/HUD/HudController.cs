using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour {

	[SerializeField]
	IntrestingPoint m_intrestingPointPrefab;

	void Awake () {
		MessageBus.Instance.OnEnemyCreated += CreateIndicator;
	}

	void OnDestroy()
	{
		MessageBus.Instance.OnEnemyCreated -= CreateIndicator;
	}


	void CreateIndicator(Vector3 worldPosition)
	{
		IntrestingPoint point = Instantiate<IntrestingPoint>(m_intrestingPointPrefab, this.transform, false);
		point.OnUpdateIntrestingPointPositionUpdated (worldPosition);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
