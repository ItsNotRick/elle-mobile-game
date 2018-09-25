using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	public void PauseGame()
	{
		MusicPlayer.instance.PauseMusic();
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		MusicPlayer.instance.ResumeMusic();
		Time.timeScale = 1;
	}
}
