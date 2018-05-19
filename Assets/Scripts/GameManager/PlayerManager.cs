using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class PlayerManager
{
	private static PlayerManager instance = null;
	private GameObject m_player;
	private Rigidbody m_playerRigidBody;

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

	public Rigidbody GetPlayerRigidBody()
	{
		Debug.Assert (m_playerRigidBody != null);
		return m_playerRigidBody;
	}

	public void RegisterPlayer( GameObject player )
	{
		m_player = player;
		m_playerRigidBody = player.GetComponent<Rigidbody> ();
	}

	public void UnRegisterPlayer()
	{
		m_player = null;
	}


}




