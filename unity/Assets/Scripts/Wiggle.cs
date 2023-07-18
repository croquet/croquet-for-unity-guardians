using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public bool wiggleScale = true;
    public Vector3 scaleWiggle = Vector3.one;

    public float scaleWiggleMag = 0.1f;

    public float scaleWiggleFreq = 2.0f;

    private Vector3 originalScale;
    void Start()
    {
        if (wiggleScale)
        {
            originalScale = transform.localScale;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        {
            transform.localScale = originalScale + scaleWiggle * (scaleWiggleMag * Mathf.Cos(scaleWiggleFreq * Time.time));
        }        
    }
}
