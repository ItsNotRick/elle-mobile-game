using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SessionManager : ScriptableObject
{
    [SerializeField]
    public string access_token = "";

    [SerializeField]
    public string id = "";

    [SerializeField]
    public Dictionary<string, string> deckPaths;

}
