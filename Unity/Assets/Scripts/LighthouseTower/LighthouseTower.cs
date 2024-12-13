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
        placingAudioSource = SoundController.instance.PlaySoundFXClip(placeClip, transform, 1f);
        laser = gameObject.transform.GetChild(0).gameObject; 
        projectileAudioSource = laser.GetComponent<Laser>().Draw2DRay(transform.position, targetPosition, boosted,boostedClip, projectileClip);
        lineRenderer = laser.GetComponent<LineRenderer>();
        hitpoint = gameObject.transform.GetChild(1).gameObject;
        Physics2D.IgnoreCollision(hitpoint.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>()); 
        targetPosition = transform.position;
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
        ShootLaser(boosted);
    }


   
    void Update()
    {
        if(State == TowerState.Bought){
            List<GameObject> enemies = EnemySpawner.Instance.enemyList;
            nearestEnemy = findNearestEnemy();
            if (boosted == false)
            {
                lineRenderer.startWidth = thickness;
                lineRenderer.endWidth = thickness;
            }
            else
            {
                lineRenderer.startWidth = boostedThickness;
                lineRenderer.endWidth = boostedThickness;
            }

            if (nearestEnemy != null && nearestEnemy.GetComponent<BasicEnemy>().Alive)
            {
                getPositionOfNearestEnemy();
                ShootLaser(boosted);
            }
            else
            {
                targetPosition = transform.position;
                ShootLaser(boosted);
            }
        }
    }

    void ShootLaser(bool isBoosted)
    {
        // Pass boostedClip as the fourth parameter.
        projectileAudioSource = laser.GetComponent<Laser>().Draw2DRay(transform.position, targetPosition, isBoosted, boostedClip,projectileClip);
        hitpoint.GetComponent<HitMarker>().MoveHitMarker(targetPosition);
    }

    
    private void getPositionOfNearestEnemy(){
        targetPosition = nearestEnemy.transform.position;
    }
     
    void OnTriggerEnter2D(Collider2D col){
    Debug.Log("lighth" + col.gameObject.tag);

    if(col.gameObject.CompareTag("PathCollider")){
        Debug.Log("Tower placed on path");
        gameObject.GetComponent<TowerHealthController>().die();
    }
}
}
