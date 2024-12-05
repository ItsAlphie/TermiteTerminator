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
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        Debug.Log("Wind initialized. Direction: " + direction);
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime);
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            StartCoroutine(DelayedDestroy());
        }
        updateVector(direction.x);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetInteger("Number_animation", 1);

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

    IEnumerator DelayedDestroy()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        Debug.Log("Destroying Wind object.");
        Destroy(gameObject);
    }
    public void setDirection(Vector2 calculatedDirection){
        direction = calculatedDirection;
        Debug.Log("Direction set to: "+ direction); 
    }
    private Vector2 updateVector(float xvalue)
    {
        direction.x = xvalue * 1.005f;
        return direction;
    }
}
