using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, ICroquetDriven
{
    public float health;
    public bool gameEnded;
    public int totalBots;
    public int wave;
    public int kills;
    CroquetBridge croquetBridge;
    
    void Awake()
    {
        Croquet.Listen(gameObject, "gameEndedSet", GameEndedSet);
        Croquet.Listen(gameObject, "waveSet", WaveSet);
        Croquet.Listen(gameObject, "totalBotsSet", TotalBotsSet);
        Croquet.Listen(gameObject, "healthSet", HealthSet);
        croquetBridge = FindObjectOfType<CroquetBridge>();
        if (croquetBridge != null) 
        {
            Croquet.Subscribe(croquetBridge.croquetViewId, "killsSet", KillsSet);
        }
    }

    public void PawnInitializationComplete()
    {
        GameEndedSet(Croquet.ReadActorBool(gameObject, "gameEnded"));
        WaveSet(Croquet.ReadActorFloat(gameObject, "wave"));
        TotalBotsSet(Croquet.ReadActorFloat(gameObject, "totalBots"));
        HealthSet(Croquet.ReadActorFloat(gameObject, "health"));
        KillsSet(Croquet.ReadActorFloat(gameObject, "kills"));
    }

    public void StartGame()
    {
        Croquet.Publish("game", "startGame");
    }
    
    public void GameEndedSet(bool gameEnded)
    {
        this.gameEnded = gameEnded;
        Debug.Log($"GameEndedSet: {gameEnded}");
    }

    void WaveSet(float wave)
    {
        this.wave = (int)wave;
        // Debug.Log($"WaveSet: {wave}");
    }
    
    void TotalBotsSet(float bots)
    {
        this.totalBots = (int)bots;
        // Debug.Log($"TotalBotsSet: {bots}");
    }
    
    void HealthSet(float health)
    {
        // Debug.Log($"HealthSet: {health}");
        this.health = health;
    }
    void KillsSet(float kills)
    {
        // Debug.Log($"HealthSet: {health}");
        this.kills = (int)kills;
    }

}
