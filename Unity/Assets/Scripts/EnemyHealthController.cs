using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthController
{
    private Animator animator;
    private GameObject enemySpriteObject;

    protected override void die()
    {
        gameObject.GetComponent<BasicEnemy>().MoveSpeed = 0;
        Destroy(gameObject.GetComponent<CircleCollider2D>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        EnemySpawner.enemyList.Remove(gameObject);
        animator.SetBool("isDead", true);
        Destroy(gameObject, 3);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemySpriteObject = gameObject.transform.GetChild(0).gameObject;
        animator = enemySpriteObject.GetComponent<Animator>();
        currHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
