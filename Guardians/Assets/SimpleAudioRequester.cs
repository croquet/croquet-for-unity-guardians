using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioRequester : MonoBehaviour
{
    public AudioClip clip;
    public bool loop;
    public float priority;
    public bool spatial;
    public bool startPlaying;

    // Start is called before the first frame update
    void Start()
    {
        loop = false;
        priority = 0f;
        spatial = true;
        if (startPlaying)
        {
            RequestPlayAudio();
        }   
    }

    public void RequestPlayAudio()
    {
        FindObjectOfType<SimpleAudioPool>().RequestPlayAudio(clip, transform, loop, priority, spatial);
    }
}
