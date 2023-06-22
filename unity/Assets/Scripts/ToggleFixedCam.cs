using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFixedCam : MonoBehaviour
{
    private FollowCam followCam;
    
    void Start()
    {
        followCam = GetComponent<FollowCam>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // TODO: OR a Response to a Shift+G God Cam call for everyone
        {
            followCam.toggleFixedTopdownView = !followCam.toggleFixedTopdownView;
        }
    }
}
