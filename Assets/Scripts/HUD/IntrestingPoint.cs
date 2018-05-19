using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class IntrestingPoint : MonoBehaviour {



	RectTransform m_rectTransfrom = null;
	RectTransform m_canvasRectTransfrom = null;
	Vector3 m_worldPosition = Vector3.zero;

	void Awake()
	{
		m_rectTransfrom = GetComponent<RectTransform> ();
		m_canvasRectTransfrom = this.transform.parent.GetComponent<RectTransform> ();
	}

	public void Init(Vector3 worldPosition)
	{
		m_worldPosition = worldPosition;
		OnUpdateIntrestingPointPositionUpdated ();
	}

	public void Init(Vector3 worldPosition, string label)
	{
		Init (worldPosition);
		GetComponentInChildren<Text> ().text = label;
	}

	public void OnUpdateIntrestingPointPositionUpdated()
	{
		if (this.isActiveAndEnabled) {
			Camera playerCamera = CameraManager.Instance.GetPlayerCamera ();
			Vector2 screenPos = playerCamera.WorldToScreenPoint (m_worldPosition);
			screenPos.x = Mathf.Clamp (screenPos.x, 0.0f, Screen.width);
			screenPos.y = Mathf.Clamp (screenPos.y, 0.0f, Screen.height);
			Vector2 localPos = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (m_canvasRectTransfrom, screenPos, null, out localPos);
			m_rectTransfrom.anchoredPosition = localPos;
		}
	}

	public void SetActive(bool active)
	{
		if ( this.isActiveAndEnabled != active )
			this.gameObject.SetActive (active);
	}
}
