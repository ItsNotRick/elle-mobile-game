using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : MonoBehaviour
{

	void Start()
	{
	}

    public void PushStatsHelper()
    {
        StartCoroutine(PushStats());
    }

    IEnumerator PushStats()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>()
        {
            new MultipartFormFileSection("userID", PlayerPrefs.GetString("userid")),
            new MultipartFormFileSection("deck_ID", PlayerPrefs.GetInt("Language Pack ID").ToString()),
            new MultipartFormFileSection("correct", PlayerPrefs.GetInt("correct").ToString()),
            new MultipartFormFileSection("incorrect", PlayerPrefs.GetInt("incorrect").ToString()),
            new MultipartFormFileSection("score", ((int)PlayerPrefs.GetFloat("Player Score")).ToString()),
            new MultipartFormFileSection("platform", "2"),
        };

        string statsURL = "endlesslearner.com/insertstats";
        UnityWebRequest www = UnityWebRequest.Post(statsURL, formData);
        PlayerPrefs.SetInt("correct", 0);
        PlayerPrefs.SetInt("incorrect", 0);
        yield return www.SendWebRequest();
    }
}
