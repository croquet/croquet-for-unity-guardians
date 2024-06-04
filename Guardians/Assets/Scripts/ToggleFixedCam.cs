using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFixedCam : MonoBehaviour
{
    private FollowCam followCam;

    void Awake()
    {
        followCam = GetComponent<FollowCam>();

        Croquet.Subscribe("all", "godModeChanged", SetToggleFixedCam);
    }

    void Update()
    {
        // Switch my View
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetToggleFixedCam(!followCam.toggleFixedTopdownView);

            // Switch Everyone's View
            if (Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.RightShift)))
            {
                Croquet.Publish("all", "godMode", followCam.toggleFixedTopdownView);
            }
        }
    }

    void SetToggleFixedCam(bool state)
    {
        followCam.toggleFixedTopdownView = state;
    }

}
