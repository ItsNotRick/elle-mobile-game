using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSwipeController : MonoBehaviour 
{

	private GameObject player;
	private GameObject level0, level1, level2;
	private Vector3 startclick = Vector3.zero;
	private Vector3 endclick = Vector3.zero;
	private Vector3 deltaclick = Vector3.zero;
	private float level0_Y, level1_Y, level2_Y;
	private int counter = 0;

	public Text testText;
	private bool allowMotion, waitRunning;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
		level0 = GameObject.Find("Level0");
		level1 = GameObject.Find("Level1");
		level2 = GameObject.Find ("Level2");
		level0_Y = level0.transform.position.y + 1.0f;
		level1_Y = level1.transform.position.y + 1.0f;
		level2_Y = level2.transform.position.y + 1.0f;

		allowMotion = true;
		waitRunning = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float accel = Input.acceleration.x;
		//testText.text = accel.ToString();

		deltaclick = Vector3.zero;
		if (Input.GetMouseButtonDown (0)) 
		{
			startclick = Input.mousePosition;
		} 
		else if (Input.GetMouseButtonUp (0)) 
		{
			endclick = Input.mousePosition;
			deltaclick = endclick - startclick;
		}

		if (deltaclick.y > 0)
		{
			//swipe up
			SwipeUp();
		}
		else if (deltaclick.y < 0)
		{
			//swipe down
			SwipeDown();
		}
		// Tilting Left
		else if (PlayerPrefs.GetInt("Motion Toggle") == 1 && accel < -.5f)
		{
			if (allowMotion)
			{
				SwipeUp();
				allowMotion = false;
			}
			if (!waitRunning)
				StartCoroutine(Wait());
		}
		// Tilting Right
		else if (PlayerPrefs.GetInt("Motion Toggle") == 1 && accel > .5f)
		{
			if (allowMotion)
			{
				SwipeDown();
				allowMotion = false;
			}
			if (!waitRunning)
				StartCoroutine(Wait());
		}
	}

	void SwipeUp()
	{
		if (counter == 0)
		{
			player.transform.position = new Vector3(player.transform.position.x, level1_Y, 0);
			counter++;
		}
		else if (counter == 1)
		{
			player.transform.position = new Vector3(player.transform.position.x, level2_Y, 0);
			counter++;
		}
	}

	void SwipeDown()
	{
		if (counter == 1)
		{
			player.transform.position = new Vector3(player.transform.position.x, level0_Y, 0);
			counter--;
		}
		else if (counter == 2)
		{
			player.transform.position = new Vector3(player.transform.position.x, level1_Y, 0);
			counter--;

		}
	}

	IEnumerator Wait()
	{
		waitRunning = true;
		yield return new WaitForSeconds(1);
		allowMotion = true;
		waitRunning = false;
	}
}
