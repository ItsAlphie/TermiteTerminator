using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int damage = 10;

    private GameObject nearestEnemy = null;

    private Vector2 targetPosition;

    private Vector3 enemyDirection = Vector3.zero;

    private GameObject projectileSprite;

    
    public void initialize(GameObject nearestEnemy){
        this.nearestEnemy = nearestEnemy;
    }

    // Start is called before the first frame update
    void Start()
    {
        projectileSprite = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    { 
        if((nearestEnemy != null) && (nearestEnemy.GetComponent<BasicEnemy>().Alive)) {
            getDirectionOfNearestEnemy();
         }
        transform.position += projectileSpeed * Time.deltaTime * enemyDirection;
        if(!projectileSprite.GetComponent<SpriteRenderer>().isVisible){
            Destroy(gameObject);
        }
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
                if(collidedEnemy.GetComponent<BasicEnemy>().type == 1){
                    damage *= 2;
                }
                healthController.takeDamage(damage);
            }
            Destroy(gameObject);
        }
      }
}
