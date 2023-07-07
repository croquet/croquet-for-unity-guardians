using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounceNoise : MonoBehaviour
{
    private bool skippedOnce = false; 
    private AudioSource bounceSound;

    private void Start()
    {
        bounceSound = this.GetComponent<AudioSource>();
    }

    void Awake()
    {
        Croquet.Listen(gameObject, "ballisticVelocitySet", PlayBounceNoise);
    }

    void PlayBounceNoise()
    {
        if (skippedOnce)
        {
            bounceSound.PlayOneShot(bounceSound.clip);
        }

        skippedOnce = true;
    }
}
