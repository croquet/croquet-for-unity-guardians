using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float frequency = 2.0f;
    public float magnitude = 0.1f;
    
    public Vector3 direction = new Vector3(0.0f, 0.1f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += direction.normalized * (magnitude * Mathf.Sin(frequency/2.0f * Time.time) * Mathf.Sin(frequency/3.0f * Time.time) * Time.deltaTime);
    }
}
