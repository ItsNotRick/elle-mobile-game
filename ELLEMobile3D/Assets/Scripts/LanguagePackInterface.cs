using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class Card
{
    public string id { get; set; }
    public string destTerm { get; set; }
    public string sourceTerm { get; set; }
    public Texture2D img;
}

public class CSVRow
{
    public string id { get; set; }
    public string English { get; set; }
    public string Translation { get; set; }
}

public class LanguagePackInterface
{
    public string packPath;
    public List<Card> Cards;

    public LanguagePackInterface(DeckInfo deck)
    {
        Cards = new List<Card>();
        packPath = "Assets/LanguagePacks/" + deck.id + "/";
        string csvPath = packPath + "Deck_" + deck.id + ".csv";
        string imgBase = packPath + "Images/";
        string[] images = Directory.EnumerateFiles(imgBase, "*.*")
            .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();

        using (var reader = new StreamReader(csvPath))
        using (var csv = new CsvReader(reader))
        {
            var records = csv.GetRecords<CSVRow>();
            foreach (CSVRow r in records)
            {
                Cards.Add(new Card
                {
                    id = r.id,
                    destTerm = r.Translation,
                    sourceTerm = r.English,
                    img = new Texture2D(2, 2)
                });
                string exp = @"[\s\S]*(" + r.id + @")[\s\S]*(.jpg|.png)";
                Regex reg = new Regex(exp);
                string imgPath = images.Where(path => reg.IsMatch(path)).First();
                byte[] fileBytes = File.ReadAllBytes(imgPath);
                Cards.Last().img.LoadImage(fileBytes);
            }
        }
    }

    public void PrintLanguagePack()
    {
        foreach(Card c in Cards)
        {
            Debug.Log(c.destTerm + " : " + c.sourceTerm + " : " + " ");
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
            int i = r.Next(0, Cards.Count - 2);
            if (hash.Add(Cards[i].destTerm.ToString()))
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
        int i = r.Next(0, Cards.Count - 2);

        wordGenerated[0] = Cards[i].destTerm.ToString();
        wordGenerated[1] = Cards[i].sourceTerm.ToString();

        return wordGenerated;
    }

    public int GetSize()
    {
        return Cards.Count;
    }

/*    public void RemoveTermsWithNoImages()
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
    }*/
}