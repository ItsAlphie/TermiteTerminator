using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseTower : BasicTower
{
    GameObject laser;

    private Vector3 enemyDirection = Vector3.zero;
    private GameObject nearestEnemy = null;
    private Vector2 targetPosition;


    // Start is called before the first frame update
    void Start()
    {
         laser = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> enemies = EnemySpawner.enemyList;

        nearestEnemy = findNearestEnemy();
        
        if(nearestEnemy != null && nearestEnemy.GetComponent<BasicEnemy>().Alive){
            Debug.Log("grdfss");
            getPositionOfNearestEnemy();
            ShootLaser();
        }
        else{
            targetPosition = transform.position;
        }
    }
    void ShootLaser(){
        laser.GetComponent<LaserProjectile>().Draw2DRay(transform.position, targetPosition);        
    }
    
    private void getPositionOfNearestEnemy(){
        targetPosition = nearestEnemy.transform.position;
        Debug.Log("dhjks");
     }
     
}
