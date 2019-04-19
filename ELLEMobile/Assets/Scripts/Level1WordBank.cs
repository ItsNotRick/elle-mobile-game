using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1WordBank : MonoBehaviour 
{
	private string[] items;
	private static LanguagePackInterface languagePack;
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

	[SerializeField]
	private SessionManager session;

	// Use this for initialization
	void Start()
	{
		isReset = false;

		// This function parses the termData into the items string array
		int id = PlayerPrefs.GetInt("Language Pack ID");
		foreach(DeckInfo d in session.decks)
		{
			if (d.id == id)
			{
				languagePack = new LanguagePackInterface(d);
				break;
			}
		}
		//languagePack.RemoveTermsWithNoImages();
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
		Debug.Log(selectedWord.text);
		Debug.Log(languagePack.Cards[indexes[correctIndex]].sourceTerm);
        // The user didn't choose a word
        if (selectedWord == null || selectedWord.text == "")
        {
            // Lose a life here
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1);
            PlayerPrefs.SetInt("incorrect", PlayerPrefs.GetInt("incorrect") + 1);

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
        else if (selectedWord.text == languagePack.Cards[indexes[correctIndex]].sourceTerm)
        {
            PlayerPrefs.SetInt("correct", PlayerPrefs.GetInt("correct") + 1);
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
            PlayerPrefs.SetInt("incorrect", PlayerPrefs.GetInt("incorrect") + 1);
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

	// When this function is called, it should randomly generate three new words under the cards
	// the player can choose from.
	private void GenerateWordDisplay()
	{
		// Makes the text visible if it was hidden
		textQuestionOne.GetComponent<TextMesh>().color = new Color(0, 0, 0, 1);
		textQuestionTwo.GetComponent<TextMesh>().color = new Color(0, 0, 0, 1);
		textQuestionThree.GetComponent<TextMesh>().color = new Color(0, 0, 0, 1);

		// Sets the texts for the card display
		textQuestionOne.GetComponent<TextMesh>().text = languagePack.Cards[indexes[0]].sourceTerm;
        textQuestionTwo.GetComponent<TextMesh>().text = languagePack.Cards[indexes[1]].sourceTerm;
        textQuestionThree.GetComponent<TextMesh>().text = languagePack.Cards[indexes[2]].sourceTerm;

		/*cardImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(.5f, .5f), 100f);
		// Adjusts the scaling of the card
		cardImage.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
		cardImage.GetComponent<SpriteRenderer>().size = new Vector2(6, 6);*/

		Card c1 = languagePack.Cards[indexes[0]];
		Rect rect1 = new Rect(0, 0, c1.img.width, c1.img.height);
		imageOne.GetComponent<SpriteRenderer>().sprite = Sprite.Create(c1.img, rect1, new Vector2(.5f, .5f), 100f);
		// Makes the text opaque if the is an image so it is "hidden"
		textQuestionOne.GetComponent<TextMesh>().color = new Color(0, 0, 0, 0);

		Card c2 = languagePack.Cards[indexes[1]];
		Rect rect2 = new Rect(0, 0, c2.img.width, c2.img.height);
		imageTwo.GetComponent<SpriteRenderer>().sprite = Sprite.Create(c2.img, rect2, new Vector2(.5f, .5f), 100f);
		// Makes the text opaque if the is an image so it is "hidden"
		textQuestionTwo.GetComponent<TextMesh>().color = new Color(0, 0, 0, 0);

		Card c3 = languagePack.Cards[indexes[2]];
		Rect rect3 = new Rect(0, 0, c3.img.width, c3.img.height);
		imageThree.GetComponent<SpriteRenderer>().sprite = Sprite.Create(c3.img, rect3, new Vector2(.5f, .5f), 100f);
		// Makes the text opaque if the is an image so it is "hidden"
		textQuestionThree.GetComponent<TextMesh>().color = new Color(0, 0, 0, 0);

		System.Random r = new System.Random();
        int correctWord = r.Next(0, 3);

        while (correctIndex == correctWord)
        {
            correctWord = r.Next(0, 3);
        }
        correctIndex = correctWord;
		WordToTranslate.text = languagePack.Cards[indexes[correctWord]].destTerm;
		selectedWord.text = "";
	}
}
