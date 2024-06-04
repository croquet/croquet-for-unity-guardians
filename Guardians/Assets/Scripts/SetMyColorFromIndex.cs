using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMyColorFromIndex : MonoBehaviour, ICroquetDriven
{
    public int numColors = 24;
    public Texture2D colorPallette;
    public Renderer rendererToSetColor;
    
    public void PawnInitializationComplete()
    {
        float colorIndex = Croquet.ReadActorFloat(gameObject, "colorIndex");

        MaterialPropertyBlock myMaterialPropertyBlock = new MaterialPropertyBlock();
        myMaterialPropertyBlock.SetColor("_Color", colorPallette.GetPixelBilinear((colorIndex / (float)numColors) + (1.0f/(float)numColors)/2.0f, 0.5f));
        
        rendererToSetColor.SetPropertyBlock(myMaterialPropertyBlock);
        // There is an extra factor of (1/numColors)/2.0 added to the U-Coordinate to sample the center of each square
        
    }
}
