using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public List<GameObject> structures;

    public int numStructures;
    public float innerDeadZoneDistance = 50.0f;
    public float outerDeadZoneDistance = 100.0f;
    
    void Start()
    {
        for (int i = 0; i < numStructures; i++)
        {
            GameObject go = Instantiate(structures[Random.Range(0, structures.Count)]) as GameObject;
            go.transform.position = Random.insideUnitSphere*outerDeadZoneDistance;

            float newX = Mathf.Sign(go.transform.position.x) * fit(go.transform.position.x, -outerDeadZoneDistance, outerDeadZoneDistance,
                innerDeadZoneDistance, outerDeadZoneDistance);
            float newZ = Mathf.Sign(go.transform.position.z) *  fit(go.transform.position.z, -outerDeadZoneDistance, outerDeadZoneDistance,
                innerDeadZoneDistance, outerDeadZoneDistance);
            go.transform.position = new Vector3(newX, 0.0f, newZ);
        }
    }
    
    private float fit( float value, float from, float to, float from2,  float to2)
    {
        return Mathf.Lerp (from2, to2, Mathf.InverseLerp (from, to, value));
    }
}
