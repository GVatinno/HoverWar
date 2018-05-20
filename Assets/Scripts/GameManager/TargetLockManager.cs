using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TargetLockManager
{
	int m_currentTargetIndex = -1;
	Enemy[] m_targetCache;

	private static TargetLockManager instance = null;

	public static TargetLockManager Instance {
		get {
			if (instance != null) {
				return instance; 
			}
			else {
				instance = new TargetLockManager ();
				return instance;
			}
		}
	}

	public Enemy currentTarget
	{
		get {
			if (!hasCurrentTarget)
				return null;
			return m_targetCache[m_currentTargetIndex];
		}
	}

	public bool hasCurrentTarget
	{
		get { return m_currentTargetIndex >= 0; }
	}

	private TargetLockManager()
	{
		MessageBus.Instance.OnEnemyChangedVisibility += OnEnemyChangedVisibility;
		MessageBus.Instance.OnEnemyCreated += OnEnemyListChanged;
		MessageBus.Instance.OnEnemyDestroyed += OnEnemyListChanged;
	}

	void OnEnemyListChanged(Enemy enemy)
	{
		RebuildEnemyTargetCache ();
	}


	private void RebuildEnemyTargetCache()
	{
		Enemy currenteEnemy = null;
		if (hasCurrentTarget) {
			currenteEnemy = m_targetCache [m_currentTargetIndex];
		}
		m_targetCache = EnemyManager.Instance.GetEnemies().ToArray();
		m_currentTargetIndex = Array.FindIndex (m_targetCache, element => element == currenteEnemy);
	}


	~TargetLockManager()
	{
		MessageBus.Instance.OnEnemyChangedVisibility -= OnEnemyChangedVisibility;
		MessageBus.Instance.OnEnemyCreated -= OnEnemyListChanged;
		MessageBus.Instance.OnEnemyDestroyed -= OnEnemyListChanged;
	}

	public void RequestChangeTarget()
	{
		if (m_targetCache == null) {
			RebuildEnemyTargetCache ();
		}
		int counter = 0;
		int count = m_targetCache.Length;
		while (counter < count) {
			m_currentTargetIndex = m_currentTargetIndex < count-1 ? m_currentTargetIndex + 1 : m_currentTargetIndex = 0;
			if (!m_targetCache [m_currentTargetIndex].isEnemyBehindThePlayer) {
				MessageBus.Instance.OnTargetLockedChanged ( m_targetCache [m_currentTargetIndex] );
				return;
			}
			++counter;
		}
		m_currentTargetIndex = -1;
		MessageBus.Instance.OnTargetLockedChanged ( null );
	}

	private void OnEnemyChangedVisibility(Enemy enemy, bool visibility)
	{
		if (!hasCurrentTarget)
			return;
		if (!visibility && m_targetCache[m_currentTargetIndex] == enemy ) {
			m_currentTargetIndex = -1;
			MessageBus.Instance.OnTargetLockedChanged ( null );
		}
	}
}
