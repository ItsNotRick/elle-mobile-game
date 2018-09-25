using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LivesCounter : MonoBehaviour 
{
	public int lives;
	public Text livesText;
	private ScoreGenerator ourSC;

    // Use this for initialization
	void Start () 
    {
		lives = PlayerPrefs.GetInt ("Lives");
        livesText.text = "Lives: " + lives;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (PlayerPrefs.GetInt("Lives") <= 0)
		{
			ourSC = FindObjectOfType<ScoreGenerator>();
			PlayerPrefs.SetFloat("Player Score", ourSC.scoreCount);
			SceneManager.LoadScene("LoseScreen");
		}
	}
	public void reduceScore()
	{
		lives--;
		PlayerPrefs.SetInt ("Lives", lives);
        livesText.text = "Lives: " + lives;
	}
		
} 
