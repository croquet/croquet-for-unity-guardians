using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapToTerrain : MonoBehaviour
{
    private Terrain _terrain;
    private float randomOffset; 
    
    void Start()
    {
        _terrain = FindObjectOfType<Terrain>();
        randomOffset = Random.Range(0.0f, 20f);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            _terrain.terrainData.GetInterpolatedHeight(transform.position.x/(_terrain.terrainData.size.x), transform.position.z/(_terrain.terrainData.size.z))+.2f*Mathf.Sin(randomOffset+randomOffset*Time.time),
            transform.position.z);
        
        var slopeRotation = Quaternion.FromToRotation(transform.up, _terrain.terrainData.GetInterpolatedNormal(transform.position.x/(_terrain.terrainData.size.x), transform.position.z/(_terrain.terrainData.size.z)));
        transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10 * Time.deltaTime);
    }
}
