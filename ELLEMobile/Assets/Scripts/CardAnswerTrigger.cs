using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardAnswerTrigger : MonoBehaviour
{
	// Note: This script is attached to the text displays under the balloons,
	// and this gameobject has a text mesh component to display the word.
	public Text cardText;
	public GameObject card;
    public AudioSource whooshSound;
    public GameObject balloons;
    public Vector2 originalPosition;
    private bool flyAway;

    public void Start()
    {
        flyAway = false;
        originalPosition = balloons.transform.position;
    }

    public void Update()
    {
        if (flyAway)
            balloons.transform.position = new Vector2(balloons.transform.position.x, balloons.transform.position.y + .2f); 
    }

    private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			cardText.text = gameObject.GetComponent<TextMesh>().text;

            if (PlayerPrefs.GetInt("Sound Toggle") == 1)
            {
                whooshSound.Play();
            }
            flyAway = true;
            StartCoroutine(Reset());
		}
	}

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        balloons.transform.position = originalPosition;
        flyAway = false;
    }
}
