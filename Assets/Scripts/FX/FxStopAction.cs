using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxStopAction : MonoBehaviour {


	public Action onParticleStopped = delegate{};

	void Awake () {
		ParticleSystem sys = GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = sys.main;
		main.stopAction = ParticleSystemStopAction.Callback;
	}

	public void OnParticleSystemStopped()
	{
		onParticleStopped ();
	}
}
