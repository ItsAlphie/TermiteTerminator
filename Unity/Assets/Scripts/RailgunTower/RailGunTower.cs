using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunTower : BasicTower
{
    private Vector2 targetPosition;
    public Transform firepoint; // Position to spawn projectiles
    public GameObject pulsePrefab;    // Projectile prefab
    public bool charged;
    private Vector2 mousePos;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Railgun started");
        firepoint = transform.Find("Firepoint");
        charged = false;
        //placingAudioSource = SoundController.instance.PlaySoundFXClip(placeClip, transform, 0.8f);
        
    }

   
    void Update()
    {
        
        /*
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        
        Vector2 direction = (mousePosition - firepoint.position).normalized;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firepoint.rotation = Quaternion.Euler(0, 0, angle);
        */


        /*if (boosted && State == TowerState.Bought)
        {
            firepoint.rotation = transform.rotation * Quaternion.Euler(0, 0, 0);
            GameObject pulseProjectile = Instantiate(pulsePrefab, firepoint.position, firepoint.rotation);
            Debug.Log("tower" + gameObject.GetComponent<Collider2D>());
            projectileAudioSource = SoundController.instance.PlaySoundFXClip(projectileClip, transform, 0.8f);
            Physics2D.IgnoreCollision(pulseProjectile.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            boosted = false;
        }*/
        
        if(boosted)
        {
            
            firepoint.rotation = transform.rotation * Quaternion.Euler(0, 0, 0);
            GameObject pulseProjectile = Instantiate(pulsePrefab, firepoint.position, firepoint.rotation);          
            Physics2D.IgnoreCollision(pulseProjectile.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            boosted = false;
            SoundController.instance.StopAllSounds();
            projectileAudioSource = SoundController.instance.PlaySoundFXClip(projectileClip, transform, 0.8f);
            
        }
    }   
    void OnMouseDown(){
        mousePos = Input.mousePosition;
        Debug.Log(mousePos);
    }
}
