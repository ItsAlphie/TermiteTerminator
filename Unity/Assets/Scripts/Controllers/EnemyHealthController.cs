using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthController
{
    private Animator animator;
    private GameObject enemySprite;

    BasicEnemy enemy;

    protected override void die()
    {
        enemy.MoveSpeed = 0;
        enemy.Alive = false;
        Destroy(gameObject.GetComponent<CircleCollider2D>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        EnemySpawner.Instance.enemyList.Remove(gameObject);
        animator.SetBool("isDead", true);
        Destroy(gameObject, 3);
    }

    // Start is called before the first frame update
    void Start()
    {   
        enemy = gameObject.GetComponent<BasicEnemy>();
        enemySprite = gameObject.transform.GetChild(0).gameObject;
        animator = enemySprite.GetComponent<Animator>();
        currHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
