using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Ionic;
using System;
using UnityEditor;

[CreateAssetMenu]
public class SessionManager : ScriptableObject
{
    [SerializeField]
    public string access_token = "";

    [SerializeField]
    public string id = "";

    [SerializeField]
    public string baseURL = "https://endlesslearner.com/";

    public List<DeckInfo> decks;

    // TODO: Redownload if hash doesn't match!
    public IEnumerator DownloadDecks()
    {
        List<DeckInfo> invalids = new List<DeckInfo>();
        foreach (DeckInfo d in this.decks)
        {
            string packPath = Application.persistentDataPath + @"/Packs/" + d.id;
            UnityWebRequest www = UnityWebRequest.Get(baseURL + @"deck/zip/" + d.id);
            www.SetRequestHeader(@"Authorization", @"Bearer " + this.access_token);
            yield return www.SendWebRequest();
            if (Directory.Exists(packPath)) Directory.Delete(packPath, true);
            Directory.CreateDirectory(packPath);
            using (BinaryWriter writer = new BinaryWriter(File.Open(packPath + ".zip", FileMode.Create)))
            {
                writer.Write(www.downloadHandler.data);
            }
            var fInfo = new FileInfo(packPath + ".zip");
            if (fInfo.Length < 50)
            {
                invalids.Add(d);
            }
            else
            {
                var opts = new Ionic.Zip.ReadOptions
                {
                    Encoding = System.Text.Encoding.GetEncoding(65001)
                };
                using (Ionic.Zip.ZipFile outZip = Ionic.Zip.ZipFile.Read(packPath + ".zip", opts))
                {
                    foreach (Ionic.Zip.ZipEntry e in outZip)
                    {
                        if (e.IsDirectory)
                        {
                            Directory.CreateDirectory(packPath + "/" + e.FileName);
                        }
                        else
                        {
                            using (FileStream stream = new FileStream(packPath + "/" + e.FileName, FileMode.Create))
                            {
                                e.Extract(stream);
                            }
                        }
                    }
                }
            }
            File.Delete(packPath + ".zip");
        }
        foreach (DeckInfo d in invalids)
        {
            this.decks.Remove(d);
        }
        foreach (var t in this.decks)
        {
            Debug.Log(t);
        }
        //EditorUtility.SetDirty(this);
    }
}

public class DecksJson
{
    public List<int> ids; //{ get; set; }
    public List<string> names; //{ get; set; }
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


