using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField]
	string m_label;

	Collider m_Collider;

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
		m_Collider = GetComponent<Collider> ();
	}

	void Start () {
		MessageBus.Instance.OnEnemyCreated (this);

	}

}
