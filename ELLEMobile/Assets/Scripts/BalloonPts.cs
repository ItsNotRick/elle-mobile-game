using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPts : MonoBehaviour {

	public int scoreToGive;
	private ScoreGenerator ourScoreGen;
	
	// Use this for initialization
	void Start () {
			ourScoreGen = FindObjectOfType<ScoreGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			ourScoreGen.AddScore(scoreToGive);
		}
		
	}
}
