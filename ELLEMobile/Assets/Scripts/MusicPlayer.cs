using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
	public static MusicPlayer instance;
	public AudioSource backgroundMusic;

	// Use this for initialization
	void Awake()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

	// Update is called once per frame
	void Start()
	{
		PlayMusic();
	}


	public void PlayMusic()
	{
		int val = PlayerPrefs.GetInt("Music Toggle");
		// Music is on
		if (val == 1)
		{
			backgroundMusic.Play();
		}
		// Music is off
		else if (val == 0)
		{
			backgroundMusic.Stop();
		}
	}

	public void PauseMusic()
	{
		backgroundMusic.Pause();
	}

	public void ResumeMusic()
	{
		int val = PlayerPrefs.GetInt("Music Toggle");

		// Music is on
		if (val == 1)
		{
			backgroundMusic.Play();
		}
	}

}

