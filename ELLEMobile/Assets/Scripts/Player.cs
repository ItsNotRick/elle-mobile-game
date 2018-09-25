using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject player;
	public int moveSpeed = 20;
	public Vector3 playerDirection = Vector3.right;

	// Update is called once per frame
	void Update ()
	{
		Movement();
	}

	public void Movement()
	{
		transform.Translate(playerDirection * moveSpeed * Time.deltaTime);
	}
}
