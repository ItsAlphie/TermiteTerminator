using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    private int damage = 2;
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
        damageCoroutine = StartCoroutine(RepeatedDamage(other.gameObject));
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (damageCoroutine != null && other.CompareTag("Enemy"))
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator RepeatedDamage(GameObject collidedObject)
    {   
        HealthController healthController = collidedObject.GetComponent<HealthController>();

        if(collidedObject.tag == "Enemy"){
            BasicEnemy enemy = collidedObject.GetComponent<BasicEnemy>();
                if(enemy.GetComponent<BasicEnemy>().type == 2){
                    damage *= 2;}
            }

        
        while(collidedObject != null && healthController.currHealth > 0){
            healthController.takeDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
        
        damageCoroutine = null;
    }

}
