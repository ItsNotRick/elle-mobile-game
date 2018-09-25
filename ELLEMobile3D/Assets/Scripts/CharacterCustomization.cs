using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCustomization : MonoBehaviour 
{
	public Renderer playerColor;

	private ColorStorage colorStorage;
	private int index;

	// Use this for initialization
	void Start () 
	{
		index = PlayerPrefs.GetInt("ColorIndex");

		playerColor.GetComponent<MeshRenderer>();
		colorStorage = new ColorStorage();

		// Color list found here: https://htmlcolorcodes.com/color-names/
		// in order of R-G-B
		colorStorage.AddColor(205, 92, 92);     // Indian Red
		colorStorage.AddColor(250, 128, 114);   // Salmon
		colorStorage.AddColor(220, 20, 60);     // Crimson
		colorStorage.AddColor(255, 0, 0);       // Red
		colorStorage.AddColor(139, 0, 0);       // Dark Red
		colorStorage.AddColor(255, 192, 203);   // Pink
		colorStorage.AddColor(255, 105, 180);   // Hot Pink
		colorStorage.AddColor(255, 127, 80);    // Coral
		colorStorage.AddColor(255, 69, 0);      // Orange Red
		colorStorage.AddColor(255, 165, 0);     // Orange
		colorStorage.AddColor(255, 255, 0);     // Yellow
		colorStorage.AddColor(230, 230, 250);   // Lavender
		colorStorage.AddColor(238, 130, 238);   // Violet
		colorStorage.AddColor(255, 0, 255);     // Magenta
		colorStorage.AddColor(147, 112, 219);   // Medium Purple
		colorStorage.AddColor(75, 0, 130);      // Indigo
		colorStorage.AddColor(106, 90, 205);    // Slate Blue
		colorStorage.AddColor(0, 255, 0);       // Lime
		colorStorage.AddColor(0, 255, 127);     // Spring Green
		colorStorage.AddColor(34, 139, 34);     // Forest Green
		colorStorage.AddColor(143, 188, 139);   // Dark Sea Green
		colorStorage.AddColor(0, 255, 255);     // Aqua
		colorStorage.AddColor(224, 255, 255);   // Light Cyan
		colorStorage.AddColor(0, 191, 255);     // Deep Sky Blue
		colorStorage.AddColor(65, 105, 225);    // Royal Blue
		colorStorage.AddColor(255, 255, 240);   // Ivory
		colorStorage.AddColor(192, 192, 192);   // Silver
		colorStorage.AddColor(245, 255, 250);	// Mint Cream
		
		SetColor();

		// This is here incase player loads into scene and doesn't change colors and
		// wants to keep the current one displayed (used in easter egg color)
		// Stores the RGB into player prefs to use in standard game
		PlayerPrefs.SetInt("ColorR", colorStorage.GetRAtIndex(index));
		PlayerPrefs.SetInt("ColorG", colorStorage.GetGAtIndex(index));
		PlayerPrefs.SetInt("ColorB", colorStorage.GetBAtIndex(index));
	}

	public void SetColor()
	{
		playerColor.material.SetColor
		(
			"_Color",
			new Color32
			(
				colorStorage.GetRAtIndex(index),
				colorStorage.GetGAtIndex(index),
				colorStorage.GetBAtIndex(index),
				100
			)
		);
	}

	// This is for manual insertion (mainly for the easter egg color)
	public void SetColor(byte r, byte g, byte b)
	{
		playerColor.material.SetColor
		(
			"_Color",
			new Color32
			(
				r,
				g,
				b,
				100
			)
		);
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void RightButton()
	{
		if (index < colorStorage.GetLength() - 1)
		{
			index++;
			SetColor();

			// Stores the RGB into player prefs to use in standard game
			PlayerPrefs.SetInt("ColorR", colorStorage.GetRAtIndex(index));
			PlayerPrefs.SetInt("ColorG", colorStorage.GetGAtIndex(index));
			PlayerPrefs.SetInt("ColorB", colorStorage.GetBAtIndex(index));
			PlayerPrefs.SetInt("ColorIndex", index);
		}
	}

	public void LeftButton()
	{
		if (index > 0)
		{
			index--;
			SetColor();

			// Stores the RGB into player prefs to use in standard game
			PlayerPrefs.SetInt("ColorR", colorStorage.GetRAtIndex(index));
			PlayerPrefs.SetInt("ColorG", colorStorage.GetGAtIndex(index));
			PlayerPrefs.SetInt("ColorB", colorStorage.GetBAtIndex(index));
			PlayerPrefs.SetInt("ColorIndex", index);

		}
	}

	// Just for fun 
	public void EasterEggButton()
	{
		Vector3 goldenRod = new Vector3(218, 165, 32);
		SetColor((byte)goldenRod.x, (byte)goldenRod.y, (byte)goldenRod.z);

		// Stores the RGB into player prefs to use in standard game
		PlayerPrefs.SetInt("ColorR", (byte)goldenRod.x);
		PlayerPrefs.SetInt("ColorG", (byte)goldenRod.y);
		PlayerPrefs.SetInt("ColorB", (byte)goldenRod.z);

		PlayerPrefs.SetInt("ColorIndex", 0);
	}
}

public class ColorStorage
{
	public ArrayList r;
	public ArrayList g;
	public ArrayList b;

	public ColorStorage()
	{
		this.r = new ArrayList();
		this.g = new ArrayList();
		this.b = new ArrayList();
	}

	public void AddColor(byte r, byte g, byte b)
	{
		this.r.Add(r);
		this.g.Add(g);
		this.b.Add(b);
	}

	public byte GetRAtIndex(int index)
	{
		return (byte)(this.r[index]);
	}

	public byte GetGAtIndex(int index)
	{
		return (byte)(this.g[index]);
	}

	public byte GetBAtIndex(int index)
	{
		return (byte)(this.b[index]);
	}

	public int GetLength()
	{
		return r.Count;
	}
}