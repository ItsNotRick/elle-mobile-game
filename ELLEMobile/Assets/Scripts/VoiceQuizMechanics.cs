using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class VoiceQuizMechanics : MonoBehaviour
{
    private string[] items;
    private static LanguagePack languagePack;
    private string genre;
	private int[] indexes;

    public Text wordToSay, wordThatUserSaid;
    public float positionY;
    private bool isActive;
    private string currentWordEnglish = "";
    private Vector2 startingPosition;

    // Use this for initialization
    IEnumerator Start()
    {
        WWW termsData = new WWW("http://10.171.204.188/ELLEMobile/TermData.php");
        yield return termsData;
        string termData = termsData.text;
        genre = PlayerPrefs.GetString("Language Pack");

        // Pause the music on this scene or else the mic will have
        // trouble picking up the words from the player
        if (PlayerPrefs.GetInt("Music Toggle") == 1)
            MusicPlayer.instance.PauseMusic();

        // This function parses the termData into the items string array
        ParseTerms(termData);

		// This class is created in "Level1WordBank.cs" its a custom storage class
		// to store the information needed regarding the language pack.
        languagePack = new LanguagePack(items, genre);
		languagePack.PrintLanguagePack();

		// These are in indexes in the languagePack.terms of the three generated questions
		GenerateIndexes();

        wordToSay.text = languagePack.terms[indexes[0]].ToString();
        currentWordEnglish = languagePack.englishTerms[indexes[0]].ToString();
        isActive = false;

        startingPosition = wordToSay.transform.position;

        int difficulty = PlayerPrefs.GetInt("Difficulty Setting");
        Debug.Log(difficulty);

        if (difficulty == 0)
        {
            InvokeRepeating("WordGravity", 4f, .50f);
        }
        else if (difficulty == 1)
        {
            InvokeRepeating("WordGravity", 4f, .35f);
        }
        else 
        {
            InvokeRepeating("WordGravity", 4f, .20f);
        }
    }

	private void Update()
	{
		// Locks the code below with a boolean to allow it to only be triggered once at a 
		// specified time interval to reduce looping
        if (!isActive)
        {
			// This if-elseif-statement catches if the user said the correct word 
			// if so, it will reset the word.
            if (currentWordEnglish.ToLower() == wordThatUserSaid.text.ToLower())
            {
                isActive = true;
                ResetWord();
            }
            // Hard coded voice answers
            else if (currentWordEnglish.ToLower() == "red" && wordThatUserSaid.text.ToLower() == "read")
            {
                wordThatUserSaid.text = "red";
                isActive = true;
                ResetWord();
            }
            else if (currentWordEnglish.ToLower() == "two" && wordThatUserSaid.text.ToLower() == "to")
            {
				isActive = true;
                ResetWord();
            }
            else if (currentWordEnglish.ToLower() == "two" && wordThatUserSaid.text.ToLower() == "too")
            {
				isActive = true;
                ResetWord();
            }
            else if (currentWordEnglish.ToLower() == "blue" && wordThatUserSaid.text.ToLower() == "lou")
            {
                wordThatUserSaid.text = "blue";
                isActive = true;
                ResetWord();
            }
        }
	}

	// This function generates indexes for the language pack class and allows the words
	// to be chosen from that pack randomly
	private void GenerateIndexes()
	{
		if (languagePack.GetSize() == 1)
		{
			indexes = new int[] { 0, 0, 0 };
		}
		else if (languagePack.GetSize() == 2)
		{
			indexes = new int[] { 0, 1, 0 };
			ShuffleList();
		}
		else if (languagePack.GetSize() == 3)
		{
			indexes = new int[] { 0, 1, 2 };
			ShuffleList();
		}
		else
		{
			indexes = languagePack.GenerateThreeTerms();
		}
	}

	// This function servers as to shuffle the indexes array
	private void ShuffleList()
	{
		System.Random r = new System.Random();
		int n = indexes.Length;
		while (n > 1)
		{
			n--;
			int k = r.Next(n + 1);
			int value = indexes[k];
			indexes[k] = indexes[n];
			indexes[n] = value;
		}
	}

	private void ParseTerms(string termData)
    {
        items = termData.Split(';');
    }

	// Allows the word the user must say to fall
    void WordGravity()
    {
        wordToSay.transform.position = new Vector2(wordToSay.transform.position.x, wordToSay.transform.position.y - 10);
        if (wordToSay.transform.position.y <= positionY)
        {
            GameOver();
        }
    }

	// If the user said the correct word, reset the position and generate a new word to say
    void ResetWord()
    {
		// Resets the position to the top
        wordToSay.transform.position = startingPosition;

		// Gets three indexes for the words
		GenerateIndexes();

        if (languagePack.GetSize() > 1)
        {
            if (languagePack.terms[indexes[0]].ToString() != wordToSay.text)
            {
                wordToSay.text = languagePack.terms[indexes[0]].ToString();
                currentWordEnglish = languagePack.englishTerms[indexes[0]].ToString();
            }
            else
            {
                wordToSay.text = languagePack.terms[indexes[1]].ToString();
                currentWordEnglish = languagePack.englishTerms[indexes[1]].ToString();
            }
        }
        else
        {
            wordThatUserSaid.text = "";
            wordToSay.text = languagePack.terms[indexes[0]].ToString();
            currentWordEnglish = languagePack.englishTerms[indexes[0]].ToString();
        }

        isActive = false;
        Debug.Log(languagePack.terms[indexes[0]] + " " + languagePack.englishTerms[indexes[0]]);
    }

    void GameOver()
    {
        // Resume the music on this scene if manually stopped by the 
        // script
        if (PlayerPrefs.GetInt("Music Toggle") == 1)
            MusicPlayer.instance.ResumeMusic();
        
        SceneManager.LoadScene("LoseScreen");
    }
}
