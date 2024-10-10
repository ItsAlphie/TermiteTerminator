using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour
{   
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float lifespan = 10f;

    private GameObject nearestEnemy = null;

    private Vector2 targetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    
    }

    // Update is called once per frame
    void Update()
    {
       MoveTowardsNearestEnemy();

    }

     private void MoveTowardsNearestEnemy(){
        
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
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            transform.position += projectileSpeed * Time.deltaTime * direction;
        }

        
     }

      void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("hit1");
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hit2");
            Destroy(gameObject);
        }
      }
}
