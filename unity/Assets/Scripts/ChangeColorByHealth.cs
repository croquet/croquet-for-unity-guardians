using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorByHealth : MonoBehaviour
{
    private GameState gameState;
    public Renderer rendererToSetColor;

    private MaterialPropertyBlock myMaterialPropertyBlock;
    void Start()
    {
        myMaterialPropertyBlock = new MaterialPropertyBlock();
        
        
    }

    // Update is called once per frame
    void Update()
    {
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
        
        if (gameState.health > 66)
        {
            myMaterialPropertyBlock.SetColor("_Color", Color.green );
        } 
        else if (gameState.health > 33)
        {
            myMaterialPropertyBlock.SetColor("_Color", Color.yellow );
        }
        else if (gameState.health > 0)
        {
            myMaterialPropertyBlock.SetColor("_Color", Color.red );
        }
        else
        {
            myMaterialPropertyBlock.SetColor("_Color", Color.black );
        }
        
        rendererToSetColor.SetPropertyBlock(myMaterialPropertyBlock);
    }
}
