using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDStuff : MonoBehaviour
{
    // placeholder for all the things the HUD needs to listen to
    
    void Start()
    {
        Croquet.Subscribe("game", "gameStarted", GameStart);
        Croquet.Subscribe("game", "endGame", Finish);
        Croquet.Subscribe("user", "endGame", Finish);
        Croquet.Subscribe("stats", "wave", SetWave);
        Croquet.Subscribe("stats", "health", SetHealth);
        Croquet.Subscribe("stats", "bots", SetBots);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Croquet.Publish("game", "undying");
        }
    }

    void GameStart()
    {
        Debug.Log($"HUD: game started");
    }

    void SetWave(float wave)
    {
        Debug.Log($"HUD: wave {wave}");
    }

    void SetHealth(float health)
    {
        Debug.Log($"HUD: health {health}");
    }

    void SetBots(float bots)
    {
        Debug.Log($"HUD: bots {bots}");
    }


    void Finish()
    {
        Debug.Log("HUD: finish");
    }
}
