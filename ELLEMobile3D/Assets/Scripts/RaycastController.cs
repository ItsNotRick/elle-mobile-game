using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastController : MonoBehaviour 
{
	private bool mutex = false;

	public Text wordChosen;
	public GameObject wordOne, wordTwo, wordThree;
	private bool correctWordTrigger;
	private RaycastHit prevHit;

	void Start()
	{
		correctWordTrigger = false;	
	}

	// Update is called once per frame
	void Update () 
	{
		CreateRaycast();
	}

	private void CreateRaycast()
	{
		RaycastHit hit;
		Vector3 forward = transform.TransformDirection(Vector3.forward) * 600;
		Debug.DrawRay(Camera.main.transform.position, forward, Color.red);

		if (Physics.Raycast(Camera.main.transform.position, forward, out hit))
		{
			prevHit = hit;
			hit.transform.SendMessage("HitByRay");
			// Sets the "invisible" words to the word hit by raycast for comparison
			// in ARQuizMechanics.cs
			// Once this field is upated it will trigger a function in ARQuizMechanics.cs and compare
			if (!mutex)
				StartCoroutine(TriggerBoolUpdate());

			// 0 for disabled, 1 for enabled
			// this is used to disabled repeated correct/incorrect popups when one is displaying
			int quizMechanics = PlayerPrefs.GetInt("ARMechanics");

			if (quizMechanics == 1 && correctWordTrigger)
			{
				hit.transform.SendMessage("UpdatePositions");
				UpdateWordChosen(hit);
			}
		}
		// If the raycast is not hitting anything
		else
		{
			int quizMechanics = PlayerPrefs.GetInt("ARMechanics");

			try
			{
				if (quizMechanics == 1)
					prevHit.transform.SendMessage("NotHitByRay");
				correctWordTrigger = false;
	
				// This is done to counteract if two words pass over each other, and get stuck
				// being the color set when looking at a word
				wordOne.GetComponent<TextMesh>().color = Color.white;
				wordTwo.GetComponent<TextMesh>().color = Color.white;
				wordThree.GetComponent<TextMesh>().color = Color.white;
			}
			catch (System.Exception e)
			{
				// When the level starts prevHit will not be set to an instance of an object.
				// When it hits the first word, then it will be able to properly send the message 
				// above 
				; // Do nothing
			}
		}
	}

	private void UpdateWordChosen(RaycastHit hit)
	{
		wordChosen.text = hit.collider.gameObject.GetComponent<TextMesh>().text;
	}

	// After 2 seconds this will update and allow the word 
	// to be checked
	IEnumerator TriggerBoolUpdate()
	{
		mutex = true;
		yield return new WaitForSeconds(1.5f);
		correctWordTrigger = true;
		mutex = false;
	}
}
