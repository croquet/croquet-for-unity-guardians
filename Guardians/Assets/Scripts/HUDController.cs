using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{

    public TMP_Text waveText;
    public TMP_Text healthText;
    public TMP_Text botCountText;
    public GameObject gameOverPanel;
    public GameObject gameStartObject;

    // public float logoSustain = 5.0f;


    private GameState gameState;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Croquet.Publish("game", "undying");
        }

        if (gameState == null)
        {
            GameObject gameStateGO = GameObject.FindWithTag("GameController");
            if (gameStateGO != null)
            {
                gameState = gameStateGO.GetComponent<GameState>();
            }

            if (gameState == null)
            {
                return;
            }
        }

        SetHealth(gameState.health);
        SetBots(gameState.totalBots);
        SetWave(gameState.wave);

        if (gameState.gameEnded)
        {
            GameEnded();
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }


    void SetWave(float wave)
    {
        waveText.text = $"{wave}";
    }

    void SetHealth(float health)
    {
        healthText.text = $"{health}";
    }

    void SetBots(float bots)
    {
        botCountText.text = $"{bots}";
    }

    void GameEnded()
    {
        gameOverPanel.SetActive(true);
    }

    // IEnumerator ShowThenFade()
    // {
    //     gameStartObject.SetActive(true);
    //     yield return new WaitForSeconds(logoSustain);
    //     gameStartObject.SetActive(false);
    // }

    public void StartANewGame()
    {
        if(gameState != null)
        {
            gameState.StartGame();
        }
    }
}
