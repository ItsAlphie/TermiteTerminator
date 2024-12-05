using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private AudioSource audioSource;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioClip gameOver;

    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Now returns the AudioSource so it can be referenced later
    public AudioSource PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn in gameObject
        audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        // Assign audioclip
        audioSource.clip = audioClip;
        // Assign volume
        audioSource.volume = volume;
        // Play sound
        audioSource.Play();

        // Track active AudioSource
        activeAudioSources.Add(audioSource);

        // Get length of sound FX clip
        float cliplength = audioSource.clip.length;

        // Remove from tracking after it finishes
        StartCoroutine(CleanupAfterPlay(audioSource, cliplength));

        return audioSource; // Return the AudioSource so it can be used elsewhere
    }

    public void StopSound(AudioSource source)
{
    if (source != null)
    {
        Debug.Log($"Stopping sound for AudioSource: {source}");
        source.Stop();
        Debug.Log("Sound stopped. Destroying AudioSource GameObject...");
        Destroy(source.gameObject);
        activeAudioSources.Remove(source);
        Debug.Log("AudioSource removed from active list.");
    }
    else
    {
        Debug.LogWarning("Attempted to stop a null AudioSource.");
    }
}


    public void StopAllSounds()
    {
        foreach (var source in activeAudioSources)
        {
            if (source != null)
            {
                source.Stop();
                Destroy(source.gameObject);
            }
        }
        activeAudioSources.Clear();
    }

    private IEnumerator CleanupAfterPlay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (source != null)
        {
            activeAudioSources.Remove(source);
            Destroy(source.gameObject);
        }
    }

    public void InterruptSound()
    {
        StopAllSounds();
    }
}
