using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour 
{
	public int timeLeft = 3;
	public Text countdownText;

	// Use this for initialization
	void Start()
	{
		StartCoroutine("LoseTime");
	}

	// Update is called once per frame
	void Update()
	{
		countdownText.text = timeLeft.ToString();

		if (timeLeft == 0)
		{
			countdownText.text = "GO!";
		}
		if (timeLeft <= -1)
		{
			StopCoroutine("LoseTime");
			gameObject.SetActive(false);
		}

	}

	IEnumerator LoseTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			timeLeft--;
		}
	}
}