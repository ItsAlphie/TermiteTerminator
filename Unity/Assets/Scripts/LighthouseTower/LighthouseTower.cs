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
    private float thickness = 0.2f;
    private float boostedThickness = 0.4f;
    private LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        
        laser = gameObject.transform.GetChild(0).gameObject;
        lineRenderer = laser.GetComponent<LineRenderer>();
        hitpoint = gameObject.transform.GetChild(1).gameObject;
        targetPosition = transform.position;

        //Setting the laser thickness to thin for the start
        
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
    
        ShootLaser();
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> enemies = EnemySpawner.Instance.enemyList;

        nearestEnemy = findNearestEnemy();
        //Checking if boosted and adjusting the laser thickness 
        if(Booster){
            lineRenderer.startWidth = boostedThickness;
            lineRenderer.endWidth = boostedThickness;
        }
        else{
            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;
        }
        
        if(nearestEnemy != null && nearestEnemy.GetComponent<BasicEnemy>().Alive){
            
            getPositionOfNearestEnemy();
            ShootLaser();
        }
        else{
            targetPosition = transform.position;
            ShootLaser();
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
