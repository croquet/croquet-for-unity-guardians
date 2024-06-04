using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MuteKey : MonoBehaviour
{
    public bool muted = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            muted = !muted;

            if (muted)
            {
                AudioListener.volume = 0.0f;

            }

            else
            {
                AudioListener.volume = 1.0f;
            }
        }
    }
}
