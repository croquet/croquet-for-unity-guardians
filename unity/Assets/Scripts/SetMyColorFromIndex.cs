using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMyColorFromIndex : MonoBehaviour, ICroquetDriven
{
    public int numColors = 24;
    public Texture2D colorPallette;
    public Material materialToSetColor;
    
    public void CroquetInitializationComplete()
    {
        float colorIndex = Croquet.ReadActorFloat(gameObject, "colorIndex");
        // Debug.Log($"time to set {gameObject} color from colorIndex {colorIndex}, u coordinate: {colorIndex/(float)numColors}");

        materialToSetColor.color = colorPallette.GetPixelBilinear((colorIndex / (float)numColors), 0.5f);
    }
}
