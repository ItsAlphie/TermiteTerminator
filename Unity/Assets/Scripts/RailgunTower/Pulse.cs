using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : BasicTower
{
    public Vector2 direction;
    public float speed = 5f;
    public float maxDistance = 10f;
    private int damage = 40;
    private Vector2 startPosition;
   
    
     
    void Start()
    {
        direction = new Vector2(1, 0);
        startPosition = transform.position;
    }
    
    

    void Update()
    {
        direction = updateVector(direction.x);
        transform.Translate(direction * Time.deltaTime);

        float distanceTraveled = Vector2.Distance(startPosition,transform.position);
        if(distanceTraveled >=maxDistance){
            Destroy(gameObject);
        } 
    }
    
    void OnTriggerEnter2D(Collider2D collision){

        GameObject collidedEnemy = collision.gameObject;
        if ((collidedEnemy.tag == "Enemy") && collidedEnemy.GetComponent<BasicEnemy>().Alive)
        {   
            HealthController healthController = collidedEnemy.GetComponent<HealthController>();
            if (collidedEnemy.GetComponent<BasicEnemy>() != null){
                if(collidedEnemy.GetComponent<BasicEnemy>().type == 2){
                    damage *= 2;
                }
                healthController.takeDamage(damage);
            }
            
        }
      }
    
    private Vector2 updateVector(float xvalue)
    {
        direction.x = xvalue * 1.015f;
        return direction;
    }
    
    
   
}
