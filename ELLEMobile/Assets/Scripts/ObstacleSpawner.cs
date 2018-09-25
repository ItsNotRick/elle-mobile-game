using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
	public GameObject obstacle;
	public GameObject player;
	private int selectedDiff;
	void Start () 
	{
		selectedDiff = PlayerPrefs.GetInt ("Difficulty Setting");
		if(selectedDiff == 0)
			//EASY
			InvokeRepeating ("SpawnRandomRepeating", 1, 2);
		else if(selectedDiff == 1)
			//MEDIUM
			InvokeRepeating ("SpawnRandomRepeating", 1, 1);
		else if(selectedDiff == 2)
			//HARD
			InvokeRepeating ("SpawnRandomRepeating", 1, 0.5f);
	}
		

	void SpawnRandomRepeating()
	{
		float intialWait = Random.Range (1.0f, 6.0f);
		Invoke("SpawnObs", intialWait);
	}

	void SpawnObs()
	{	float randomY = 0, randomDecider = Random.Range(0.0f,3.0f);
		//FIND A WAY TO ADD DIFFICULTIES
		if (randomDecider < 1.0) {
			randomY = -3.3765f;
		} else if (randomDecider <= 2.0) {
			randomY = -0.6234999f;
		} else if (randomDecider <= 3.0) {
			randomY = 2.1235f;
		}

		Vector3 spawnpt = new Vector3 (player.transform.position.x + 16, randomY, 0f);
		GameObject obstacleCloneTrigger = (GameObject)Instantiate (obstacle, spawnpt, transform.rotation);
	}



}
