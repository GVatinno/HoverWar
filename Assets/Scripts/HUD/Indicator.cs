using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Indicator : MonoBehaviour {



	RectTransform m_rectTransfrom = null;
	RectTransform m_canvasRectTransfrom = null;
	Text m_textComponent = null;
	Vector3 m_worldPosition = Vector3.zero;
	float m_screenMargin = 10.0f;

	void Awake()
	{
		m_rectTransfrom = GetComponent<RectTransform> ();
		m_canvasRectTransfrom = this.transform.parent.GetComponent<RectTransform> ();
		m_textComponent = GetComponentInChildren<Text> ();
		MessageBus.Instance.OnPlayerCameraMoved += OnUpdateIntrestingPointPositionUpdated;
	}

	void OnDestroy()
	{
		MessageBus.Instance.OnPlayerCameraMoved -= OnUpdateIntrestingPointPositionUpdated;
	}

	public void SetPosition(Vector3 worldPosition)
	{
		m_worldPosition = worldPosition;
	}

	public void SetLabel(string label)
	{
		if (m_textComponent) {
			m_textComponent.text = label;
		}
	}

	public void OnUpdateIntrestingPointPositionUpdated()
	{
		if (this.isActiveAndEnabled) {
			Camera playerCamera = PlayerManager.Instance.GetPlayerCamera ();
			Vector2 screenPos = playerCamera.WorldToScreenPoint (m_worldPosition);
			screenPos.x = Mathf.Clamp (screenPos.x, m_screenMargin, Screen.width -m_screenMargin);
			screenPos.y = Mathf.Clamp (screenPos.y, m_screenMargin, Screen.height -m_screenMargin);
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
