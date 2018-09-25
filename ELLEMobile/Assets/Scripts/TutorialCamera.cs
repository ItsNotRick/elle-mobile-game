using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCamera : MonoBehaviour 
{
	public GameObject subwayDescription, parkDescription, darkCityDescription;
	public GameObject darkCityImg;

	public float subwayX, parkX, darkCityX;
	// Use this for initialization
	void Start () 
	{
		darkCityImg.SetActive(false);
	}

	public void RightButton()
	{
		// If camera is at subway, move to park 
		if (gameObject.transform.position.x == subwayX)
		{
			gameObject.transform.position = new Vector2(parkX, gameObject.transform.position.y);

			subwayDescription.SetActive(false);
			parkDescription.SetActive(true);
		}
		// If camera is at park, move to city 
		else if (gameObject.transform.position.x == parkX)
		{
			gameObject.transform.position = new Vector2(darkCityX, gameObject.transform.position.y);
			parkDescription.SetActive(false);
			darkCityDescription.SetActive(true);
			darkCityImg.SetActive(true);
		}
		// If camera is at city, do nothing
		else if (gameObject.transform.position.x == darkCityX)
		{
			; // Do Nothing
		}
	}

	public void LeftButton()
	{
		// If camera is at subway, don't move
		if (gameObject.transform.position.x == subwayX)
		{
			; // Do nothing!
		}
		// If camera is at park, move to subway
		else if (gameObject.transform.position.x == parkX)
		{
			gameObject.transform.position = new Vector2(subwayX, gameObject.transform.position.y);
			parkDescription.SetActive(false);
			subwayDescription.SetActive(true);
		}
		// If camera is at city, move to park
		else if (gameObject.transform.position.x == darkCityX)
		{
			gameObject.transform.position = new Vector2(parkX, gameObject.transform.position.y);
			darkCityDescription.SetActive(false);
			parkDescription.SetActive(true);
			darkCityImg.SetActive(false);
		}
	}
}
