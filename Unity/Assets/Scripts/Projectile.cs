using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float lifespan = 10f;

    [SerializeField] private int damage = 10;


    private GameObject nearestEnemy = null;

    private Vector2 targetPosition;

    private Vector3 enemyDirection = Vector3.zero;
    
    public void initialize(GameObject nearestEnemy){
        this.nearestEnemy = nearestEnemy;
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    
    }

    // Update is called once per frame
    void Update()
    { 
        if((nearestEnemy != null) && (nearestEnemy.GetComponent<BasicEnemy>().Alive)) {
            getDirectionOfNearestEnemy();
         }
        transform.position += projectileSpeed * Time.deltaTime * enemyDirection;
    }    

     private void getDirectionOfNearestEnemy(){
        enemyDirection = (nearestEnemy.transform.position - transform.position).normalized;
     }

      void OnCollisionEnter2D(Collision2D collision){
        GameObject collidedEnemy = collision.gameObject;
        if ((collidedEnemy.tag == "Enemy") && collidedEnemy.GetComponent<BasicEnemy>().Alive)
        {   
            HealthController healthController = collidedEnemy.GetComponent<HealthController>();
            if (collidedEnemy.GetComponent<BasicEnemy>() != null){
                healthController.takeDamage(damage);
            }
            Destroy(gameObject);
        }
      }
}
