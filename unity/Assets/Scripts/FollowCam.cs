using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 followOffset = new Vector3(0.0f, 8.0f, -20f);
    public Vector3 rotationOffset = new Vector3(10, 0, 0);
    public float translationalLerpSpeed = 0.2f;
    public float rotationalSlerpSpeed = 0.2f;

    void LateUpdate()
    {
        if (target)
        {
            float tTug = translationalLerpSpeed * Time.deltaTime;
            float rTug = rotationalSlerpSpeed * Time.deltaTime;

            Quaternion desiredRotation = Quaternion.Euler(rotationOffset.x, target.eulerAngles.y, 0);
            Vector3 desiredPosition = target.position + desiredRotation * followOffset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, tTug);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rTug);
        }
    }
}
