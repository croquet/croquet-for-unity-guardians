using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour

{
    public Vector3 axis = Vector3.up;
    public float speed = 0.1f;
    
    void Update()
    {
        transform.RotateAround(transform.position, axis, speed*Time.deltaTime);
    }
}
