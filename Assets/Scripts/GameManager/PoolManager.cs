using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoolManager : MonoBehaviour {

	public enum PoolType
	{
		PROJECTILE = 0,
		MISSILE = 1
	}

	[SerializeField]
	MonoPool[] m_pools;

	public static PoolManager Instance = null;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
		else {
			Debug.Assert (false,"Trying to recreate this singleton");
			Destroy (this.gameObject);
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
