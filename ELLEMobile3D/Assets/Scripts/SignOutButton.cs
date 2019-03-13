using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SignOutButton : MonoBehaviour
{
    public Button myButton;

    [SerializeField]
    SessionManager session;
    // Use this for initialization
    void Start()
    {
        myButton.onClick.AddListener(SignOut);
    }

    void SignOut()
    {
        session.access_token = "";
        session.id = "";
        PlayerPrefs.DeleteKey("session");
        //EditorUtility.SetDirty(session);
        SceneManager.LoadScene("Login");
    }

    void Update()
    {
    }

}
