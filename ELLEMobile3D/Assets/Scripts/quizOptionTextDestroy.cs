using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quizOptionTextDestroy : MonoBehaviour {

	private GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find ("Player");
	}

	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.z < Player.transform.position.z) {
			Destroy (gameObject);
			GroundGeneration.spawnedSetOfPlatforms = false;
		}
		if(gameObject.transform.position.z  < Player.transform.position.z) 
			Destroy (gameObject, 1f);
	}
}
