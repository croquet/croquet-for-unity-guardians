using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneCleanUpInitiative : MonoBehaviour
{

    public string stubOfParentToAttachTo = "SceneCleanUpInitiative";
    void Start()
    {
        transform.parent = GameObject.Find(stubOfParentToAttachTo).transform;
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
