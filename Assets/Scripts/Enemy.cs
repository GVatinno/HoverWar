using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField]
	string m_label;

	public string label
	{
		get { return m_label; }
	}

	void Start () {
		MessageBus.Instance.OnEnemyCreated (this);
	}

}
