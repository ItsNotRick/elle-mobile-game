using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject musicOn, musicOff;
	public GameObject soundOn, soundOff;
	public GameObject motionOn, motionOff;
	public GameObject scoreOn, scoreOff;
	public GameObject diffEasy, diffMed, diffHard;

	public AudioSource menuMusic;
	public MusicPlayer musicPlayer;

	public void Start()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		DisplayOptionButtons();
		DisplayDifficultyButtons();
		PlayerPrefs.SetFloat("Player Score", 0);
		PlayerPrefs.SetInt ("Lives", 3);
		
	}

	public void LangugePacks()
	{
		SceneManager.LoadScene("LanguagePacks");
	}

	public void PlayGame()
	{
		SceneManager.LoadScene("Level1");
	}

	public void TutorialButton()
	{
		SceneManager.LoadScene("Tutorial");
	}

	public void LeaderboardButton()
	{
		SceneManager.LoadScene("Leaderboard");
	}

	public void QuitButton()
	{
		Application.Quit();
	}

	public void LogoutButton()
	{
		PlayerPrefs.SetString("Username", "");
		PlayerPrefs.SetString("Password", "");
		SceneManager.LoadScene("Login");
	}

	// Clicking difficulty easy transitions pref to medium
	public void DifficultyEasyOnClick()
	{
		PlayerPrefs.SetInt("Difficulty Setting", 1);
	}

	// Clicking difficulty medium transitions pref to hard
	public void DifficultyMediumOnClick()
	{
		PlayerPrefs.SetInt("Difficulty Setting", 2);
	}

	// Clicking difficulty hard transitions pref to easy
	public void DifficultyHardOnClick()
	{
		PlayerPrefs.SetInt("Difficulty Setting", 0);
	}

	// When the music is currently on and the button to turn it off is pressed
	// this function occurs
	public void MusicOnClick()
	{
		PlayerPrefs.SetInt("Music Toggle", 0);
		MusicPlayer.instance.PauseMusic();
	}

	// When the music is currently off and the button to turn it on is pressed
	// this function occurs
	public void MusicOffClick()
	{
		PlayerPrefs.SetInt("Music Toggle", 1);
		MusicPlayer.instance.ResumeMusic();
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

	public void DisplayOptionButtons()
	{
		DisplayMusicButtons();
		DisplaySoundButtons();
		DisplayMotionControlsButtons();
		DisplayScoresButtons();
	}

	public void DisplayDifficultyButtons()
	{
		int val = PlayerPrefs.GetInt("Difficulty Setting");

		// easy
		if (val == 0)
		{
			diffEasy.SetActive(true);
			diffMed.SetActive(false);
			diffHard.SetActive(false);
		}
		// medium
		else if (val == 1)
		{
			diffEasy.SetActive(false);
			diffMed.SetActive(true);
			diffHard.SetActive(false);
		}
		// hard
		else if (val == 2)
		{
			diffEasy.SetActive(false);
			diffMed.SetActive(false);
			diffHard.SetActive(true);
		}
	}

	public void DisplayMusicButtons()
	{
		int val = PlayerPrefs.GetInt("Music Toggle");

		// Music is on
		if (val == 1)
		{
			musicOn.SetActive(true);
			musicOff.SetActive(false);
//			menuMusic.Play();
		}
		// Music is off
		else if (val == 0)
		{
//			menuMusic.Stop();
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

	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}

	public void MusicPref()
	{
		Debug.Log(PlayerPrefs.GetInt("Music Toggle"));
	}

    public void DefaultTimeScale()
    {
        Time.timeScale = 1;
    }
}
