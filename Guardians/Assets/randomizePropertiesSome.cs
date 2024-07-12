using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizePropertiesSome : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Randomly rotate on the y-axis
        float randomYRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);

        // Randomly change size up to 10% on every axis
        Vector3 originalScale = transform.localScale;
        float scaleFactor = Random.Range(0.9f, 1.1f);
        transform.localScale = new Vector3(
            originalScale.x * scaleFactor,
            originalScale.y * scaleFactor,
            originalScale.z * scaleFactor
        );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
 