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
    public bool objectIsStatic = false;

    [HeaderAttribute("Alignment")] 
    public bool alignToTerrain = true;
    public float alignmentStrengthNorm = 1.0f;
    
    private TerrainData terrainData;
    private Vector3 tPos;
    private float computedRaise;
    private string croquetHandle;
    private CroquetSpatialComponent sc;
    
    void Start()
    {
        var terrain = FindObjectOfType<Terrain>();
        terrainData = terrain.terrainData;
        tPos = terrain.gameObject.transform.position;

        computedRaise = additionalRaise + Random.Range(-raiseSpreadStrength, raiseSpreadStrength);
    }

    void Update()
    {
        if (sc == null)
        {
            sc = gameObject.GetComponent<CroquetSpatialComponent>();
            if (sc == null)
            {
                return;
            }
        }
        
        if (sc.hasBeenMoved)
        {
            Align();
            
            if (objectIsStatic)
            {
                // only need to run once
                Destroy(this);
            }            
        }
    }
    
    void Align()
    {
        if (raiseToTerrainHeight)
        {
            // Raise to terrain height with additional raise amount
            transform.position = new Vector3(
                transform.position.x,
                terrainData.GetInterpolatedHeight(
                    (transform.position.x - tPos.x) / (terrainData.size.x),
                    (transform.position.z - tPos.z) / (terrainData.size.z)
                    ) + computedRaise,
                transform.position.z);
        }

        if (alignToTerrain)
        {
            // align rotation with interpolated terrain normal
            var slopeRotation = Quaternion.FromToRotation(transform.up,
                terrainData.GetInterpolatedNormal((transform.position.x - tPos.x) / (terrainData.size.x),
                    (transform.position.z - tPos.z) / (terrainData.size.z)));
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation,
                alignmentStrengthNorm);
        }
        
        string croquetHandle = gameObject.GetComponent<CroquetEntityComponent>().croquetHandle;
        CroquetSpatialSystem.Instance.SnapObjectTo(croquetHandle, transform.position, transform.rotation);
    }
}
