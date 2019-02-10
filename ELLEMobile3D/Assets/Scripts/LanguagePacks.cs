using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguagePacks : MonoBehaviour 
{
	private DeckInfo[] languages;
	public Text[] languageDisplay;
	public Text lpText;
	public Text testText;
	private uint selected;

    [SerializeField]
    SessionManager session;

	// Use this for initialization
	void Start()
	{
        languages = session.decks.ToArray();
		selected = 0;
		GenerateLpDisplay();
		testText.text = PlayerPrefs.GetString("Language Pack");
	}

	private void GenerateLpDisplay()
	{
        //TODO: CHECK IF LENGTH IS < 1
		lpText.text = languages[selected % (languages.Length)].name;
	}

	public void LeftButton()
	{
		selected += (uint) languages.Length - 1;
		GenerateLpDisplay();
	}

	public void RightButton()
	{
		selected++;
		GenerateLpDisplay();
	}

	public void SelectButton()
	{
		PlayerPrefs.SetString("Language Pack Name", languages[selected % (languages.Length)].name);
		testText.text = PlayerPrefs.GetString("Language Pack Name");
        PlayerPrefs.SetInt("Language Pack ID", languages[selected % (languages.Length)].id);
    }

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}


}
