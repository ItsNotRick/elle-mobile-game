using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFading : MonoBehaviour
{
	public Texture2D fadingTexture;
	public float fadeSpeed;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI()
	{
		alpha += fadeDir + fadeSpeed + Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

	 	GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadingTexture);
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	void OnLevelLoad()
	{
		BeginFade(-1);
	}
}
