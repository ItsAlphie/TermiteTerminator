using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthController
{
    private Animator animator;
    private GameObject enemySprite;

    BasicEnemy enemy;

    public override void die()
    {
        enemy.MoveSpeed = 0;
        Debug.Log(enemy.MoveSpeed);
        enemy.Alive = false;
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        EnemySpawner.Instance.enemyList.Remove(gameObject);
        UIManager.Instance.showCoinPopUp(enemy.gameObject.transform.position, enemy.CoinDrop, true);
        MoneyManager.Instance.addMoney(enemy.CoinDrop);
        animator.SetBool("isDead", true);
        Destroy(gameObject, 3);
        Debug.Log("kut");
    }

    // Start is called before the first frame update
    protected override void Start()
    {   
        base.Start();
        enemy = gameObject.GetComponent<BasicEnemy>();
        enemySprite = gameObject.transform.GetChild(0).gameObject;
        animator = enemySprite.GetComponent<Animator>();
    }

  
}
