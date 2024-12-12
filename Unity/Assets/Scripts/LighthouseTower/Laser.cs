using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //
    public Transform laserFirePoint;
    [SerializeField] private AudioClip shootSoundclip;
    public LineRenderer m_lineRenderer;
    public AudioSource laserAudioSource; 
    public AudioSource boostAudioSource; 
    Transform m_transform;
    public bool firstBoost = true;
    public Vector2 targetPosition;

    void Start() {}

    void Update() {}

    private void Awake(){
        m_transform = GetComponent<Transform>();
    }

    public AudioSource Draw2DRay(Vector2 startPos, Vector2 endPos, bool boost, AudioClip boostClip, AudioClip shootSoundclip)
    {

        if(boost==false){
            firstBoost = true;
            if(laserAudioSource == null ){
                laserAudioSource = SoundController.instance.PlaySoundFXClip(shootSoundclip, transform, 0.2f);
            }
            else{
                SoundController.instance.ChangeVolume(laserAudioSource, 0.2f);
            }
            
        }
        if(boost == true){
            if(firstBoost == true){
                boostAudioSource = SoundController.instance.PlaySoundFXClip(boostClip, transform, 0.8f);
            }
            firstBoost = false;
            if(laserAudioSource == null){
                laserAudioSource = SoundController.instance.PlaySoundFXClip(shootSoundclip, transform, 0.8f);
            }
            else{
                SoundController.instance.ChangeVolume(laserAudioSource, 0.8f);
            }
            
        }
        
        if (startPos != endPos)
        {             
            //StartCoroutine(StopSoundAfterDelay(laserAudioSource, 0.1f));
        }
        else
        {
            if (laserAudioSource != null && laserAudioSource.gameObject != null)
            {
                
                SoundController.instance.StopSound(laserAudioSource);
                
                laserAudioSource = null;
                
            }
           
        }


        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
        return laserAudioSource;
    }
    


}
