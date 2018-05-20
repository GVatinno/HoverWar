using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager
{
	private static EnemyManager instance = null;
	private List<Enemy> m_enemyList = new List<Enemy>();

	public static EnemyManager Instance {
		get {
			if (instance != null) {
				return instance; 
			}
			else {
				instance = new EnemyManager ();
				return instance;
			}
		}
	}

	private EnemyManager()
	{
	}
		

	public List<Enemy> GetEnemies()
	{
		return m_enemyList;
	}

	public void RegisterEnemy( Enemy enemy )
	{
		m_enemyList.Add (enemy);
	}

	public void UnRegisterEnemy(Enemy enemy)
	{
		m_enemyList.Remove (enemy);
	}


}




