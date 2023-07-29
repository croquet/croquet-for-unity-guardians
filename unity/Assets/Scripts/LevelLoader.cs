using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static void StartSession()
    {
        CroquetBridge bridge = FindObjectOfType<CroquetBridge>();
        if (bridge != null)
        {
            string sessionName = PlayerPrefs.GetInt("sessionNameValue", 0).ToString();
            bridge.SetSessionName(sessionName); // this will start the session
        }
    }
}
