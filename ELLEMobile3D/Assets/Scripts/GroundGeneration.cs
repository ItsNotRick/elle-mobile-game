using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundGeneration : MonoBehaviour {

	public GameObject Player;
	public GameObject Ground0;
	public GameObject Ground1;
	public GameObject Ground2;
	public GameObject Ground3;
	public GameObject Ground4;
	public GameObject LastGround;

	public GameObject Obstacle0;
	public GameObject Obstacle1;
	public GameObject Obstacle2;
	public GameObject Obstacle3;
	public GameObject Obstacle4;
	public GameObject Obstacle5;
	public GameObject quizChoiceText;

	private GameObject newGround;
	private GameObject obstacle;
	private GameObject choiceText;

	private int randomPlatform;
	private int randomObstacle;
	private int randomObstaclePosition;
	private int difficulty;
	private int platformCounter = 1;
	private float chanceOfObstacleSpawn;
	private float spawnObstacleChoice;
	public static bool spawnedSetOfPlatforms = false;
	public bool spawned = false;

	private Vector3 newPlayerPostion;
	private Vector3 startPlayerPostion;
	private Vector3 newGroundPosition;
	private Vector3 obstaclePosition;

	// Quiz Mechanics 
	private static LanguagePackInterface languagePack;
	int[] indexes = { 0, 0, 0 };

    [SerializeField]
    private SessionManager session;

	string[] fullWordOptions = new string[3];
	public List<string> selection = new List<string> ();
	public List<string> correct = new List<string> ();
	public List<string> incorrect = new List<string> ();
	public List<string> date = new List<string> ();
	public Text generatedWordDisplay;
	// Use this for initialization
	void Start () 
	{
		startPlayerPostion = Player.transform.position;
        Random.InitState((int)System.DateTime.Now.Ticks);
		difficulty = PlayerPrefs.GetInt ("Difficulty Setting");

		// This is used to trigger loading of platforms
		PlayerPrefs.SetInt("QuizImagesSpawned", 0);

		if (difficulty == 0) {
			chanceOfObstacleSpawn = 0.25f;
		} 
		else if (difficulty == 1) {
			chanceOfObstacleSpawn = 0.50f;
		} 
		else {
			chanceOfObstacleSpawn = 0.75f;
		}

        int id = PlayerPrefs.GetInt("Language Pack ID");
        foreach(DeckInfo d in session.decks)
        {
            if (d.id == id)
            {
                languagePack = new LanguagePackInterface(d);
                break;
            }
        }
        UpdateIndexes();
        //languagePack.RemoveTermsWithNoImages();
    }

	public void UpdateIndexes()
	{
		// These are in indexes in the languagePack.terms of the three generated questions
		if (languagePack.GetSize() == 1)
		{
			indexes = new int[] { 0, 0, 0 };
		}
		else if (languagePack.GetSize() == 2)
		{
			indexes = new int[] { 0, 1, 0 };
		}
		else if (languagePack.GetSize() == 3)
		{
			indexes = new int[] { 0, 1, 2 };
		}
		else
		{
			indexes = languagePack.GenerateThreeTerms();
		}
	}

	// Once per frame if the player completes a ground platform the game will
	//spawn a ground platform two platforms ahead.
	void Update () 
	{
		if (PlayerPrefs.GetInt("QuizImagesSpawned") == 0) 
		{
			PlayerPrefs.SetInt("QuizImagesSpawned", 1);

			// This loop determines how many platforms are spawned before the images are placed
			for (int i = 0; i < 6; i++) 
			{
				SpawnPlatform();
				SpawnObstacle();
			}

			// This loop determines how many "safe" platforms will spawn 
			// before the quiz cards
			for (int i = 0; i < 3; i++)
			{
				SpawnPlatform();
			}

			UpdateIndexes();
			SpawnQuizChoices(0, 0);
			SpawnQuizChoices(1, 1);
			SpawnQuizChoices(2, 2);

			// This loop spawns platforms after the quiz cards
			for (int i = 0; i < 5; i++)
			{
				SpawnPlatform();
			}

			// When a player chooses a card, the QuizImagesSpawned is updated to allow this if statement
			// to trigger again through a function in the PlayerMovementControls.cs
		}
	}
		
	void SpawnObstacle()
	{
		spawnObstacleChoice = Random.value;

		if (spawnObstacleChoice <= chanceOfObstacleSpawn) 
		{
			randomObstacle = (int)Random.Range (0.0f, 6.0f);
			randomObstaclePosition = (int)Random.Range (0.0f, 3.0f);

			switch (randomObstaclePosition) 
			{
				case 0:
					obstaclePosition = new Vector3 (-12f, 2.5f, newGroundPosition.z);
					break;
				case 1:
					obstaclePosition = new Vector3 (-5f, 2.5f, newGroundPosition.z);
					break;
				case 2:
					obstaclePosition = new Vector3 (3f, 2.5f, newGroundPosition.z);
					break;
				default:
					obstaclePosition = new Vector3 (-12f, 2.5f, newGroundPosition.z);
					break;
			}


			switch (randomObstacle) 
			{
				case 0:
					obstacle = (GameObject)Instantiate (Obstacle0, obstaclePosition, transform.rotation);
					break;
				case 1:
					obstacle = (GameObject)Instantiate (Obstacle1, obstaclePosition, transform.rotation);
					break;
				case 2:
					obstacle = (GameObject)Instantiate (Obstacle2, obstaclePosition, transform.rotation);
					break;
				case 3:
					obstacle = (GameObject)Instantiate (Obstacle3, obstaclePosition, transform.rotation);
					break;
				case 4:
					obstacle = (GameObject)Instantiate (Obstacle4, obstaclePosition, transform.rotation);
					break;
				case 5:
					obstacle = (GameObject)Instantiate (Obstacle5, obstaclePosition, transform.rotation);
					break;
				default:
					obstacle = (GameObject)Instantiate (Obstacle0, obstaclePosition, transform.rotation);
					break;
			}
		}
	}
	
	void SpawnPlatform()
	{
		newGroundPosition = new Vector3(LastGround.transform.position.x, LastGround.transform.position.y, LastGround.transform.position.z + 10f);
		randomPlatform = (int) Random.Range(0.0f, 5.0f);

		switch (randomPlatform) 
		{
			case 0:
				newGround = (GameObject) Instantiate (Ground0, newGroundPosition, transform.rotation);
				break;
			case 1:
				newGround = (GameObject) Instantiate (Ground1, newGroundPosition, transform.rotation);
				break;
			case 2:
				newGround = (GameObject) Instantiate (Ground2, newGroundPosition, transform.rotation);
				break;
			case 3:
				newGround = (GameObject) Instantiate (Ground3, newGroundPosition, transform.rotation);
				break;
			case 4:
				newGround = (GameObject) Instantiate (Ground4, newGroundPosition, transform.rotation);
				break;
			default:
				newGround = (GameObject) Instantiate (Ground0, newGroundPosition, transform.rotation);
				break;
		}
		LastGround = newGround;
	}

	void SpawnQuizChoices(int choiceLane, int choiceIndex)
	{

		switch (choiceLane)
		{
			case 0:
			obstaclePosition = new Vector3(-12f, 6f, newGroundPosition.z);
			break;
			case 1:
			obstaclePosition = new Vector3(-5f, 6f, newGroundPosition.z);
			break;
			case 2:
			obstaclePosition = new Vector3(2f, 6f, newGroundPosition.z);
			break;
			default:
			obstaclePosition = new Vector3(-17f, 2.5f, newGroundPosition.z);
			break;
		}

		choiceText = (GameObject)Instantiate(quizChoiceText, obstaclePosition, transform.rotation);
        //choiceText.GetComponent<TextMesh>().text = languagePack.englishTerms[indexes[choiceIndex]].ToString();
        Card c = languagePack.Cards[indexes[choiceIndex]];
        Rect rect = new Rect(0, 0, c.img.width, c.img.height);
        choiceText.GetComponent<SpriteRenderer>().sprite = Sprite.Create(c.img, rect, new Vector2(.5f, .5f), 100f);
        choiceText.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        choiceText.GetComponent<SpriteRenderer>().size = new Vector2(24, 6);

        choiceText.name = languagePack.Cards[indexes[choiceIndex]].destTerm.ToString();
		fullWordOptions [choiceIndex] = choiceText.name;
		if (choiceIndex == 2) {
			incorrect.Add(fullWordOptions[0] + "," + fullWordOptions[1] +","+fullWordOptions[2]+"," );
		}
		if (generatedWordDisplay.text == "")
		{
			generatedWordDisplay.text = languagePack.Cards[indexes[choiceIndex]].destTerm.ToString();
		}

		int chance = Random.Range(0, 2);
		if (chance == 1)
		{
			generatedWordDisplay.text = languagePack.Cards[indexes[choiceIndex]].destTerm.ToString();
		}
	}
}