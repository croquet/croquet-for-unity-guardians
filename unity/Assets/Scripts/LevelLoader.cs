using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static void StartSession()
    {
        Croquet.SetSessionName(sessionName); // this will start the session
    }
}
