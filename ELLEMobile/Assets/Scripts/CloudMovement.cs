using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
	public GameObject cloud, camera;
	public float moveSpeed;
	public Vector3 cloudDirection = Vector3.right;
	private Vector3 cameraPos;
	public int offset;

	// Update is called once per frame
	void FixedUpdate()
	{
		Movement();
	}

	public void Movement()
	{
		transform.Translate(cloudDirection * moveSpeed * Time.deltaTime);

		if (cloud.transform.position.x < camera.transform.position.x - 10)
		{
			cloud.transform.position = new Vector2(camera.transform.position.x + offset, cloud.transform.position.y);
		}
	}
}
