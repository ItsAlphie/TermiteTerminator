using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{   
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float lifespan = 10f;

    [SerializeField] private int damage = 10;

    private GameObject nearestEnemy = null;

    private Vector2 targetPosition;

    private Vector3 enemyDirection = Vector3.zero;

    private bool enemyFound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    
    }

    // Update is called once per frame
    void Update()
    { 
        if(!enemyFound){
            findNearestEnemy();
        }
            
        else if(enemyFound){

        if(nearestEnemy != null) {
            getDirectionOfNearestEnemy();
         } 
         Debug.Log(enemyDirection.ToString("F2"));
        transform.position += projectileSpeed * Time.deltaTime * enemyDirection;
    }

    }

    private void findNearestEnemy(){
        float closestDistance = 1000000;
        List<GameObject> enemies = EnemySpawner.enemyList;
        if (enemies.Count == 0) return;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy;
                enemyFound = true;
            }
        }

    }

     private void getDirectionOfNearestEnemy(){

        if (nearestEnemy.GetComponent<BasicEnemy>().Alive)
        {
            Debug.Log("bgsrfe");
            enemyDirection = (nearestEnemy.transform.position - transform.position).normalized;
        }
        

        
     }

      void OnCollisionEnter2D(Collision2D collision){
        
        if (collision.gameObject.tag == "Enemy")
        {   
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();
            if (enemy != null){
                healthController.takeDamage(damage);
            }

            Destroy(gameObject);
        }
        
      }
}
