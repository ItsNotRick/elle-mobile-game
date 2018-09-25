using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerColor : MonoBehaviour 
{
	public Renderer playerColor;

	// Use this for initialization
	void Start () 
	{
		playerColor.GetComponent<MeshRenderer>();
		SetColor();
	}

	public void SetColor()
	{
		playerColor.material.SetColor
		(
			"_Color",
			new Color32
			(
				(byte)PlayerPrefs.GetInt("ColorR"),
				(byte)PlayerPrefs.GetInt("ColorG"),
				(byte)PlayerPrefs.GetInt("ColorB"),
				100
			));
	}
}
