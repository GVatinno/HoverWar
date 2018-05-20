using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour {

	[SerializeField]
	GameObject m_win = null;
	[SerializeField]
	GameObject m_lose = null;

	int m_enemyCount = 0;
	int m_enemyCounter = 0;
	void Awake () {
		MessageBus.Instance.OnEntityDead += OnEntityDead;
		m_enemyCount = EnemyManager.Instance.GetEnemies ().Count;
		m_enemyCounter = 0;
		m_win.SetActive (false);
		m_lose.SetActive (false);
	}

	void OnEntityDead( GameObject obj )
	{
		if (obj.tag == "Player") {
			// lost
			m_lose.SetActive(true);
		}
		else
		if (obj.tag == "Enemy") {
			++m_enemyCounter;
			if (m_enemyCounter >= m_enemyCount) {
				// won
				m_win.SetActive(true);
				
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
	

	void OnDestroy () {
		MessageBus.Instance.OnEntityDead -= OnEntityDead;
	}
}
