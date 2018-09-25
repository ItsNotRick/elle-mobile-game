using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour 
{
	public void ResumeGame()
	{
		Time.timeScale = 1f;
		PlayerPrefs.SetInt("GamePaused", 0);
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
		PlayerPrefs.SetInt("GamePaused", 1);
	}

	public void MainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
		PlayerPrefs.SetInt("GamePaused", 0);
	}
}
