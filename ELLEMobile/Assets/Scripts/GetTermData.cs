using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTermData : MonoBehaviour
{
	private string[] items;
	public Text[] wordDisplay;

	// Use this for initialization
	IEnumerator Start()
	{
		WWW termsData = new WWW("http://10.171.204.188/ELLEMobile/TermData.php");
		yield return termsData;
		string termData = termsData.text;
		ParseTerms(termData);
	}

	private void ParseTerms(string termData)
	{
		items = termData.Split(';');

		for (int i = 0; i < items.Length - 1; i++)
		{
			wordDisplay[i].text = items[i];
		}
	}
}