using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour 
{
	public Text[] username;
	public Text[] score;

	private string[] items;

	// Use this for initialization
	IEnumerator Start()
	{
		WWW termsData = new WWW("http://10.171.204.188/ELLEMobile/HighScores.php");
		yield return termsData;
		string termData = termsData.text;
		ParseTerms(termData);
	}

	private void ParseTerms(string termData)
	{
		items = termData.Split(';');

		int val = Min(items.Length - 1, 10);
		for (int i = 0; i < val; i++)
		{
			string[] playerData = items[i].Split('|');

			if (playerData[0].Length > 14)
			{
				playerData[0] = playerData[0].Substring(0, 13);
			}
			username[i].text = playerData[0];
			score[i].text = playerData[1];
		}
	}

	int Min(int a, int b)
	{
		if (a > b)
			return b;
		else
			return a;
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
