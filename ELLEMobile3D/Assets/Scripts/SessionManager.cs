using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;
using System;


// This class is utilized in both this script and the "VoiceQuizMechanics" script.
// This class stores the language pack pulled at the start of the scene.
public class LanguagePacksContainer
{
    public ArrayList terms;
    public ArrayList englishTerms;
    public ArrayList imagesForTerms;

    public LanguagePacksContainer(string[] items, string name)
    {
        terms = new ArrayList();
        englishTerms = new ArrayList();
        imagesForTerms = new ArrayList();

        string[] parsed, termsParsed, englishParsed, imagesParsed;
        for (int i = 0; i < items.Length/* - 1*/; i++)
        {
            parsed = items[i].Split('|');
            termsParsed = parsed[0].Split('&');
            englishParsed = parsed[1].Split('&');
            imagesParsed = parsed[5].Split('&');

            // Adds the fields to the corresponding arraylists
            if (parsed[2].Contains(name))
            {
                terms.Add(termsParsed[1]);
                englishTerms.Add(englishParsed[1]);
                imagesForTerms.Add(imagesParsed[1]);
            }
        }
    }

    public void PrintLanguagePack()
    {
        for (int i = 0; i < terms.Count; i++)
        {
            Debug.Log(terms[i] + " : " + englishTerms[i] + " : " + imagesForTerms[i]);
        }
    }

    public int[] GenerateThreeTerms()
    {
        HashSet<string> hash = new HashSet<string>();
        int[] indexes = new int[3];

        int j = 0;
        while (hash.Count < 3)
        {
            System.Random r = new System.Random();
            int i = r.Next(0, terms.Count - 2);
            if (hash.Add(terms[i].ToString()))
            {
                indexes[j++] = i;
            }
        }

        return indexes;
    }

    public string[] GenerateSingleTerm()
    {
        HashSet<string> hash = new HashSet<string>();
        string[] wordGenerated = new string[2];

        System.Random r = new System.Random();
        int i = r.Next(0, terms.Count - 2);

        wordGenerated[0] = terms[i].ToString();
        wordGenerated[1] = englishTerms[i].ToString();

        return wordGenerated;
    }

    public int GetSize()
    {
        return terms.Count;
    }

    public void RemoveTermsWithNoImages()
    {
        // Note with each removal the terms.Count changes so we must adjust i accordingly
        for (int i = 0; i < terms.Count; i++)
        {
            if (imagesForTerms[i].ToString() == "")
            {
                terms.RemoveAt(i);
                englishTerms.RemoveAt(i);
                imagesForTerms.RemoveAt(i);
                i--;
            }
        }
    }
}

[CreateAssetMenu]
public class SessionManager : ScriptableObject
{
    [SerializeField]
    public string access_token = "";

    [SerializeField]
    public string id = "";

    //public Dictionary<Tuple<int, string>, string> deckPaths;

    public List<DeckInfo> decks;

}


public class DecksJson
{
    public List<int> ids { get; set; }
    public List<string> names { get; set; }
}

[Serializable]
public class DeckInfo
{
    public DeckInfo(int id_, string name_)
    {
        id = id_;
        name = name_;
    }
    public int id;
    public string name;
}
