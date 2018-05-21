using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoolManager : MonoBehaviour {

	public enum PoolType
	{
		PROJECTILE = 0,
		MISSILE = 1,
		EXPLOSION = 2,
		HIT = 3
	}

	[SerializeField]
	MonoPool[] m_pools = null;

	public static PoolManager Instance = null;

	void Awake () {
		if (Instance == null) {
			Instance = this;
		}
	}
	
	public GameObject GetPoolElement(PoolType type)
	{
		if ((int)type > m_pools.Length)
		{
			Debug.Assert (false,"Trying to access pool that does not exist");
			return null;
		}
		return m_pools [(int)type].Request(null, false);
	}

	public void returnPoolElement(PoolType type, GameObject item)
	{
		if ((int)type > m_pools.Length)
		{
			Debug.Assert (false,"Trying to access pool that does not exist");

		}
		m_pools [(int)type].Return (item);
	}
}
