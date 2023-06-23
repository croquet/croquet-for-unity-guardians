using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMyColorFromIndex : MonoBehaviour, ICroquetDriven
{
    public void CroquetInitializationComplete()
    {
        float colorIndex = Croquet.ReadActorFloat(gameObject, "colorIndex");
        Debug.Log($"time to set {gameObject} color from colorIndex {colorIndex}");
    }
}
