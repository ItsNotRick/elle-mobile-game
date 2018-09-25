using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObstacle : MonoBehaviour 
{

	public GameObject obstacleClone;
	public GameObject player;

	void Start()
	{
		Destroy(gameObject, 10);
	}
}
