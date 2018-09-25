using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAndCameraFollow : MonoBehaviour {

	public GameObject Player;
	public GameObject Camera;
	public int moveSpeed = 20;
	
	// Update is called once per frame
	void Update () 
	{
		Player.transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		Camera.transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		//Moves Player fowards at a constant rate. To change speed adjust moveSpeed
	}
}
