using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;	

public class WrongAnswer : MonoBehaviour {

	private LivesCounter lc;
	public string word;
	public AudioSource hurtSound;

	// Use this for initialization
	void Start () 
	{
		lc = FindObjectOfType<LivesCounter>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			int playSound = PlayerPrefs.GetInt("Sound Toggle");
			if (playSound == 1)
			{
				hurtSound.Play();
				Debug.Log("PLAY");
			}

			PlayerPrefs.SetString ("wrong" + lc.lives, word);
			lc.reduceScore();

			// Destroys the obstacle after .25 seconds which is long enough to 
			// play the hurt sound
			Destroy (gameObject, .25f);
		}
		
	}
}
