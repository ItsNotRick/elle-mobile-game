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

    // There currently aren't any password restrictions built into the web service, the error checking should happen
    // on the server and the message should get sent back and displayed. This way a change to the service will
    // immediately be in effect for all users of the API.
    public void OnSubmitClick()
    {
		// Ensures the username is only letters and numbers
		if (!Regex.IsMatch(usernameRegisterField.text, @"^[a-zA-Z0-9]+$"))
		{
			registrationErrorText.text = "Username can only contain letters and numbers";
		}
		else if (passwordRegisterField.text.Length < 4)// || passwordRegisterField.text.Length > 20)
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

			StartCoroutine(RegisterAccount(usernameRegisterField.text, passwordRegisterField.text));//password));
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

    class RegMsg {
        public string message;
    }

   IEnumerator RegisterAccount(string username, string password)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            // Fields must be the same as they are in the Python script on the server
            new MultipartFormDataSection("username", username),
            new MultipartFormDataSection("password", password),
        };

        UnityWebRequest www = UnityWebRequest.Post(createAccountURL, formData);
		yield return www.SendWebRequest();
        long responseCode = www.responseCode;
        RegMsg regResult = JsonUtility.FromJson<RegMsg>(www.downloadHandler.text);

        if (responseCode == 400)
        {
            registrationErrorText.text = regResult.message;
            registrationCompleteText.text = "";
            usernameTaken = true;
        }
        else if (responseCode == 201)
        {
            registrationErrorText.text = "";
            registrationCompleteText.text = regResult.message;
        }
        else
        {
            registrationErrorText.text = "Unkown Error Occurred";
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
            PlayerPrefs.SetString("userid", user.id);
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
