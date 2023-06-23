using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthCoinController : MonoBehaviour
{
    public TMP_Text healthTextFront;
    public TMP_Text healthTextBack;
    private GameState gameState;
    void Start()
    {
        // Croquet.Subscribe("stats", "health", SetHealth);
    }

    private void Update()
    {
        if (gameState == null)
        {
            gameState = GameObject.FindWithTag("GameController").GetComponent<GameState>();
            if (gameState == null)
            {
                return;
            }
        }
        SetHealth(gameState.health);
    }

    void SetHealth(float health)
    {
        healthTextFront.text = $"{health}";
        healthTextBack.text = $"{health}";
    }
}
