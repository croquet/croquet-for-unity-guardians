using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounceNoise : MonoBehaviour, ICroquetDriven
{
    private bool skippedOnce = false;
    private AudioSource bounceSound;

    private void Start()
    {
        bounceSound = this.GetComponent<AudioSource>();
    }

    public void PawnInitializationComplete()
    {
        Croquet.Listen(gameObject, "ballisticVelocitySet", PlayBounceNoise);
    }

    void PlayBounceNoise()
    { Debug.Log("bounce");
        if (skippedOnce)
        {
            bounceSound.PlayOneShot(bounceSound.clip);
        }

        skippedOnce = true;
    }
}
