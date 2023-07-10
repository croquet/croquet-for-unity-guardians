using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBots : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Croquet.Publish("game", "bots", 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Croquet.Publish("game", "bots", 25);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Croquet.Publish("game", "bots", 50);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Croquet.Publish("game", "bots", 100);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Croquet.Publish("game", "bots", 250);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Croquet.Publish("game", "bots", 500);
        }
    }
}
