using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject player;
	private Vector3 cameraPos;
	private int offset = 7;

	void Start()
	{
		cameraPos = transform.position - player.transform.position;
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		followPlayer();
	}

	void followPlayer()
	{
		cameraPos.x = player.transform.position.x + offset;
		cameraPos.y = transform.position.y;
		cameraPos.z = transform.position.z;
		transform.position = cameraPos;
	}
}
