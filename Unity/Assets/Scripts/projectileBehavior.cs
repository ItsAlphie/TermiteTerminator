using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour
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

        if (nearestEnemy != null)
        {
            enemyDirection = (nearestEnemy.transform.position - transform.position).normalized;
        }
        

        
     }

      void OnCollisionEnter2D(Collision2D collision){
        
        if (collision.gameObject.tag == "Enemy")
        {   
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null){
                enemy.takeDamage(damage);
            }

            Destroy(gameObject);
        }
        
      }
}
