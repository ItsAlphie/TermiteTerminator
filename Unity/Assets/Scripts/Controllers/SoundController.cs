using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private AudioSource audioSource;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] public AudioClip startGameClip;
    [SerializeField] public AudioClip errorClip;
    [SerializeField] public AudioClip loseGameClip;
    [SerializeField] public AudioClip winGameClip;
    [SerializeField] public AudioClip backgroundClip;
    [SerializeField] public AudioClip receiveMoneyClip;
    [SerializeField] public AudioClip loseMoneyClip;
    [SerializeField] public AudioClip bombClip;
    [SerializeField] public AudioClip towerHitClip;
    public AudioSource startGameSource;
    public AudioSource errorSource;
    public AudioSource loseGameSource;
    public AudioSource winGameSource;
    public AudioSource backgroundSource;
    public AudioSource receiveMoneySource;
    public AudioSource loseMoneySource;
    public AudioSource bombSource;
    public AudioSource towerHitSource;


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
    public void PlayBackground()
    {
        startGameSource = Instantiate(soundFXObject);
        startGameSource.clip = startGameClip;
        startGameSource.volume = 0.1f;
        startGameSource.Play();
        StartCoroutine(PlayBackgroundAfterDelay(startGameClip.length));
    }

    private IEnumerator PlayBackgroundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        backgroundSource = Instantiate(soundFXObject);
        backgroundSource.clip = backgroundClip;
        backgroundSource.volume = 0.1f;
        backgroundSource.loop = true;
        backgroundSource.Play();
        activeAudioSources.Add(backgroundSource);
    }
    public void PlayMoneyReceive(){
        receiveMoneySource = Instantiate(soundFXObject);
        receiveMoneySource.clip = receiveMoneyClip;
        receiveMoneySource.volume = 0.6f;
        receiveMoneySource.Play();
        float receiveMoneyClipLength = receiveMoneySource.clip.length;
        Destroy(receiveMoneySource.gameObject, receiveMoneyClipLength);
    }
    public void PlayMoneyLose(){
        loseMoneySource = Instantiate(soundFXObject);
        loseMoneySource.clip = loseMoneyClip;
        loseMoneySource.volume = 0.6f;
        loseMoneySource.Play();
        float loseMoneyClipLength = loseMoneySource.clip.length;
        Destroy(loseMoneySource.gameObject, loseMoneyClipLength);
    }
    public void PlayLoseGame(){
        StopAllSounds();
        StopSound(backgroundSource);
        StopSound(startGameSource);
        loseGameSource = Instantiate(soundFXObject);
        loseGameSource.clip = loseGameClip;
        loseGameSource.volume = 1f;
        loseGameSource.Play();
        float loseGameClipLength = loseGameSource.clip.length;
        Destroy(loseGameSource.gameObject, loseGameClipLength);
    }
    public void PlayErrorSound(){
        errorSource = Instantiate(soundFXObject);
        errorSource.clip = errorClip;
        errorSource.volume = 1f;
        errorSource.Play();
        float errorClipLength = errorSource.clip.length;
        Destroy(errorSource.gameObject, errorClipLength);
    }
    public void PlayWinWave(){
        winGameSource = Instantiate(soundFXObject);
        winGameSource.clip = winGameClip;
        winGameSource.volume = 1f;
        winGameSource.Play();
        float winWaveClipLength = winGameSource.clip.length;
        Destroy(winGameSource.gameObject, winWaveClipLength);
    }
    public void PlayBomb(){
        bombSource = Instantiate(soundFXObject);
        bombSource.clip = bombClip;
        bombSource.volume = 1f;
        bombSource.Play();
        float bombClipLength = bombSource.clip.length;
        Destroy(bombSource.gameObject, bombClipLength);
    }
    public void PlayTowerHit(){
        towerHitSource = Instantiate(soundFXObject);
        towerHitSource.clip = towerHitClip;
        towerHitSource.volume = 1f;
        towerHitSource.Play();
        float towerClipLength = towerHitSource.clip.length;
        Destroy(towerHitSource.gameObject, towerClipLength);
    }


    public void StopSound(AudioSource source)
    {
        if (source != null)
        {
            
            source.Stop();
            
            Destroy(source.gameObject);
            activeAudioSources.Remove(source);
            
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
    public void ChangeVolume(AudioSource audioSource, float volume)
    {
        audioSource.volume = volume;
    }
}
