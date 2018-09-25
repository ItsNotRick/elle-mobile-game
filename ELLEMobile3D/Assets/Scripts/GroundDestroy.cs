using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroy : MonoBehaviour {
	private GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find ("Player");
			
	}

	void Update() {
		if(gameObject.transform.position.z  < Player.transform.position.z) 
			Destroy (gameObject, 0.25f);
	}
}
