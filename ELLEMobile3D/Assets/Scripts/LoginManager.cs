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
		string passwordHash = MD5(passwordField.text.GetHashCode().ToString());

		// This is done because the database only accepts passwords up to 35 characters
		if (passwordHash.Length > 35)
			passwordHash = passwordHash.Substring(0, 35);

		StartCoroutine(LoginAccount(username, passwordHash));
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
			string passwordHash = MD5(passwordRegisterField.text.GetHashCode().ToString());

			// This is done because the database only accepts passwords up to 35 characters
			if (passwordHash.Length > 35)
				passwordHash = passwordHash.Substring(0, 35);

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
		usernameRegisterField.text = "";
		passwordRegisterField.text = "";
		passwordConfirmField.text = "";
		submissionErrorText.text = "";
		submissionText.text = "";		
	}

	private string MD5(string pass)
	{
		System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
		bs = x.ComputeHash(bs);
		System.Text.StringBuilder s = new System.Text.StringBuilder();
		foreach (byte b in bs)
		{
			s.Append(b.ToString("x2").ToLower());
		}
		return s.ToString();
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
