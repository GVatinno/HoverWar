using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	void Start () {
		MessageBus.Instance.OnEnemyCreated (this.transform.position);
	}

}
