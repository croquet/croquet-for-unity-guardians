using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    
    [HeaderAttribute("FollowCam Settings")]
    public Transform target;
    public Vector3 followOffset = new Vector3(0.0f, 8.0f, -20f);
    public Vector3 rotationOffset = new Vector3(10, 0, 0);
    public float translationalLerpSpeed = 0.2f;
    public float rotationalSlerpSpeed = 0.2f;

    [HeaderAttribute("Fixed Camera View Settings")]
    public bool toggleFixedTopdownView = false;

    public Vector3 fixedPosition = new Vector3(0.0f, 20.0f, 0.0f);
    public Vector3 fixedRotation = new Vector3(90.0f, 0.0f, 0.0f);
    
    void LateUpdate()
    {
        if (target && !toggleFixedTopdownView)
        {
            float tTug = translationalLerpSpeed * Time.deltaTime;
            float rTug = rotationalSlerpSpeed * Time.deltaTime;

            Quaternion desiredRotation = Quaternion.Euler(rotationOffset.x, target.eulerAngles.y, 0);
            Vector3 desiredPosition = target.position + desiredRotation * followOffset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, tTug);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rTug);
        }
        else if (toggleFixedTopdownView)
        {
            transform.position = fixedPosition;
            transform.rotation = Quaternion.Euler(fixedRotation);
        }
        
    }
}
