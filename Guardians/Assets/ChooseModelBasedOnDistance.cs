using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseModelBasedOnDistance : MonoBehaviour
{
    public GameObject[] models; // Array of models to choose from
    public float[] distances; // Array of distances to switch models
    // public Transform target; // The target to measure distance from, default to origin (0,0,0)
    private Vector3 origin = Vector3.zero;
    private GameObject currentModel;

    // Start is called before the first frame update
    void Start()
    {
        // If no target is specified, default to origin
        // if (target == null)
        // {
        //     // target = transform;
        //     target.position = origin;
        // }
        foreach (GameObject model in models)
        {
            model.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, origin);
        int modelIndex = ChooseModelIndexBasedOnDistance(distance);

        if (currentModel == null || currentModel != models[modelIndex])
        {
            if (currentModel != null)
            {
                Destroy(currentModel);
            }
            models[modelIndex].SetActive(true);
        }
    }

    int ChooseModelIndexBasedOnDistance(float distance)
    {
        // Example logic to choose a model based on distance
        // Customize this to fit your needs
        if (distance < distances[0])
        {
            return 0; // Model for close distance
        }
        else if (distance < distances[1])
        {
            return 1; // Model for medium distance
        }
        else
        {
            return 2; // Model for far distance
        }
    }
}
