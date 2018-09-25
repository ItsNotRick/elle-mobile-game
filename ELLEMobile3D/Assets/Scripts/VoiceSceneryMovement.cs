using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceSceneryMovement : MonoBehaviour 
{
	public GameObject[] sceneryObjects;

	// Update is called once per frame
	void Update () 
	{
		float moveSpeed = .5f;
		for (int i = 0; i < sceneryObjects.Length; i++)
		{
			sceneryObjects[i].transform.position = new Vector3(sceneryObjects[i].transform.position.x - moveSpeed, sceneryObjects[i].transform.position.y, sceneryObjects[i].transform.position.z);
		}

		// When last object moves off screen, reset the loop
		if (sceneryObjects[4].transform.position.x <= -30)
		{
			for (int i = 0; i < sceneryObjects.Length; i++)
			{
				sceneryObjects[i].transform.position = new Vector3((60 * i) + 65, sceneryObjects[i].transform.position.y, sceneryObjects[i].transform.position.z);
			}
		}
	}
}
