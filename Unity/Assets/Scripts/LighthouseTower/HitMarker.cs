using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    private int Bdamage = 2;
    
    private Coroutine damageCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveHitMarker(Vector2 hitpoint){
        transform.position = hitpoint;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        

        // Make sure we're colliding with an enemy
        if (other.CompareTag("Enemy"))
        {
            BasicEnemy enemyComponent = other.GetComponent<BasicEnemy>();
            if (enemyComponent != null && enemyComponent.Alive)
            {
                HealthController healthController = other.GetComponent<HealthController>();
                
                if (healthController != null)
                {
                    
                    damageCoroutine = StartCoroutine(RepeatedDamage(enemyComponent, healthController));
                }
            }
            else
            {
                
            }
        }
        else
        {
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (damageCoroutine != null && other.CompareTag("Enemy"))
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
            //Debug.Log("Stopped damaging enemy as it exited trigger area.");
        }
    }

    private IEnumerator RepeatedDamage(BasicEnemy enemy, HealthController healthController)
    {
        // Repeat while the enemy is alive and health controller is valid
        while (enemy.Alive && healthController.currHealth > 0)
        {
            
            if(enemy.GetComponent<BasicEnemy>().type == 2){
                Bdamage *= 2;
                }
            healthController.takeDamage(Bdamage);
            
            yield return new WaitForSeconds(0.5f); // Wait 0.5 seconds before repeating
        }
        
        // Stop the coroutine when the enemy is dead
        damageCoroutine = null;
        
    }

}
