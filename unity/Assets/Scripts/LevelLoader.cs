using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static void LoadLevel(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);

        Debug.Log($"Loading Level: {level}");
    }
}
