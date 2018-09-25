using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLoad : MonoBehaviour 
{
	public GameObject trashCan, vendingMachine;
	public AudioSource soundEffect;

	// Use this for initialization
	void Start ()
	{
		int playSound = PlayerPrefs.GetInt("Sound Toggle");
		if (playSound == 1)
		{
			soundEffect.Play();
		}

		int randIcon = Random.Range(0, 2);
		if (randIcon == 0)
		{
			vendingMachine.SetActive(true);
			trashCan.SetActive(false);
		}
		else
		{
			trashCan.SetActive(true);
			vendingMachine.SetActive(false);
		}
	}
	
}
