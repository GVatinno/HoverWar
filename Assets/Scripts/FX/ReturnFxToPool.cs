using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ReturnFxToPool : MonoBehaviour {

	[SerializeField]
	PoolManager.PoolType m_pool;
	int m_count = 0;
	int m_counter = 0;

	FxStopAction[] m_systems;
	void Awake()
	{
		m_systems = GetComponentsInChildren<FxStopAction>();
		m_count = m_systems.Length;
		for ( int i = 0; i < m_count; ++i )
		{
			m_systems [i].onParticleStopped += ReturnToPool;
		}
		
	}

	void Start()
	{
		m_counter = 0;
	}

	void OnDestroy()
	{
		m_count = m_systems.Length;
		for ( int i = 0; i < m_count; ++i )
		{
			m_systems [i].onParticleStopped -= ReturnToPool;
		}
	}

	public void ReturnToPool()
	{
		++m_counter;
		PoolManager.Instance.returnPoolElement (m_pool, this.gameObject);
	}
}
