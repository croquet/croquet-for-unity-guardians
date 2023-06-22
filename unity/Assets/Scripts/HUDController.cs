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

    public float logoSustain = 5.0f;
    
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
        gameOverPanel.SetActive(false);
        StartCoroutine(ShowThenFade());
    }

    void SetWave(float wave) // TODO: Needs to happen if someone joins midwave too
    {
        waveText.text = $"Wave: {wave}";
    }

    void SetHealth(float health)
    {
        healthText.text = $"Health: {health}";
    }

    void SetBots(float bots)
    {
        botCountText.text = $"Bots: {bots}";
    }
    
    void Finish()
    {
        gameOverPanel.SetActive(true);
    }

    IEnumerator ShowThenFade()
    {
        gameStartObject.SetActive(true);
        yield return new WaitForSeconds(logoSustain);
        gameStartObject.SetActive(false);
    }
}
