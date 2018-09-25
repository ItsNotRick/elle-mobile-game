using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour 
{
    public InputField usernameField, passwordField;
    public InputField usernameRegisterField, passwordRegisterField, passwordConfirmField;
    public Text registrationErrorText, registrationCompleteText, submissionText, submissionErrorText;

	private string createAccountURL = "http://10.171.204.188/ELLEMobile/CreateStudent.php";
    private string loginAccountURL = "http://10.171.204.188/ELLEMobile/LoginApp.php";

    private bool usernameTaken;
	private bool passwordSaved;
	// Use this for initialization
	void Start () 
    {
        usernameTaken = false;

		if (PlayerPrefs.GetString("Username").Length > 0)
		{
			usernameField.text = PlayerPrefs.GetString("Username");
			passwordField.text = PlayerPrefs.GetString("Password");
		}
	}
	
    public void OnLoginClick()
    {
        string username = usernameField.text;
		PlayerPrefs.SetString("Password", passwordField.text);
        string passwordHash = passwordField.text.GetHashCode().ToString();
        StartCoroutine(LoginAccount(username, passwordHash));
    }

	public void OnRegisterClick()
	{
		usernameField.text = "";
        passwordField.text = "";
	}

    public void OnSubmitClick()
    {
		// Check if username is too long or short
		if (usernameRegisterField.text.Length < 3 || usernameRegisterField.text.Length > 20)
		{
			registrationErrorText.text = "Username must be between 3-20 characters long";
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
			string passwordHash = passwordRegisterField.text.GetHashCode().ToString();
            StartCoroutine(RegisterAccount(usernameRegisterField.text, passwordHash));
		}

    }

	public void OnBackClick()
    {
        usernameRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordConfirmField.text = "";
        registrationErrorText.text = "";
        registrationCompleteText.text = "";
    }

	IEnumerator RegisterAccount(string username, string passwordHash)
	{
		WWWForm registerForm = new WWWForm();
		
		// Fields must be the same as they are in the PHP script on the server
		registerForm.AddField("usernamePost", username);
		registerForm.AddField("passwordPost", passwordHash);

		WWW www = new WWW(createAccountURL, registerForm);
		yield return www;

        if (www.text.ToString().Contains("User already exists"))
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

    IEnumerator LoginAccount(string username, string passwordHash)
    {
        WWWForm registerForm = new WWWForm();

        // Fields must be the same as they are in the PHP script on the server
        registerForm.AddField("usernamePost", username);
        registerForm.AddField("passwordPost", passwordHash);

        WWW www = new WWW(loginAccountURL, registerForm);
        yield return www;

        Debug.Log(www.text);

        if (www.text.Contains("Success"))
        {
            string[] terms = www.text.Split('|');
            PlayerPrefs.SetInt("UserID", int.Parse(terms[1].TrimEnd(';')));
            PlayerPrefs.SetString("Username", username);
			submissionText.text = terms[0] + " - logging in...";
            submissionErrorText.text = "";
            SceneManager.LoadScene("MainMenu");

        }
        else
        {
            submissionErrorText.text = www.text;
            submissionText.text = "";
        }
    }

}
