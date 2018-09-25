using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpeechQuizMechanics : MonoBehaviour 
{
	// This class is found in the ARQuizMechanics.cs
	private static LocalLanguagePacksData languagePack;
	private string[] items;
	private string genre;

	public GameObject rockObject;
	public Text wordToSay, hiddenTranslation, mostRecentWordSaid;

	private Vector3 rockPositionInitial;
	private float finalPositionZ = -22f;

	// Use this for initialization
	IEnumerator Start()
	{
		WWW termsData = new WWW("http://10.171.204.188/ELLEMobile/TermData.php");
		yield return termsData;
		string termData = termsData.text;
		genre = PlayerPrefs.GetString("Language Pack");

		// This function parses the termData into the items string array
		ParseTerms(termData);
		languagePack = new LocalLanguagePacksData(items, genre);

		UpdateWordDisplay();
		rockPositionInitial = rockObject.transform.position;
		RotateRock();
	}

	private void ParseTerms(string termData)
	{
		items = termData.Split(';');
	}

	private void UpdateWordDisplay()
	{
		wordToSay.text = languagePack.GenerateSingleTerm()[0];
		hiddenTranslation.text = languagePack.GenerateSingleTerm()[1];
	}

	public void CompareTerms()
	{
		// Correct choice
		if (hiddenTranslation.text.ToLower() == mostRecentWordSaid.text.ToLower())
		{
			UpdateWordDisplay();
			rockObject.transform.position = rockPositionInitial;
		}
		// Wrong choice
		else
		{
			// Do nothing;
		}
	}

	void Update()
	{
		if (PlayerPrefs.GetInt("GamePaused") == 0)
		{
			CompareTerms();
			RotateRock();
			MoveRock();
		}
	}

	void RotateRock()
	{
		rockObject.transform.Rotate(0, 50 * Time.deltaTime, 0); 
	}

	void MoveRock()
	{
		int val = PlayerPrefs.GetInt("Difficulty Setting");

		// easy
		if (val == 0)
			rockObject.transform.position = new Vector3(rockObject.transform.position.x, rockObject.transform.position.y, rockObject.transform.position.z - .5f);
		// medium
		else if (val == 1)
			rockObject.transform.position = new Vector3(rockObject.transform.position.x, rockObject.transform.position.y, rockObject.transform.position.z - .75f);
		// hard
		else
			rockObject.transform.position = new Vector3(rockObject.transform.position.x, rockObject.transform.position.y, rockObject.transform.position.z - 1f);

		if (rockObject.transform.position.z <= finalPositionZ)
		{
			GameOver();
		}
	}

	void GameOver()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
