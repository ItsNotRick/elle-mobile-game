using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Linq;


public class Account
{
    public string id; //{ get; set; }
    public string access_token; //{ get; set; }
    public string permissions; //{ get; set; }
}


public class LoginManager : MonoBehaviour 
{
    public InputField usernameField, passwordField;
    public InputField usernameRegisterField, passwordRegisterField, passwordConfirmField;
    public Text registrationErrorText, registrationCompleteText, submissionText, submissionErrorText;

    [SerializeField]
    private SessionManager session;

    private static string baseURL = "https://endlesslearner.com/";
	  private static string createAccountURL = baseURL + "register";
    private static string loginAccountURL = baseURL + "login";

    private bool usernameTaken;
	private bool passwordSaved;
	// Use this for initialization
	void Start ()
    {
        usernameTaken = false;
        string potentialSession = PlayerPrefs.GetString("session");
        if (session == null)
        {
            session = ScriptableObject.CreateInstance<SessionManager>();
        }
        JsonUtility.FromJsonOverwrite(potentialSession, session);

        //TODO Actually test connection here!
        if (session.access_token.Length > 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
	
    public void OnLoginClick()
    {
        string username = usernameField.text;
		string password = passwordField.text;

		StartCoroutine(LoginAccount(username, password));
    }

	public void OnRegisterClick()
	{
		usernameField.text = "";
        passwordField.text = "";
		submissionErrorText.text = "";
		submissionText.text = "";
	}

    public void OnSubmitClick()
    {
		// Check if username is too long or short
		if (usernameRegisterField.text.Length < 2 || usernameRegisterField.text.Length > 8)
		{
			registrationErrorText.text = "Username must be between 2-8 characters long";
		}
		// Ensures the username is only letters and numbers
		else if (!Regex.IsMatch(usernameRegisterField.text, @"^[a-zA-Z0-9]+$"))
		{
			registrationErrorText.text = "Username can only contain letters and numbers";
		}
		else if (passwordRegisterField.text.Length < 4 || passwordRegisterField.text.Length > 20)
		{
			registrationErrorText.text = "Password must be between 4-20 characters long";
		}
		// Check if passwords match
		else if (passwordRegisterField.text != passwordConfirmField.text)
		{
			registrationErrorText.text = "Passwords do not match";
		}
		// Account registration information is valid
		else
		{
			string password = passwordRegisterField.text;

			StartCoroutine(RegisterAccount(usernameRegisterField.text, password));
		}
    }

    public void OnBackClick()
    {
        usernameRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordConfirmField.text = "";
        registrationErrorText.text = "";
        registrationCompleteText.text = "";
        usernameRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordConfirmField.text = "";
        submissionErrorText.text = "";
        submissionText.text = "";
    }

    IEnumerator RegisterAccount(string username, string passwordHash)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            // Fields must be the same as they are in the Python script on the server
            new MultipartFormDataSection("username", "ii"),
            new MultipartFormDataSection("password", "ii"),
        };

        UnityWebRequest www = UnityWebRequest.Post(createAccountURL, formData);
		yield return www;

        if (www.downloadHandler.text.ToString().Contains("User already exists"))
        {
            registrationErrorText.text = "Username already exists";
            registrationCompleteText.text = "";
            usernameTaken = true;
        }
        else
        {
            registrationErrorText.text = "";
            registrationCompleteText.text = "Registration Complete!";
        }

        // Resets the password fields
        passwordRegisterField.text = "";
        passwordConfirmField.text = "";
	}

    IEnumerator LoginAccount(string username, string password)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            // Fields must be the same as they are in the Python script on the server
            new MultipartFormDataSection("username", username),
            new MultipartFormDataSection("password", password),
        };

        UnityWebRequest www = UnityWebRequest.Post(loginAccountURL, formData);
        yield return www.SendWebRequest();

        string dat = www.downloadHandler.text;

        if (!dat.Contains("Invalid credentials!") && dat != null)
        {
            Account user = JsonUtility.FromJson<Account>(dat);
            submissionText.text = user.id + " - logging in...";
            submissionErrorText.text = "";
            session.access_token = user.access_token;
            session.id = user.id;
            yield return StartCoroutine(GetDeckNames());
            //EditorUtility.SetDirty(session);
            PlayerPrefs.SetString("session", JsonUtility.ToJson(session));
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            if (dat == null)
            {
                submissionErrorText.text = "Unable to connect to server.";
            }
            else {
                submissionErrorText.text = dat;
            }
            submissionText.text = "";
        }
    }




    IEnumerator GetDeckNames()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseURL + "decks");
        www.SetRequestHeader("Authorization", "Bearer " + session.access_token);
        yield return www.SendWebRequest();

        string decks = www.downloadHandler.text;

        if (!decks.Contains("Invalid credentials!"))
        {
            DecksJson deckLists = JsonUtility.FromJson<DecksJson>(decks);
            session.decks = deckLists.ids.Zip(deckLists.names, (a, b) => new DeckInfo(a, b)).ToList();
            //Debug.Log(decks);
            //EditorUtility.SetDirty(session);
            yield return StartCoroutine(session.DownloadDecks());
        }
    }
}
