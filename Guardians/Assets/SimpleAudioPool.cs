using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioPool : MonoBehaviour
{
    public List<GameObject> audioPool = new List<GameObject>();
    public Dictionary<GameObject, bool> audioPoolDict = new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, float> audioPoolPriority = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, GameObject> requesterToAudioSource = new Dictionary<GameObject, GameObject>();

    private void Start()
    {
        foreach (GameObject audioSourceObject in audioPool)
        {
            AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.loop = false;
                audioSource.playOnAwake = false;
                audioPoolDict[audioSourceObject] = false; // Initially, all are not playing
                audioPoolPriority[audioSourceObject] = 0f; // Default priority
            }
        }
    }

    public void RequestPlayAudio(GameObject requester, AudioClip clip, Transform position, bool loop, float priority, bool spatial)
    {
        GameObject availableSource = GetAvailableAudioSource(priority);

        if (availableSource != null)
        {
            AudioSource audioSource = availableSource.GetComponent<AudioSource>();
            audioSource.enabled = true;
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.spatialBlend = spatial ? 1f : 0f; // 1 for 3D sound, 0 for 2D sound
            audioSource.transform.position = position.position;
            audioSource.Play();

            // Track the playing state and priority
            audioPoolDict[availableSource] = true;
            audioPoolPriority[availableSource] = priority;
            requesterToAudioSource[requester] = availableSource;

            // Start a coroutine to track when the audio finishes playing
            StartCoroutine(TrackAudioSource(availableSource, requester));
        }
    }

    private GameObject GetAvailableAudioSource(float priority)
    {
        // Find an available audio source
        foreach (GameObject audioSourceObject in audioPool)
        {
            if (!audioPoolDict[audioSourceObject])
            {
                return audioSourceObject;
            }
        }

        // If no available source, find the one with the lowest priority
        GameObject lowestPrioritySource = null;
        float lowestPriority = float.MaxValue;

        foreach (KeyValuePair<GameObject, float> entry in audioPoolPriority)
        {
            if (entry.Value < lowestPriority)
            {
                lowestPriority = entry.Value;
                lowestPrioritySource = entry.Key;
            }
        }

        // If the incoming audio has higher priority, stop the lowest priority source
        if (lowestPrioritySource != null && priority > lowestPriority)
        {
            AudioSource lowestPriorityAudioSource = lowestPrioritySource.GetComponent<AudioSource>();
            if (lowestPriorityAudioSource != null)
            {
                lowestPriorityAudioSource.Stop();
                audioPoolDict[lowestPrioritySource] = false; // Mark as available
            }
            return lowestPrioritySource;
        }

        return null; // No available source and no lower priority source to interrupt
    }

    private IEnumerator TrackAudioSource(GameObject audioSourceObject, GameObject requester)
    {
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();

        while (audioSource != null && audioSource.isPlaying)
        {
            yield return null; // Wait until the next frame
        }

        // Audio finished playing
        if (audioSource != null)
        {
            audioPoolDict[audioSourceObject] = false;
            audioPoolPriority[audioSourceObject] = 0f; // Reset priority
            audioSourceObject.transform.position = transform.position; // Reset parent
            audioSourceObject.transform.localPosition = Vector3.zero; // Reset position
            requesterToAudioSource.Remove(requester);
        }
    }

    // Method to stop the audio for a specific requester
    public void StopAudio(GameObject requester)
    {
        if (requesterToAudioSource.ContainsKey(requester))
        {
            GameObject audioSourceObject = requesterToAudioSource[requester];
            AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Stop();
                audioPoolDict[audioSourceObject] = false;
                audioPoolPriority[audioSourceObject] = 0f;
                audioSourceObject.transform.position = transform.position; // Reset parent
                audioSourceObject.transform.localPosition = Vector3.zero;
                requesterToAudioSource.Remove(requester);
            }
        }
    }
}
