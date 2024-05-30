using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static void StartSession()
    {
        string sessionName = PlayerPrefs.GetInt("sessionNameValue", 0).ToString();
        Croquet.SetSessionName(sessionName); // this will start the session
    }
}
