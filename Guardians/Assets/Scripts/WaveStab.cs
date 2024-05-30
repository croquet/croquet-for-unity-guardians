using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStab : MonoBehaviour
{
    private AudioSource waveStab;

    void Awake()
    {
        waveStab = this.GetComponent<AudioSource>();
        Croquet.Subscribe("bots", "madeWave", PlayWaveStab);
    }

    void PlayWaveStab()
    {
        waveStab.PlayOneShot(waveStab.clip);
    }
}
