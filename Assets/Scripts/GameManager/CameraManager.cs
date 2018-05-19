﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO KILL THIS MANAGER USE PLAYER MANAGER
// TODO use the lazy generic
public sealed class CameraManager
{
	private static CameraManager instance = null;
	private Camera m_playerCamera;

	private CameraManager()
	{
		m_playerCamera = Camera.main;
	}

	public static CameraManager Instance {
		get {
			if (instance != null) {
				return instance; 
			}
			else {
				instance = new CameraManager ();
				return instance;
			}
		}
	}
		

	public Camera GetPlayerCamera()
	{
		return m_playerCamera;
	}


}




