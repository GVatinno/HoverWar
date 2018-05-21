using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour {



	int m_enemyCount = 0;
	int m_enemyCounter = 0;
	void Awake () {
		MessageBus.Instance.OnEntityDead += OnEntityDead;
		m_enemyCount = EnemyManager.Instance.GetEnemies ().Count;
		m_enemyCounter = 0;

	}

	void OnEntityDead( GameObject obj )
	{
		if (obj.tag == "Player") {
			// lost
			MessageBus.Instance.OnGameLost();
		}
		else
		if (obj.tag == "Enemy") {
			++m_enemyCounter;
			if (m_enemyCounter >= m_enemyCount) {
			// won
				MessageBus.Instance.OnGameWon();
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		if (Input.GetKeyDown (KeyCode.R))
			SceneManager.LoadScene ("HoverWar");
	}
	

	void OnDestroy () {
		MessageBus.Instance.OnEntityDead -= OnEntityDead;
	}
}
