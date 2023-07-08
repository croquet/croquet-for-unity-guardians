using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class bounceNoise : MonoBehaviour, ICroquetDriven
{
    private AudioSource bounceSound;

    private void Awake()
    {
        bounceSound = this.GetComponent<AudioSource>();
        Croquet.Listen(gameObject, "ballisticVelocitySet", PlayBounceNoise);
    }

    void PlayBounceNoise()
    {
        Debug.Log("plink?");
        bounceSound.pitch = Random.Range(0.3f, 3.0f);
        bounceSound.PlayOneShot(bounceSound.clip);
    }
}
