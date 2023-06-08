using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAround : MonoBehaviour
{
    public float speed = 1.0f;
    public float boostSpeedFactor = 2.0f;
    public float rotationSpeed = 10.0f;

    private Terrain _terrain;
    private float boostSpeed;
    private float computedSpeed;
    void Start()
    {
        _terrain = FindObjectOfType<Terrain>();
        boostSpeed = boostSpeedFactor * speed;
        computedSpeed = speed;
    }

  
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            computedSpeed = boostSpeed;
        }
        else
        {
            computedSpeed = speed;
        }
        
        transform.Translate(transform.forward * (computedSpeed * Time.deltaTime * Input.GetAxis("Vertical")), Space.World);
        transform.position = new Vector3(transform.position.x,
            _terrain.terrainData.GetInterpolatedHeight(transform.position.x/(_terrain.terrainData.size.x), transform.position.z/(_terrain.terrainData.size.z)),
            transform.position.z);
        
        
        transform.Rotate(Vector3.up, rotationSpeed*Time.deltaTime*Input.GetAxis("Horizontal"));

        var slopeRotation = Quaternion.FromToRotation(transform.up, _terrain.terrainData.GetInterpolatedNormal(transform.position.x/(_terrain.terrainData.size.x), transform.position.z/(_terrain.terrainData.size.z)));
        transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10.0f* Time.deltaTime);
    }
}
