using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class PlayerManager
{
	private static PlayerManager instance = null;
	private GameObject m_player;

	public static PlayerManager Instance {
		get {
			if (instance != null) {
				return instance; 
			}
			else {
				instance = new PlayerManager ();
				return instance;
			}
		}
	}

	private PlayerManager()
	{
	}

	public GameObject GetPlayer ()
	{
		Debug.Assert (m_player != null);
		return m_player; 
	}

	public void RegisterPlayer( GameObject player )
	{
		m_player = player;
	}

	public void UnRegisterPlayer()
	{
		m_player = null;
	}


}




