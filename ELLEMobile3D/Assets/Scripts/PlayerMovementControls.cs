using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerMovementControls : MonoBehaviour {

    public GameObject Player;
    private Vector3 startclick = Vector3.zero;
    private Vector3 endclick = Vector3.zero;
    private Vector3 deltaclick = Vector3.zero;
    private int swipeCounter = 0;
    private int controlScheme = 0;  // 0 off 1 on
    private bool choicePanelDisplayed;
    private int score;
    private double scoredbl;
    private int lives;
    private GroundGeneration x1;

    public Text wordToTranslateText, scoreText, livesText;
    public GameObject correctPanel, incorrectPanel;
    public GameObject cameraControl;
    int totalCorrect, totalIncorrect, total;
    float playedTime;
    string correctWords, incorrectWords;


    [SerializeField]
    SessionManager session;

    private static string statsURL = "insertstats";

    //Old stats variables **Deprecated**
    bool mutexAnalytics;
    private string dataURL = "http://10.171.204.188/ELLEMobile/PushAnalyticsHS.php";
    private string dataURL2 = "http://10.171.204.188/ELLEMobile/PushResponses.php";
    
    // Use this for initialization
    void Start () 
    {
        print(PlayerPrefs.GetInt("UserID"));
        GameObject groundgen = GameObject.Find ("Main Camera");
        x1 = groundgen.GetComponent<GroundGeneration> ();
        scoredbl = 0;
        score = 0;
        lives = 3;
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
        totalCorrect = totalIncorrect = 0;
        playedTime = 0;
        correctWords =  "";
        incorrectWords =  "";
        mutexAnalytics = false;
        controlScheme = PlayerPrefs.GetInt ("Motion Toggle");
        choicePanelDisplayed = false;
        UpdateMovement ();
    }
    
    // Update is called once per frame
    void UpdateMovement () 
    {
        if (controlScheme == 1)
        {
            InvokeRepeating("MotionController", 0, 0.05f);
        }
    }

    void Update()
    {
        scoredbl += (0.1 * Time.deltaTime);
        //Debug.Log (scoredbl);
        if ((int)scoredbl > score) {
            score = (int)scoredbl;
            scoreText.text = score.ToString();

        }
        playedTime += Time.deltaTime;
        if (controlScheme == 0)
        {
            SwipeController();
        }

    }
    //new MultipartFormDataSection("GameTime", playedTime.ToString()),

    IEnumerator PushStats()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {

            new MultipartFormDataSection("userID", session.id),
            new MultipartFormDataSection("deck_ID", PlayerPrefs.GetInt("Language Pack ID").ToString()),
            new MultipartFormDataSection("correct", totalCorrect.ToString()),
            new MultipartFormDataSection("incorrect", totalIncorrect.ToString()),
            new MultipartFormDataSection("score", score.ToString()),
            new MultipartFormDataSection("platform", "3"),
        };
        UnityWebRequest www = UnityWebRequest.Post(session.baseURL + statsURL, formData);
        yield return www.SendWebRequest();
        long responseCode = www.responseCode;
    }

    IEnumerator PushAnalyticsHelper()
    {
        
        mutexAnalytics = true;

        WWWForm dataForm = new WWWForm();

        // Fields to add to the table
        // dataForm.AddField("scorePost", scoreDisplay.text);
        dataForm.AddField("userIDPost", PlayerPrefs.GetInt("UserID"));
        dataForm.AddField("scorePost", score.ToString());
        dataForm.AddField("iWordsPost", correctWords);
        dataForm.AddField("iGenresPost", incorrectWords);
        dataForm.AddField("totalCorrectPost", totalCorrect);
        dataForm.AddField("totalIncorrectPost", totalIncorrect);
        dataForm.AddField("minsPlayedPost", (playedTime / 60).ToString());

        WWW www = new WWW(dataURL, dataForm);
        yield return www;

        Debug.Log(www.text.ToString());
        mutexAnalytics = false;
    }
    IEnumerator PushResponsesHelper()
    {
        mutexAnalytics = true;

        WWWForm dataForm = new WWWForm();

        // Fields to add to the table
        // dataForm.AddField("scorePost", scoreDisplay.text);

        dataForm.AddField("userIDPost", PlayerPrefs.GetInt("UserID"));
        int y = 1;
        while (total > 0 && y <= 10) {
            dataForm.AddField("selection"+y+"Post", x1.selection[0]);
            dataForm.AddField("correct"+y+"Post", x1.correct[0]);
            dataForm.AddField("incorrect"+y+"Post", x1.incorrect[0]);
            dataForm.AddField("date"+y+"Post", System.DateTime.Now.ToString("MM/dd/yyyy"));
            x1.selection.RemoveAt(0);
            x1.correct.RemoveAt(0);
            x1.incorrect.RemoveAt(0);
            total--;
            y++;

        }
        WWW www = new WWW(dataURL2, dataForm);
        yield return www;
        Debug.Log(www.text.ToString());
        mutexAnalytics = false;
    }
    public void PushAnalytics()
    {
            StartCoroutine(PushAnalyticsHelper());
    }

    public void PushResponses()
    {
            StartCoroutine(PushResponsesHelper());
    }

    void MotionController()
    {
        Debug.Log (Player.transform.position.x);
        Debug.Log (Input.acceleration.x);
        if ((Player.transform.position.x > -11) && (Player.transform.position.x < 5))
            transform.Translate (Input.acceleration.x * 2, 0, 0);
        else if((Player.transform.position.x >= 5))
            Player.transform.position = new Vector3(4.9f, Player.transform.position.y, Player.transform.position.z);
        else if ((Player.transform.position.x <= -11))
            Player.transform.position = new Vector3(-10.9f, Player.transform.position.y, Player.transform.position.z);
    }

    void SwipeController()
    {
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

        if (deltaclick.x < 0)
        {
            //swipe left
            MoveLeft();
        }
        else if (deltaclick.x > 0)
        {
            //swipe right
            MoveRight();
        }
    }

    void MoveLeft()
    {
        if (swipeCounter == 0 || swipeCounter == 1)
        {
            Player.transform.position = new Vector3(Player.transform.position.x - 8, Player.transform.position.y, Player.transform.position.z);
            swipeCounter--;
        }	
    }

    void MoveRight()
    {
        if (swipeCounter == 0 || swipeCounter == -1)
        {
            Player.transform.position = new Vector3(Player.transform.position.x + 8, Player.transform.position.y, Player.transform.position.z);
            swipeCounter++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Debug.Log("Hit Obstacle!");
            // Deduct a life here
            lives--;
            livesText.text = lives.ToString();


            if (lives <= 0)
            {
                //PushAnalytics ();
                total = totalCorrect + totalIncorrect;
                StartCoroutine(PushStats());
                //PushResponses ();
                GameOver();
            }
        }
        else if (other.name == wordToTranslateText.text)
        {
            // Correct Decision
            Debug.Log("Correct");
            score += 5;
            scoredbl += 5;
            totalCorrect++;
            correctWords = correctWords + wordToTranslateText.text;
            scoreText.text = score.ToString();
            x1.selection.Add ("correct");
            x1.correct.Add (wordToTranslateText.text);
            PlayerPrefs.SetInt("QuizImagesSpawned", 0);

            if (!choicePanelDisplayed)
            {
                StartCoroutine(DisplayCorrectPanel());
            }
        }
        else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("QuizImagesSpawned", 0);
            totalIncorrect++;
            x1.selection.Add ("incorrect");
            x1.correct.Add (wordToTranslateText.text);
            incorrectWords = incorrectWords + wordToTranslateText.text;
            if (!choicePanelDisplayed)
            {
                StartCoroutine(DisplayIncorrectPanel());
            }
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator DisplayCorrectPanel()
    {
        correctPanel.SetActive(true);
        StartCoroutine(DisableMovement());

        yield return new WaitForSeconds(2f);

        correctPanel.SetActive(false);
        cameraControl.GetComponent<PlayerMovementAndCameraFollow>().moveSpeed = 20;
    }

    IEnumerator DisplayIncorrectPanel()
    {
        incorrectPanel.SetActive(true);
        StartCoroutine(DisableMovement());

        yield return new WaitForSeconds(2f);

        incorrectPanel.SetActive(false);
        cameraControl.GetComponent<PlayerMovementAndCameraFollow>().moveSpeed = 20;
    }

    IEnumerator DisableMovement()
    {
        yield return new WaitForSeconds(.5f);
        cameraControl.GetComponent<PlayerMovementAndCameraFollow>().moveSpeed = 0;
    }
}
