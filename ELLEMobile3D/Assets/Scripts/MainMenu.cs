using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	public GameObject diffEasy, diffMed, diffHard;
	public GameObject standard, protanopia, deuteranopia, tritanopia;
	public GameObject camera;
	public GameObject motionOffBtn, motionOnBtn;

	// Use this for initialization
	void Start () 
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		camera = GameObject.Find("Main Camera");
		DisplayDifficultyButtons();
		// Non functional
		//SetColorBlindnessButton();
		DisplayMotionButtons();
	}

	// Loads standard gameplay scene
	public void StandardGame()
	{
		SceneManager.LoadScene("StandardGame");
	}

	// Loads AR gameplay scene
	public void AugmentedRealityGame()
	{
		SceneManager.LoadScene("AR");
	}


	// Loads speech gameplay scene
	public void SpeechLevel()
	{
		SceneManager.LoadScene("SpeechLevel");
	}

	// Loads the language packs page
	public void LanguagePacksScene()
	{
		SceneManager.LoadScene("LanguagePacks");
	}

	// Loads the leaderboard Scene
	public void Leaderboard()
	{
		SceneManager.LoadScene("Leaderboard");
	}

	public void CharacterCustomization()
	{
		SceneManager.LoadScene("CharacterCustomization");
	}

	public void Credits()
	{
		SceneManager.LoadScene("Credits");
	}

	public void Tutorial()
	{
		SceneManager.LoadScene("Tutorial");
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

	// When motion controls is enabled, this button will be pressed and will turn it off
	public void MotionOn()
	{
		PlayerPrefs.SetInt("Motion Toggle", 0);
	}

	// When motion controls is disabled, this button will be pressed and will turn it on
	public void MotionOff()
	{
		PlayerPrefs.SetInt("Motion Toggle", 1);
	}

	public void DisplayMotionButtons()
	{
		int val = PlayerPrefs.GetInt("Motion Toggle");

		if (val == 0)
		{
			motionOffBtn.SetActive(true);
		}
		else if (val == 1)
		{
			motionOnBtn.SetActive(true);
		}
	}

	// Note: The color blindness modes cycle through another
	// When ColorBlindnessPref is 0 it is standard, 1 is protanopia
	// 2 is deuteranopia, 3 is tritanopia
	public void StandardVision()
	{
		// When pressing standard vision, update to protanopia mode
		PlayerPrefs.SetInt("ColorBlindness", 1);
	}

	public void Protanopia()
	{
		// When pressing protanopia, update to deuteranopia
		PlayerPrefs.SetInt("ColorBlindness", 2);
	}

	public void Deuteranopia()
	{
		// When pressing deuteranopia, update to tritanopia
		PlayerPrefs.SetInt("ColorBlindness", 3);
	}

	public void Tritanopia()
	{
		// When pressing tritanopia, update to standard
		PlayerPrefs.SetInt("ColorBlindness", 0);
	}

	// Determines which button to have enabled at start
	public void SetColorBlindnessButton()
	{
		int val = PlayerPrefs.GetInt("ColorBlindness");

		if (val == 0)
		{
			standard.SetActive(true);
		}
		else if (val == 1)
		{
			protanopia.SetActive(true);
		}
		else if (val == 2)
		{
			deuteranopia.SetActive(true);
		}
		else if (val == 3)
		{
			tritanopia.SetActive(true);
		}
	}
}
