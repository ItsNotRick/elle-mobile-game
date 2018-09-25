using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainMenu : MonoBehaviour
{
	public GameObject player;
	public int moveSpeed = 1;
	public Vector3 playerDirection = Vector3.right;
	private Vector3 playerStartPos;
	public int maxXPos;

	void Start()
	{
		playerStartPos = transform.position;
	}

	// Update is called once per frame
	void Update ()
	{
		Movement();
	}

	public void Movement()
	{
		transform.Translate(playerDirection * moveSpeed * Time.deltaTime);
		if (player.transform.position.x >= maxXPos)
		{
			transform.position = playerStartPos;
		}
	}
}
