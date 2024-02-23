using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AssignFollowCamTarget : MonoBehaviour
{
    public FollowCam followCamToUpdate;

    void Awake()
    {
        Croquet.Subscribe("croquet", "sceneRunning", CroquetSceneRunning);
    }

    void Update()
    {
        CroquetDrivableComponent a = CroquetDrivableSystem.Instance.GetActiveDrivableComponent();

        if ( a != null)
        {
            followCamToUpdate.target = a.transform;

            enabled = false;
        }
    }

    void CroquetSceneRunning()
    {
        enabled = true; // make sure we're looking for the avatar
    }
}
