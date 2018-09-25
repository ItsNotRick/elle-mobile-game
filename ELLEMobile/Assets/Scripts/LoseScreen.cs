using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour {

	public Text scoreText;
	public Text wrongAnswers;
	
	public void PlayAgain()
	{
		PlayerPrefs.SetFloat ("Player Score", 0);
		PlayerPrefs.SetInt ("Lives", 3);
		SceneManager.LoadScene("Level2");
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void Leaderboard()
	{
		SceneManager.LoadScene("Leaderboard");
	}

	// Use this for initialization
	void Start () 
	{
//		scoreText.text = "Score: " + Mathf.Round(PlayerPrefs.GetFloat("Player Score"));
//		wrongAnswers.text = "Wrong Answers: " + PlayerPrefs.GetString ("wrong3") + ", " + PlayerPrefs.GetString ("wrong2") + ", " + PlayerPrefs.GetString ("wrong1");
	}
	
	
}
