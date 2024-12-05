using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTower : BasicTower
{
    private Vector2 targetPosition;
    public Transform firepoint; 
    public GameObject windPrefab; 
    public bool charged;
    private GameObject nearestEnemy;
    public Vector2 direction;

    
    void Start()
    {
        firepoint = transform.Find("Firepoint");
        charged = false;
    }

    void Update()
    {
        nearestEnemy = findNearestEnemy();
        if (nearestEnemy != null)
        {
            direction = (nearestEnemy.transform.position - firepoint.position).normalized;
           // Debug.Log("direction = "+ direction);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firepoint.rotation = Quaternion.Euler(0, 0, angle);
        }

       
        if (Input.GetButtonDown("Fire1") && nearestEnemy != null)
        {
            GameObject windProjectile = Instantiate(windPrefab, firepoint.position, firepoint.rotation);
            Wind windScript = windProjectile.GetComponentInChildren<Wind>();

            if (windScript != null)
            {
                Debug.Log("this is the direction = " + direction);
                windScript.setDirection(direction); // Pass the direction
            }
            else
            {
                Debug.LogError("Wind script not found on the instantiated prefab!");
            }
        }
    }
    private GameObject findNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all active enemies in the scene
        float closestDistance = float.MaxValue;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.GetComponent<BasicEnemy>().Alive)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        return closestEnemy;
    }
}
