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
        firepoint = transform.Find("Firepoint");
        charged = false;
        
    }

   
    void Update()
    {
        
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        
        Vector2 direction = (mousePosition - firepoint.position).normalized;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firepoint.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject pulseProjectile = Instantiate(pulsePrefab, firepoint.position, firepoint.rotation);
        }
    }   
    void OnMouseDown(){
        mousePos = Input.mousePosition;
        Debug.Log(mousePos);
    }
}
