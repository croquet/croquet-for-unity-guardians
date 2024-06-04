using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AssignFollowCamTarget : MonoBehaviour
{
    public FollowCam followCamToUpdate;

    void Awake()
    {
        // in case the scene is reloaded, make sure the original camera lives on
        // and that no duplicate camera - with duplicate subscriptions - is started up.
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MainCamera");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            Croquet.Subscribe("croquet", "sceneRunning", CroquetSceneRunning);
        }
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
