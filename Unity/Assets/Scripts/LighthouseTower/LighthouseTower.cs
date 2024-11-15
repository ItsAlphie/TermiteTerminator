using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseTower : BasicTower
{
    GameObject laser;
    GameObject hitpoint;
    private Vector3 enemyDirection = Vector3.zero;
    private GameObject nearestEnemy = null;
    private Vector2 targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        
        laser = gameObject.transform.GetChild(0).gameObject;
        hitpoint = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
{
    List<GameObject> enemies = EnemySpawner.enemyList;
    nearestEnemy = findNearestEnemy();
    
    if(nearestEnemy != null && nearestEnemy.GetComponent<BasicEnemy>().Alive)
    {
        getPositionOfNearestEnemy();
        Debug.Log("Tower Position: " + transform.position);
        Debug.Log("Target Position: " + targetPosition);
        ShootLaser();
    }
    else
    {
        targetPosition = transform.position; // Set to own position when no enemy
        Debug.Log("No enemy found, target position set to tower's position");
    }
}
    void ShootLaser(){
        laser.GetComponent<Laser>().Draw2DRay(transform.position, targetPosition);
        hitpoint.GetComponent<HitMarker>().MoveHitMarker(targetPosition);
    }
    
    private void getPositionOfNearestEnemy(){
        targetPosition = nearestEnemy.transform.position;
        //Debug.Log("dhjks");
     }
     
}
