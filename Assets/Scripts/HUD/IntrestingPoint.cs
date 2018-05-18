using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class IntrestingPoint : MonoBehaviour {

	[SerializeField]
	string m_intrestingPointLabel = "N";
	Text m_indicator = null;
	RectTransform m_rectTransfrom = null;

	void Awake()
	{
		m_indicator = GetComponentInChildren<Text> ();
		m_indicator.text = m_intrestingPointLabel;
		m_rectTransfrom = GetComponent<RectTransform> ();
	}

	public void OnUpdateIntrestingPointPositionUpdated(Vector3 worldPosition)
	{
		Camera playerCamera = CameraManager.Instance.GetPlayerCamera ();
		Debug.Log (playerCamera.WorldToScreenPoint (worldPosition));
		m_rectTransfrom.localPosition = playerCamera.WorldToScreenPoint (worldPosition);
	}
}
