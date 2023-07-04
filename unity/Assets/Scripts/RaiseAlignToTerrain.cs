using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using the existing XZ positions, raise the object to the terrain Y (height) and align it with the normal.
/// </summary>
public class RaiseAlignToTerrain : MonoBehaviour
{
    
    public bool objectIsStatic = false;

    [HeaderAttribute("Raising")]
    public bool raiseToTerrainHeight = true;
    public float fixedExtraRaise = 0.0f;
    public float randomRaiseLower = 0.0f;
    
    [HeaderAttribute("Alignment")] 
    public bool alignToTerrain = true;
    public float alignmentStrengthNorm = 1.0f;
    
    private TerrainData terrainData;
    private Vector3 tPos;
    private float computedRaise;
    private CroquetSpatialComponent sc;
    private float radius;
    
    void Start()
    {
        var terrain = FindObjectOfType<Terrain>();
        terrainData = terrain.terrainData;
        tPos = terrain.gameObject.transform.position;

        computedRaise = fixedExtraRaise + Random.Range(-randomRaiseLower, randomRaiseLower);
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

            radius = objectIsStatic
                ? Croquet.ReadActorFloat(gameObject, "radius")
                : 0;
        }
        
        if (sc.hasBeenPlaced)
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
        if (!raiseToTerrainHeight && !alignToTerrain)
        {
            return;
        }
        
        Vector3 slopeNormal = terrainData.GetInterpolatedNormal((transform.position.x - tPos.x) / (terrainData.size.x),
            (transform.position.z - tPos.z) / (terrainData.size.z));

        if (raiseToTerrainHeight)
        {
            // Raise to terrain height with additional raise amount
            float slopeUnitOffset = Mathf.Sqrt(slopeNormal.x * slopeNormal.x + slopeNormal.z * slopeNormal.z);
            transform.position = new Vector3(
                transform.position.x,
                terrainData.GetInterpolatedHeight(
                    (transform.position.x - tPos.x) / (terrainData.size.x),
                    (transform.position.z - tPos.z) / (terrainData.size.z)
                    ) + computedRaise - slopeUnitOffset * radius,
                transform.position.z);
        }

        if (alignToTerrain)
        {
            // align rotation with interpolated terrain normal
            var slopeRotation = Quaternion.FromToRotation(transform.up,
                slopeNormal);
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation,
                alignmentStrengthNorm);
        }

        if (objectIsStatic)
        {
            string croquetHandle = gameObject.GetComponent<CroquetEntityComponent>().croquetHandle;
            CroquetSpatialSystem.Instance.SnapObjectTo(croquetHandle, transform.position, transform.rotation);
        }
    }
}
