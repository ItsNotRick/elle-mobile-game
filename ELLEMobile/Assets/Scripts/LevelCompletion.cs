using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletion : MonoBehaviour
{
	public Transform player;
	public Transform flag;
	public int numberLoops = 3;

	private float resetPos = -7;
	private int count;
	System.Random rand;
	
	private Vector2 speechReset;
	void Start()
	{
		speechReset = player.transform.position;
		count = 0;
	}

	// Update is called once per frame
	void Update ()
	{
        // Park level
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            LevelOneCompletion();
        }

        // Subway level
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            LevelTwoCompletion();
        }

		// Voice level
		if (SceneManager.GetActiveScene().name == "SpeechRecognitionLevel")
		{
			SpeechRecognitionCompletion();
		}
	}

    private void LevelTwoCompletion()
    {
        // Loops the player back to the beginning
        if (player.position.x >= flag.position.x)
        {
            SceneManager.LoadScene("Level1");
        }
    }

    private void LevelOneCompletion()
	{
		// Loops the player back to the beginning
		if (player.position.x >= flag.position.x)
		{
			float resetTotal = resetPos - player.position.x;
			player.position = new Vector2(resetPos, player.position.y);

			// Moves player
			player.position = new Vector2(resetPos, player.position.y);

            // After three loops through the level the player will go back 
            // to the subway level
            if (PlayerPrefs.GetInt("Lives") > 0 && count >= numberLoops - 1)
			{
				SceneManager.LoadScene("Level2");
            }
			count++;
		}
	}

	private void SpeechRecognitionCompletion()
	{
		if (player.transform.position.x >= flag.position.x)
			player.transform.position = speechReset;
	}

	public int GetLoopCount()
	{
		return count;
	}
}
