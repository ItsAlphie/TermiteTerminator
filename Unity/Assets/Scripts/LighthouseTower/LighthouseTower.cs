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
        placingAudioSource = SoundController.instance.PlaySoundFXClip(placeClip, transform, 0.8f);
        laser = gameObject.transform.GetChild(0).gameObject; 
        projectileAudioSource = laser.GetComponent<Laser>().Draw2DRay(transform.position, targetPosition, boosted,boostedClip, projectileClip);
        lineRenderer = laser.GetComponent<LineRenderer>();
        hitpoint = gameObject.transform.GetChild(1).gameObject;
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
            Debug.Log("Boosted value in Update: " + boosted);
            if (boosted == false)
            {
                Debug.Log("not boosted");
                lineRenderer.startWidth = thickness;
                lineRenderer.endWidth = thickness;
            }
            else
            {
                Debug.Log("i am boosted");
                lineRenderer.startWidth = boostedThickness;
                lineRenderer.endWidth = boostedThickness;
            }

            if (nearestEnemy != null && nearestEnemy.GetComponent<BasicEnemy>().Alive)
            {
                Debug.Log("Shooting Laser at enemy.");
                getPositionOfNearestEnemy();
                ShootLaser(boosted);
            }
            else
            {
                Debug.Log("No enemy. Laser stationary.");
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
     
}
