using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetLanguagePacks : MonoBehaviour {
	
	private DeckInfo[] languages;
	public Text[] languageDisplay;
	public Text lpText;
	public Text currentPackText;
	private uint selected;

	[SerializeField]
	SessionManager session;

	// Use this for initialization
	void Start () 
	{
		languages = session.decks.ToArray();
		selected = 0;
		GenerateLpDisplay ();
		currentPackText.text = PlayerPrefs.GetString("Language Pack Name");
	}
	

	private void GenerateLpDisplay()
	{
		lpText.text = languages[selected % (languages.Length)].name;
	}

	public void leftButton()
	{
		selected += (uint) languages.Length - 1;
		GenerateLpDisplay ();
	}

	public void rightButton()
	{
		selected++;
		GenerateLpDisplay ();
	}

	public void selectButton()
	{
		PlayerPrefs.SetString ("Language Pack Name", languages[selected % (languages.Length)].name);
		currentPackText.text =  PlayerPrefs.GetString("Language Pack Name");
		PlayerPrefs.SetInt("Language Pack ID", languages[selected % (languages.Length)].id);
	}
}
