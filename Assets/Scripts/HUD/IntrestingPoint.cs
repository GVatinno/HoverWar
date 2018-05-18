using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class IntrestingPoint : MonoBehaviour {

	string m_intrestingPointLabel = "";
	Text m_indicator = null;
	RectTransform m_rectTransfrom = null;
	RectTransform m_canvasRectTransfrom = null;
	Vector3 m_worldPosition = Vector3.zero;

	public Vector3 worldPosition
	{
		set { m_worldPosition = value; }
	}

	public string label
	{
		set { 
			m_intrestingPointLabel = value;
			m_indicator.text = m_intrestingPointLabel;
		}
	}

	void Awake()
	{
		m_indicator = GetComponentInChildren<Text> ();
		m_indicator.text = m_intrestingPointLabel;
		m_rectTransfrom = GetComponent<RectTransform> ();
		m_canvasRectTransfrom = this.transform.parent.GetComponent<RectTransform> ();
	}

	public void OnUpdateIntrestingPointPositionUpdated()
	{
		Camera playerCamera = CameraManager.Instance.GetPlayerCamera ();

		Vector3 eyeTargetVector = m_worldPosition - playerCamera.transform.position;
		if (Vector3.Dot (playerCamera.transform.forward, eyeTargetVector) >= 0)
		{
			Vector2 screenPos = playerCamera.WorldToScreenPoint (m_worldPosition);
			screenPos.x = Mathf.Clamp (screenPos.x, 0.0f, Screen.width);
			screenPos.y = Mathf.Clamp (screenPos.y, 0.0f, Screen.height);
			Vector2 localPos = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (m_canvasRectTransfrom, screenPos, null, out localPos);
			m_rectTransfrom.anchoredPosition = localPos;
			SetActive (true);
		} 
		else
		{
			SetActive (false);
		}
	}

	void SetActive(bool active)
	{
		if ( this.isActiveAndEnabled != active )
			this.gameObject.SetActive (active);
	}
}
