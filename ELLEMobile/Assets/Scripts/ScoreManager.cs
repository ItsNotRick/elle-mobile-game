using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public Text scoreText;
	public float scoreCount;
	
	public float pointsPerSecond;
	public bool scoreIncreasing;

	// Use this for initialization
	void Start() 
	{
		scoreCount = PlayerPrefs.GetFloat("Player Score");
		//scoreText.text = score.ToString();
	
		//InvokeRepeating("CalculateScore", 0, 1.0f);
		
	}

	public void Update()
	{
		if(scoreIncreasing)
		{
			scoreCount += pointsPerSecond * Time.deltaTime;
		}
		
		scoreText.text = "Score : " + Mathf.Round(scoreCount);
		PlayerPrefs.SetFloat("Player Score", scoreCount);
	}
	
	public void AddScore(int points)
	{
		scoreCount += points;
	}
	
}
