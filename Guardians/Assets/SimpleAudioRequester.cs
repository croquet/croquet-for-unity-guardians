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

    private SimpleAudioPool audioPool;

    // Start is called before the first frame update
    void Start()
    {
        loop = false;
        priority = 0f;
        spatial = true;
        audioPool = FindObjectOfType<SimpleAudioPool>();
        if (startPlaying)
        {
            RequestPlayAudio();
        }   
    }

    public void RequestPlayAudio()
    {
        audioPool.RequestPlayAudio(gameObject, clip, transform, loop, priority, spatial);
    }

    public void RequestStopAudio()
    {
        audioPool.StopAudio(gameObject);
    }
    void OnDestroy()
    {
        audioPool.StopAudio(gameObject);
    }
    void OnDisable()
    {
        audioPool.StopAudio(gameObject);
    }   
}
