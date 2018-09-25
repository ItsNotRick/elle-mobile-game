using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARObjectController : MonoBehaviour
{
	private bool isFlashing;
	private Vector3 direction;
	private bool isHitting;

	public GameObject wordOne, wordTwo, wordThree;

	void Start()
	{
		isFlashing = isHitting = false;
		Random.seed = (int)System.DateTime.Now.Ticks;
		GenerateDirection();
	}

	// Update is called once per frame
	void Update()
	{
		Orbit();
	}

	void Orbit()
	{
		float speed = 15f;

		int difRating = PlayerPrefs.GetInt("Difficulty Setting");
		transform.RotateAround(Camera.main.transform.position, direction, (speed * (difRating + 1)) * Time.deltaTime);
	}

	// This function is called when the object is hit by the raycast
	void HitByRay()
	{
		// Uncomment line below to allow text images to flash colors
		// StartCoroutine(ObjectFlash());

		// When the object is in contact with the ray, turn it red
		gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
	}

	// This is called when the raycast moves off the text
	void NotHitByRay()
	{
		isHitting = false;

		// When the object is no longer in contact with the ray, turn its color back to
		// white
		gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
	}

	// This is called when the correct word is chosen
	void UpdatePositions()
	{
		GenerateDirection();
		DisableWords();
	}

	void GenerateDirection()
	{
		int val = Random.Range(0, 4);

		// Makes a number from 0 to 3, this will determine the direction the card will rotate
		if (val == 0)
		{
			direction = Vector3.up;
		}
		else if (val == 1)
		{
			direction = Vector3.down;
		}
		else if (val == 2)
		{
			direction = Vector3.left;
		}
		else
		{
			direction = Vector3.right;
		}
	}

	void DisableWords()
	{
		// Once the positions are updated set the objects to not be active.
		// This is done because the ARQuizMechanics.cs enables a panel overlay to display
		// whether a choice is right or wrong. And if the panel is enabled the 
		// player can still accidentally trigger the word again
		// The words will be re-enabled in the correct/incorrect coroutines in ArQuizMechanics.cs
		wordOne.GetComponent<TextMesh>().color = Color.white;
		wordTwo.GetComponent<TextMesh>().color = Color.white;
		wordThree.GetComponent<TextMesh>().color = Color.white;

		wordOne.SetActive(false);
		wordTwo.SetActive(false);
		wordThree.SetActive(false);
	}

	IEnumerator ObjectFlash()
	{
		isFlashing = true;

		gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		yield return new WaitForSeconds(.7f);
		gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
		yield return new WaitForSeconds(.7f);

		isFlashing = false;
	}
}
