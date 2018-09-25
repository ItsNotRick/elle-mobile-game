using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1WordBank : MonoBehaviour 
{
	private string[] items;
	private static LanguagePack languagePack;
    private string genre;
    private bool isReset;
    private int correctIndex; 

	public GameObject textQuestionOne, textQuestionTwo, textQuestionThree;
	public GameObject imageOne, imageTwo, imageThree;
    public GameObject player, resetFlag;
    public AudioSource correctSound, errorSound;
	public GameObject imageTest;
	public GameObject correctPanel, incorrectPanel;
	public GameObject hud;

	public Text WordToTranslate;
    public Text selectedWord;
    public Text lifeCounter;

    int[] indexes = {0, 0, 0};

	// Use this for initialization
	IEnumerator Start()
	{
		WWW termsData = new WWW("http://10.171.204.188/ELLEMobile/TermData.php");
		yield return termsData;
		string termData = termsData.text;
		genre = PlayerPrefs.GetString("Language Pack"); ;
		isReset = false;

		// This function parses the termData into the items string array
		ParseTerms(termData);
		languagePack = new LanguagePack(items, genre);
		languagePack.RemoveTermsWithNoImages();
		UpdateIndexes();
		GenerateWordDisplay();
	}

    public void Update()
    {
		// If the player reaches the reset flag, check to see if they chose the correct word, 
		// and reset them back to the start.
        if (player.transform.position.x >= resetFlag.transform.position.x - 5)
        {
            if (!isReset)
            {
                isReset = true;
                CheckWord();
                StartCoroutine(Wait());
            }
        }
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

	// Checks to see if the player chose the proper word 
    public void CheckWord()
    {
        // The user didn't choose a word
        if (selectedWord == null || selectedWord.text == "")
        {
            // Lose a life here
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1);

            // Update life counter on UI
            lifeCounter.text = "Lives: " + PlayerPrefs.GetInt("Lives");
            Debug.Log("Lives: " + PlayerPrefs.GetInt("Lives"));

            if (PlayerPrefs.GetInt("Sound Toggle") == 1)
            {
                Debug.Log("error sound!");
                errorSound.Play();
				correctPanel.SetActive(false);
				incorrectPanel.SetActive(true);
				hud.SetActive(false);
				StartCoroutine(DisablePanels());
			}
        }
        // The user chose the right word
        else if (selectedWord.text == languagePack.englishTerms[indexes[correctIndex]].ToString())
        {
            if (PlayerPrefs.GetInt("Sound Toggle") == 1)
            {
                Debug.Log("Correct sound!");
                correctSound.Play();
				correctPanel.SetActive(true);
				incorrectPanel.SetActive(false);
				hud.SetActive(false);
				StartCoroutine(DisablePanels());
			}
		}
        // The user chose the wrong word
        else
        {
            // Lose a life here
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1);

            // Update life counter on UI
            lifeCounter.text = "Lives: " + PlayerPrefs.GetInt("Lives");
            Debug.Log("Lives: " + PlayerPrefs.GetInt("Lives"));

            if (PlayerPrefs.GetInt("Sound Toggle") == 1)
            {
                Debug.Log("error sound!");
                errorSound.Play();
				correctPanel.SetActive(false);
				incorrectPanel.SetActive(true);
				hud.SetActive(false);
				StartCoroutine(DisablePanels());
			}
        }

        UpdateIndexes();
        GenerateWordDisplay();
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        isReset = false;
    }

	IEnumerator DisablePanels()
	{
		yield return new WaitForSeconds(1);
		correctPanel.SetActive(false);
		incorrectPanel.SetActive(false);
		hud.SetActive(true);
	}

	private void ParseTerms(string termData)
	{
		items = termData.Split(';');
	}

	// When this function is called, it should randomly generate three new words under the cards
	// the player can choose from.
	private void GenerateWordDisplay()
	{
		// Makes the text visible if it was hidden
		textQuestionOne.GetComponent<TextMesh>().color = new Color(0, 0, 0, 1);
		textQuestionTwo.GetComponent<TextMesh>().color = new Color(0, 0, 0, 1);
		textQuestionThree.GetComponent<TextMesh>().color = new Color(0, 0, 0, 1);

		// Sets the texts for the card display
		textQuestionOne.GetComponent<TextMesh>().text = languagePack.englishTerms[indexes[0]].ToString();
        textQuestionTwo.GetComponent<TextMesh>().text = languagePack.englishTerms[indexes[1]].ToString();
        textQuestionThree.GetComponent<TextMesh>().text = languagePack.englishTerms[indexes[2]].ToString();

		// Sets the images (if any present)  to the card display and removes the text
		string cardOneURL = languagePack.imagesForTerms[indexes[0]].ToString();
		if (cardOneURL != "")
		{
			StartCoroutine(GrabImage("http://10.171.204.188/ELLEMobile/Images/" + cardOneURL, imageOne));
			// Makes the text opaque if the is an image so it is "hidden"
			textQuestionOne.GetComponent<TextMesh>().color = new Color(0, 0, 0, 0);
		}

		string cardTwoURL = languagePack.imagesForTerms[indexes[1]].ToString();
		if (cardTwoURL != "")
		{
			StartCoroutine(GrabImage("http://10.171.204.188/ELLEMobile/Images/" + cardTwoURL, imageTwo));
			// Makes the text opaque if the is an image so it is "hidden"
			textQuestionTwo.GetComponent<TextMesh>().color = new Color(0, 0, 0, 0);
		}

		string cardThreeURL = languagePack.imagesForTerms[indexes[2]].ToString();
		if (cardOneURL != "")
		{
			StartCoroutine(GrabImage("http://10.171.204.188/ELLEMobile/Images/" + cardThreeURL, imageThree));
			// Makes the text opaque if the is an image so it is "hidden"
			textQuestionThree.GetComponent<TextMesh>().color = new Color(0, 0, 0, 0);
		}

		System.Random r = new System.Random();
        int correctWord = r.Next(0, 3);

        while (correctIndex == correctWord)
        {
            correctWord = r.Next(0, 3);
        }
        correctIndex = correctWord;
		WordToTranslate.text = languagePack.terms[indexes[correctWord]].ToString();
		selectedWord.text = "";
	}

	IEnumerator GrabImage(string url, GameObject cardImage)
	{
		WWW www = new WWW(url);
		yield return www;
		// The last two fields in the rect determine the sprites size
		cardImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(.5f, .5f), 100f);
		
		// Adjusts the scaling of the card
		cardImage.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
		cardImage.GetComponent<SpriteRenderer>().size = new Vector2(6, 6);

	}
}

// This class is utilized in both this script and the "VoiceQuizMechanics" script.
// This class stores the language pack pulled at the start of the scene.
public class LanguagePack
{
	public ArrayList terms;
	public ArrayList englishTerms;
	public ArrayList imagesForTerms;

	public LanguagePack(string[] items, string genre)
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

			if (imagesParsed[1].ToString() == "")
				Debug.Log(imagesParsed[1]);

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
			int i = r.Next(0, terms.Count - 1);
			if (hash.Add(terms[i].ToString()))
			{
				indexes[j++] = i;
			}
		}

		return indexes;
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
