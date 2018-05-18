﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBus {

	private static MessageBus instance = null;

	private MessageBus()
	{
	}

	public static MessageBus Instance {
		get {
			if (instance != null) {
				return instance; 
			}
			else {
				instance = new MessageBus ();
				return instance;
			}
		}
	}


	// Action 
	public Action<Vector3> OnEnemyCreated = delegate {};
}