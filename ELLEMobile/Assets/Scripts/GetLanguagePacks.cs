using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetLanguagePacks : MonoBehaviour {
	
	private string[] languages;
	public Text[] languageDisplay;
	public Text lpText;
	public Text testText;
	private int selected;

	// Use this for initialization
	IEnumerator Start () 
	{
		WWW languagepacks = new WWW ("http://10.171.204.188/ELLEMobile/LanguagePacks.php");
		yield return languagepacks;
		string lp = languagepacks.text;
		//ParseLps (lp);
		languages = lp.Split (';');
		selected = 0;
		GenerateLpDisplay ();
		testText.text = PlayerPrefs.GetString("Language Pack");
	}
	

	/*private void ParseLps(string lp)*	
 * {
		languages = lp.Split (';');
		for (int i = 0; i < languages.Length - 1; i++) {
			languageDisplay [i].text = languages [i];
		}
	}*/

	private void GenerateLpDisplay()
	{
		if (selected < 0) 
		{
			selected = languages.Length - 2;
		}

		lpText.text = languages[selected % (languages.Length-1)];
	}

	public void leftButton()
	{
		selected--;
		GenerateLpDisplay ();
	}

	public void rightButton()
	{
		selected++;
		GenerateLpDisplay ();
	}

	public void selectButton()
	{
		PlayerPrefs.SetString ("Language Pack", languages[selected % (languages.Length-1)]);
		testText.text =  PlayerPrefs.GetString("Language Pack")  ;
	}
}
