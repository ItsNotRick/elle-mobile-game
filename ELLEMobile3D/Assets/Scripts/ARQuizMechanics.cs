using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARQuizMechanics : MonoBehaviour 
{
	private string[] items;
	private static LocalLanguagePacksData languagePack;
	private bool isReset;
	private string genre;
	private int correctIndex;
	int[] indexes = { 0, 0, 0 };
	private string selectedWord;
	private bool startComplete;
	private bool mutex;

	public Text wordToFind, wordChosen;
	public GameObject wordOne, wordTwo, wordThree;
	public Text wordDisplay;
	public GameObject correctPanel, incorrectPanel;
	public Text scoreDisplay;


	// Analytics data
	private string dataURL = "http://10.171.204.188/ELLEMobile/PushAnalyticsHS.php";

	int totalCorrect, totalIncorrect;
	float playedTime;
	string correctWords, incorrectWords;
	bool mutexAnalytics;

	// Use this for initialization
	IEnumerator Start()
	{
		startComplete = false;

		WWW termsData = new WWW("http://10.171.204.188/ELLEMobile/TermData.php");
		yield return termsData;
		string termData = termsData.text;
		genre = PlayerPrefs.GetString("Language Pack");
		isReset = false;

		// This function parses the termData into the items string array
		ParseTerms(termData);
		languagePack = new LocalLanguagePacksData(items, genre);
		UpdateIndexes();
		GenerateWordDisplay();

		// Proper intializations
		selectedWord = "";
		mutex = false;
		PlayerPrefs.SetInt("ARMechanics", 1);
		scoreDisplay.text = "0";

		// Analytics initialization
		totalCorrect = totalIncorrect = 0;
		playedTime = 0;
		correctWords =  "";
		incorrectWords =  "";
		mutexAnalytics = false;

		startComplete = true;
	}

	
	private void Update()
	{
		// This isn't ideal to compare here as it is being checked every frame
		if (startComplete)
		{
			WordComparison();
		}

		// Updates the played time since level load
		playedTime += Time.deltaTime;
	}

	public void PushAnalytics()
	{
		Debug.Log("HELLO");
		if (!mutexAnalytics)
			StartCoroutine(PushAnalyticsHelper());
	}

	IEnumerator PushAnalyticsHelper()
	{
		mutexAnalytics = true;

		WWWForm dataForm = new WWWForm();

		// Fields to add to the table
		dataForm.AddField("userIDPost", PlayerPrefs.GetInt("UserID"));
		dataForm.AddField("scorePost", scoreDisplay.text);
		dataForm.AddField("iWordsPost", correctWords);
		dataForm.AddField("iGenresPost", incorrectWords);
		dataForm.AddField("totalCorrectPost", totalCorrect);
		dataForm.AddField("totalIncorrectPost", totalIncorrect);
		dataForm.AddField("minsPlayedPost", (playedTime / 60).ToString());

		WWW www = new WWW(dataURL, dataForm);
		yield return www;

		Debug.Log(www.text.ToString());
		mutexAnalytics = false;
	}

	// This function compares the selected word to the correct index 
	private void WordComparison()
	{
		selectedWord = wordChosen.text;
		// The correct word is chosen
		if (selectedWord == languagePack.terms[indexes[correctIndex]].ToString())
		{
			UpdateIndexes();
			GenerateWordDisplay();
		
			if (!mutex)
				StartCoroutine(CorrectChoice());
		}
		// Length check is important here, since selected word is defualted to "". If we don't 
		// check for the size this would always be triggered
		// Wrong word is chosen
		else if (selectedWord.Length > 0 && selectedWord != languagePack.terms[indexes[correctIndex]].ToString())
		{			
			if (!mutex)
				StartCoroutine(IncorrectChoice());
			//Debug.Log(selectedWord + " : " + languagePack.terms[indexes[correctIndex]].ToString());
		}

		// Resets the words 
		selectedWord = wordChosen.text = "";
	}

	IEnumerator CorrectChoice()
	{
		mutex = true;
		correctPanel.SetActive(true);

		scoreDisplay.text = (System.Int32.Parse(scoreDisplay.text) + 5).ToString();
		correctWords = correctWords + " : " + wordChosen.text;
		totalCorrect++;

		PlayerPrefs.SetInt("ARMechanics", 0);
		yield return new WaitForSeconds(2f);
		PlayerPrefs.SetInt("ARMechanics", 1);

		wordOne.SetActive(true);
		wordTwo.SetActive(true);
		wordThree.SetActive(true);
		correctPanel.SetActive(false); 
		mutex = false;
	}

	IEnumerator IncorrectChoice()
	{
		mutex = true;
		incorrectPanel.SetActive(true);

		incorrectWords = incorrectWords + " : " + wordChosen.text;
		totalIncorrect++;

		PlayerPrefs.SetInt("ARMechanics", 0);
		yield return new WaitForSeconds(2f);
		PlayerPrefs.SetInt("ARMechanics", 1);

		wordOne.SetActive(true);
		wordTwo.SetActive(true);
		wordThree.SetActive(true);

		incorrectPanel.SetActive(false);
		mutex = false;
	}

	public void UpdateIndexes()
	{
		// These are in indexes in the languagePack.terms of the three generated questions
		if (languagePack.GetSize() == 1)
		{
			indexes = new int[] { 0, 0, 0 };
		}
		else if (languagePack.GetSize() == 2)
		{
			indexes = new int[] { 0, 1, 0 };
		}
		else if (languagePack.GetSize() == 3)
		{
			indexes = new int[] { 0, 1, 2 };
		}
		else
		{
			indexes = languagePack.GenerateThreeTerms();
		}
	}

	private void ParseTerms(string termData)
	{
		items = termData.Split(';');
	}

	public void GenerateWordDisplay()
	{
		// Sets the texts for the card display
		wordOne.GetComponent<TextMesh>().text = languagePack.terms[indexes[0]].ToString();
		wordTwo.GetComponent<TextMesh>().text = languagePack.terms[indexes[1]].ToString();
		wordThree.GetComponent<TextMesh>().text = languagePack.terms[indexes[2]].ToString();

		// Choose which term to use from the above three and if its the same, 
		// keep retrying until a different one is chosen
		System.Random r = new System.Random();
		int correctWord = r.Next(0, 3);

		while (correctIndex == correctWord)
		{
			correctWord = r.Next(0, 3);
		}
		correctIndex = correctWord;

		// Sets the UI Element to the proper english word
		wordToFind.text = languagePack.englishTerms[indexes[correctWord]].ToString();
	}
}

// This class is utilized in both this script and the "VoiceQuizMechanics" script.
// This class stores the language pack pulled at the start of the scene.
public class LocalLanguagePacksData
{
	public ArrayList terms;
	public ArrayList englishTerms;
	public ArrayList imagesForTerms;

	public LocalLanguagePacksData(string[] items, string genre)
	{
		terms = new ArrayList();
		englishTerms = new ArrayList();
		imagesForTerms = new ArrayList();

		string[] parsed, termsParsed, englishParsed, imagesParsed;
		for (int i = 0; i < items.Length - 1; i++)
		{
			parsed = items[i].Split('|');
			termsParsed = parsed[0].Split(':');
			englishParsed = parsed[1].Split(':');
			imagesParsed = parsed[5].Split(':');

			// Adds the fields to the corresponding arraylists
			if (parsed[2].Contains(genre))
			{
				terms.Add(termsParsed[1]);
				englishTerms.Add(englishParsed[1]);
				imagesForTerms.Add(imagesParsed[1]);
			}
		}
	}

	public void PrintLanguagePack()
	{
		for (int i = 0; i < terms.Count; i++)
		{
			Debug.Log(terms[i] + " : " + englishTerms[i] + " : " + imagesForTerms[i]);
		}
	}

	public int[] GenerateThreeTerms()
	{
		HashSet<string> hash = new HashSet<string>();
		int[] indexes = new int[3];

		int j = 0;
		while (hash.Count < 3)
		{
			System.Random r = new System.Random();
			int i = r.Next(0, terms.Count - 2);
			if (hash.Add(terms[i].ToString()))
			{
				indexes[j++] = i;
			}
		}

		return indexes;
	}

	public string[] GenerateSingleTerm()
	{
		HashSet<string> hash = new HashSet<string>();
		string[] wordGenerated = new string[2];

		System.Random r = new System.Random();
		int i = r.Next(0, terms.Count - 2);

		wordGenerated[0] = terms[i].ToString();
		wordGenerated[1] = englishTerms[i].ToString();

		return wordGenerated;
	}

	public int GetSize()
	{
		return terms.Count;
	}

	public void RemoveTermsWithNoImages()
	{
		// Note with each removal the terms.Count changes so we must adjust i accordingly
		for (int i = 0; i < terms.Count; i++)
		{
			if (imagesForTerms[i].ToString() == "")
			{
				terms.RemoveAt(i);
				englishTerms.RemoveAt(i);
				imagesForTerms.RemoveAt(i);
				i--;
			}
		}
	}
}

