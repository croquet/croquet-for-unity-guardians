using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class bounceNoise : MonoBehaviour
{

    private void Awake()
    {
        Croquet.Listen(gameObject, "ballisticVelocitySet", PlayBounceNoise);
    }

    void PlayBounceNoise()
    {
        GetComponent<SimpleAudioRequester>().RequestPlayAudio();
    }
}
