using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlaySuspenseMusic : MonoBehaviour
{
    private AudioSource suspenseLoop;

    private float nearestBotDistanceToBase;

    public float DistanceMin = 10.0f;
    public float DistanceMax = 200.0f;
    public float pitchMin = .5f;
    public float pitchMax = 2.0f;
    public float volumeMin = .1f;
    public float volumeMax = 0.7f;

    public float fadeTime = 1.0f;

    private GameObject closestDetected;
    
    
    private float velocity = 0.00f;
    
    private void Awake()
    {
        suspenseLoop = this.GetComponent<AudioSource>();
        Croquet.Subscribe("bots", "madeWave", PlaySuspense);

        //StartCoroutine(FindNearestBot());
    }

    void PlaySuspense()
    {
        suspenseLoop.pitch = Utility.fit(nearestBotDistanceToBase, DistanceMin, DistanceMax, pitchMax, pitchMin);
        suspenseLoop.volume = Utility.fit(nearestBotDistanceToBase, DistanceMin, DistanceMax, volumeMax, volumeMin);
        //Debug.Log($"Playing suspense with pitch: {suspenseLoop.pitch} because of nearestDistance: {nearestBotDistanceToBase}" );
        suspenseLoop.loop = true;
        suspenseLoop.Play();
    }

    private void Update()
    {
        closestDetected = GameObject.FindGameObjectWithTag("Enemy");
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (bot.transform.position.magnitude < closestDetected.transform.position.magnitude)
            {
                closestDetected = bot;
            }
        }
        if (closestDetected == null)
        {
            suspenseLoop.loop = false;
        }
        else
        {
            nearestBotDistanceToBase = closestDetected.transform.position.magnitude;
            
            suspenseLoop.pitch = Mathf.SmoothDamp( suspenseLoop.pitch, Utility.fit(nearestBotDistanceToBase, DistanceMin, DistanceMax, pitchMax, pitchMin), ref velocity, fadeTime);

            suspenseLoop.volume = Mathf.SmoothDamp(suspenseLoop.volume, Utility.fit(nearestBotDistanceToBase, DistanceMin, DistanceMax, volumeMax, volumeMin), ref velocity, fadeTime ); ;
        }
    }

    // IEnumerator FindNearestBot()
    // {
    //     for (;;)
    //     {
    //         GameObject closestDetected = GameObject.FindGameObjectWithTag("Enemy");
    //     
    //         foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
    //         {
    //             Debug.Log($"Analyzing Enemy {bot.name} with distance {bot.transform.position.magnitude}");
    //             if (bot.transform.position.magnitude < closestDetected.transform.position.magnitude)
    //             {
    //                 closestDetected = bot;
    //             }
    //         }
    //     
    //         if (closestDetected == null)
    //         {
    //             suspenseLoop.loop = false;
    //             yield return new WaitForSeconds(3.0f);
    //         }
    //         else
    //         {
    //             nearestBotDistanceToBase = closestDetected.transform.position.magnitude;
    //         }
    //
    //     
    //         yield return new WaitForSeconds(3.0f);
    //     }
    //     
    // }
}
