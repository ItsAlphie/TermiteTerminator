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
    Transform m_transform;
    public Vector2 targetPosition;

    void Start() {}

    void Update() {}

    private void Awake(){
        m_transform = GetComponent<Transform>();
    }

    public void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        if (startPos != endPos)
        {
            laserAudioSource = SoundController.instance.PlaySoundFXClip(shootSoundclip, transform, 0.2f);
        
            StartCoroutine(StopSoundAfterDelay(laserAudioSource, 0.1f));
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
    }
    private IEnumerator StopSoundAfterDelay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (source != null)
        {
            SoundController.instance.StopSound(source);
        }
    }


}
