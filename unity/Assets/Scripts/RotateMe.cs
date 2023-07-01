using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour, ICroquetDriven

{
    public Vector3 axis = Vector3.up;
    public float speed = Mathf.Floor(Mathf.Rad2Deg); // degrees per second
    private string croquetHandle;
    
    public void PawnInitializationComplete()
    {
        croquetHandle = gameObject.GetComponent<CroquetEntityComponent>().croquetHandle;
    }
    void Update()
    {
        float sessionTime = Croquet.SessionTime();
        if (sessionTime == -1f) return; // not ready yet

        float angle = - speed * sessionTime;
        // we can't attempt to rotate the Croquet-controlled gameObject, because the Spatial
        // System will constantly seek to align it with the Croquet actor.  but we can
        // rotate its children.
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
