using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public GameObject loadingScreen;
	public Slider slider;
	public Text progressText;

	public void LoadLevel(int sceneIndex)
	{
		// This means we will return to the main menu, possibly from the pause
		// screen, if that is the case we will need to set the game speed back
		// normal.
		if (sceneIndex == 0)
		{
			Time.timeScale = 1;
		}

		StartCoroutine(LoadAsynchronously(sceneIndex));
	}

	IEnumerator LoadAsynchronously (int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		loadingScreen.SetActive(true);
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);
			slider.value = progress;
			progressText.text = (int)(progress * 100) + "%";
			yield return null;
		}
	}
}
