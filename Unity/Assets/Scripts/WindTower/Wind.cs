using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public Animator animator;
    public Vector2 direction;
    public float maxDistance = 30f;
    private int damage = 20;
    private Vector2 startPosition;

    void Start()
    {
        direction = new Vector2(1, 0);
        startPosition = transform.position;
        animator =  GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Hit", true);
        
        direction = updateVector(direction.x);
        transform.Translate(direction * Time.deltaTime);

        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            StartCoroutine(DelayedDestroy());
        }

        Debug.Log("Animator 'Hit' value: " + animator.GetBool("Hit"));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        direction = Vector2.zero; // Stop movement on collision
        animator.SetInteger("Number_animation", 1);
        Debug.Log(animator.GetBool("Hit"));
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        GameObject collidedEnemy = collision.gameObject;
        if (collidedEnemy.tag == "Enemy" && collidedEnemy.GetComponent<BasicEnemy>().Alive)
        {
            HealthController healthController = collidedEnemy.GetComponent<HealthController>();
            if(collidedEnemy.GetComponent<BasicEnemy>().type == 1){
                damage = damage * 2;
            }
            healthController.takeDamage(damage);
        }

        StartCoroutine(DelayedDestroy());
    }

    private Vector2 updateVector(float xvalue)
    {
        direction.x = xvalue * 1.005f;
        return direction;
    }

    IEnumerator DelayedDestroy()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length); // Wait for the animation length
        Debug.Log("Destroying object: " + gameObject.name);
        Destroy(gameObject);
    }
}
