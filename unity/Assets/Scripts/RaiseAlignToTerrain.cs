using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using the existing XZ positions, raise the object to the terrain Y (height) and align it with the normal.
/// </summary>
public class RaiseAlignToTerrain : MonoBehaviour
{
    [HeaderAttribute("Raising")]
    public bool raiseToTerrainHeight = true;
    public float additionalRaise = 0.0f;
    public float raiseSpreadStrength = 0.0f;

    [HeaderAttribute("Alignment")] 
    public bool alignToTerrain = true;
    public float alignmentStrengthNorm = 1.0f;
    
    
    private TerrainData terrainData;
    private float computedRaise;
    
    
    void Start()
    {
        terrainData = FindObjectOfType<Terrain>().terrainData;
        computedRaise = additionalRaise + Random.Range(-raiseSpreadStrength, raiseSpreadStrength);
    }

    void Update()
    {
        // Raise to terrain height with additional raise amount
        transform.position = new Vector3(
            transform.position.x,
            terrainData.GetInterpolatedHeight(
                transform.position.x/(terrainData.size.x),
                transform.position.z/(terrainData.size.z)) + computedRaise,
            transform.position.z);
        
        // align rotation with interpolated terrain normal
        var slopeRotation = Quaternion.FromToRotation(
            transform.up, 
            terrainData.GetInterpolatedNormal(transform.position.x/(terrainData.size.x), transform.position.z/(terrainData.size.z)));
        transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, alignmentStrengthNorm);

        
    }
}
