﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Indicator : MonoBehaviour {



	RectTransform m_rectTransfrom = null;
	RectTransform m_canvasRectTransfrom = null;
	Vector3 m_worldPosition = Vector3.zero;

	void Awake()
	{
		m_rectTransfrom = GetComponent<RectTransform> ();
		m_canvasRectTransfrom = this.transform.parent.GetComponent<RectTransform> ();
		MessageBus.Instance.OnPlayerCameraMoved += OnUpdateIntrestingPointPositionUpdated;
	}

	void OnDestroy()
	{
		MessageBus.Instance.OnPlayerCameraMoved -= OnUpdateIntrestingPointPositionUpdated;
	}

	public void Init(Vector3 worldPosition)
	{
		m_worldPosition = worldPosition;
	}

	public void Init(Vector3 worldPosition, string label)
	{
		Init (worldPosition);
		GetComponentInChildren<Text> ().text = label;
	}

	public void OnUpdateIntrestingPointPositionUpdated()
	{
		if (this.isActiveAndEnabled) {
			Camera playerCamera = PlayerManager.Instance.GetPlayerCamera ();
			Vector2 screenPos = playerCamera.WorldToScreenPoint (m_worldPosition);
			screenPos.x = Mathf.Clamp (screenPos.x, 0.0f, Screen.width);
			screenPos.y = Mathf.Clamp (screenPos.y, 0.0f, Screen.height);
			RectTransformUtility.ScreenPointToLocalPointInRectangle (m_canvasRectTransfrom, screenPos, null, out screenPos);
			m_rectTransfrom.anchoredPosition = screenPos;
		}
	}

	public void SetActive(bool active)
	{
		if ( this.isActiveAndEnabled != active )
			this.gameObject.SetActive (active);
	}
}
