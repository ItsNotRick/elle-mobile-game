using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOptions : MonoBehaviour
{
	public GameObject musicOn, musicOff;
	public GameObject soundOn, soundOff;
	public GameObject motionOn, motionOff;
	public GameObject scoreOn, scoreOff;
	public GameObject pauseCanvas;

	public void Start()
	{
		DisplayOptionButtons();
        Time.timeScale = 1f;
	}

	public void DisplayOptionButtons()
	{
		DisplayMusicButtons();
		DisplaySoundButtons();
		DisplayMotionControlsButtons();
		DisplayScoresButtons();
	}

	public void DisplayMusicButtons()
	{
		int val = PlayerPrefs.GetInt("Music Toggle");

		// Music is on
		if (val == 1)
		{
			musicOn.SetActive(true);
			musicOff.SetActive(false);
		}
		// Music is off
		else if (val == 0)
		{
			musicOn.SetActive(false);
			musicOff.SetActive(true);
		}
	}

	public void DisplaySoundButtons()
	{
		int val = PlayerPrefs.GetInt("Sound Toggle");

		// Sound is on
		if (val == 1)
		{
			soundOn.SetActive(true);
			soundOff.SetActive(false);
		}
		// Music is off
		else if (val == 0)
		{
			soundOn.SetActive(false);
			soundOff.SetActive(true);
		}
	}

	public void DisplayMotionControlsButtons()
	{
		int val = PlayerPrefs.GetInt("Motion Toggle");

		// motion is on
		if (val == 1)
		{
			motionOn.SetActive(true);
			motionOff.SetActive(false);
		}
		// motion is off
		else if (val == 0)
		{
			motionOn.SetActive(false);
			motionOff.SetActive(true);
		}
	}

	public void DisplayScoresButtons()
	{
		int val = PlayerPrefs.GetInt("Score Toggle");

		// motion is on
		if (val == 1)
		{
			scoreOn.SetActive(true);
			scoreOff.SetActive(false);
		}
		// motion is off
		else if (val == 0)
		{
			scoreOn.SetActive(false);
			scoreOff.SetActive(true);
		}
	}

	// When the music is currently on and the button to turn it off is pressed
	// this function occurs
	public void MusicOnClick()
	{
		PlayerPrefs.SetInt("Music Toggle", 0);
	}

	// When the music is currently off and the button to turn it on is pressed
	// this function occurs
	public void MusicOffClick()
	{
		PlayerPrefs.SetInt("Music Toggle", 1);
	}

	// When the sound is currently on and the button to turn it off is pressed
	// this function occurs
	public void SoundEffectsOnClick()
	{
		PlayerPrefs.SetInt("Sound Toggle", 0);
	}

	// When the sound is currently off and the button to turn it on is pressed
	// this function occurs
	public void SoundEffectsOffClick()
	{
		PlayerPrefs.SetInt("Sound Toggle", 1);
	}

	// When the motion is currently on and the button to turn it off is pressed
	// this function occurs
	public void MotionControlsOnClick()
	{
		PlayerPrefs.SetInt("Motion Toggle", 0);
	}

	// When the motion is currently off and the button to turn it on is pressed
	// this function occurs
	public void MotionControlsOffClick()
	{
		PlayerPrefs.SetInt("Motion Toggle", 1);
	}

	// When the score is currently on and the button to turn it off is pressed
	// this function occurs
	public void ScoreOnClick()
	{
		PlayerPrefs.SetInt("Score Toggle", 0);
	}

	// When the score is currently off and the button to turn it on is pressed
	// this function occurs
	public void ScoreOffClick()
	{
		PlayerPrefs.SetInt("Score Toggle", 1);
	}

	public void MenuPressed()
	{
		// This condition is to resume music when returning to main menu from a paused screen
		int musicValue = PlayerPrefs.GetInt("Music Toggle");
		if (musicValue == 1)
		{
			MusicPlayer.instance.ResumeMusic();
		}
		pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
	}
}
