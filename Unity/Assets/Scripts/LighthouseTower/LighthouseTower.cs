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

    bool gameBoost = false;
    public float timer = 0.0f;
    [SerializeField] protected float boosted_duration = 5.0f; 
     
    // Start is called before the first frame update
    void Start(){
        placingAudioSource = SoundController.instance.PlaySoundFXClip(placeClip, transform, 1f);
        laser = gameObject.transform.GetChild(0).gameObject; 
        projectileAudioSource = laser.GetComponent<Laser>().Draw2DRay(transform.position, targetPosition, gameBoost,boostedClip, projectileClip);
        lineRenderer = laser.GetComponent<LineRenderer>();
        hitpoint = gameObject.transform.GetChild(1).gameObject;
        Physics2D.IgnoreCollision(hitpoint.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>()); 
        targetPosition = transform.position;
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
        ShootLaser(gameBoost);
    }


   
    void Update()
    {
        if(State == TowerState.Bought){
            List<GameObject> enemies = EnemySpawner.Instance.enemyList;
            nearestEnemy = findNearestEnemy();
            if (gameBoost == false){
                lineRenderer.startWidth = thickness;
                lineRenderer.endWidth = thickness;
            }
            else{
                lineRenderer.startWidth = boostedThickness;
                lineRenderer.endWidth = boostedThickness;
            }

            if (nearestEnemy != null && nearestEnemy.GetComponent<BasicEnemy>().Alive){
                getPositionOfNearestEnemy();
                // unhide laser
                ShootLaser(gameBoost);
            }
            else{
                targetPosition = transform.position;
                ShootLaser(gameBoost);
                // hide laser
            }
        }
        // hide laser

        //// Boost logic ////
        // When receiving a boost, start a timer (reset if needed)
        if(boosted){
            gameBoost = true;
            timer = 0;
            boosted = false;
        }

        // Boosted beam returns to normal automatically, after 'boosted_duration' seconds of not receiving boosts
        if (gameBoost){
            if (timer > boosted_duration){
                timer = 0;
                gameBoost = false;
            }
            timer += UnityEngine.Time.deltaTime;
        }
    }

    void ShootLaser(bool isBoosted){
        // Pass boostedClip as the fourth parameter.
        projectileAudioSource = laser.GetComponent<Laser>().Draw2DRay(transform.position, targetPosition, isBoosted, boostedClip,projectileClip);
        hitpoint.GetComponent<HitMarker>().MoveHitMarker(targetPosition);
    }

    
    private void getPositionOfNearestEnemy(){
        targetPosition = nearestEnemy.transform.position;
    }
     
    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("light" + col.gameObject.tag);

        if(col.gameObject.CompareTag("PathCollider")){
            Debug.Log("Tower placed on path");
            gameObject.GetComponent<TowerHealthController>().die();
        }
    }
}
