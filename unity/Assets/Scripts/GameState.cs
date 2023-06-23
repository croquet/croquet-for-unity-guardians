using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, ICroquetDriven
{
    void Awake()
    {
        Croquet.Listen(gameObject, "gameEndedSet", GameEndedSet);
        Croquet.Listen(gameObject, "waveSet", WaveSet);
        Croquet.Listen(gameObject, "totalBotsSet", TotalBotsSet);
        Croquet.Listen(gameObject, "healthSet", HealthSet);
    }

    public void CroquetInitializationComplete()
    {
        GameEndedSet(Croquet.ReadActorBool(gameObject, "gameEnded"));
        WaveSet(Croquet.ReadActorFloat(gameObject, "wave"));
        TotalBotsSet(Croquet.ReadActorFloat(gameObject, "totalBots"));
        HealthSet(Croquet.ReadActorFloat(gameObject, "health"));
    }
    
    void GameEndedSet(bool gameEnded)
    {
        Debug.Log($"GameEndedSet: {gameEnded}");
    }

    void WaveSet(float wave)
    {
        Debug.Log($"WaveSet: {wave}");
    }
    
    void TotalBotsSet(float bots)
    {
        Debug.Log($"TotalBotsSet: {bots}");
    }
    
    void HealthSet(float health)
    {
        Debug.Log($"HealthSet: {health}");
    }

}