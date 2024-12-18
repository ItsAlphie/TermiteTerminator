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
        PointsController.globalPointsController.AddPoints(totalHealth);
        enemy.MoveSpeed = 0;
        enemy.Alive = false;
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        EnemySpawner.Instance.enemyList.Remove(gameObject);
        UIManager.Instance.showCoinPopUp(enemy.gameObject.transform.position, enemy.CoinDrop, true);
        MoneyManager.Instance.addMoney(enemy.CoinDrop);
        animator.SetBool("isDead", true);
        Destroy(gameObject, 3);
        
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {   
        base.Start();
        enemy = gameObject.GetComponent<BasicEnemy>();
        enemySprite = gameObject.transform.GetChild(0).gameObject;
        animator = enemySprite.GetComponent<Animator>();
        //pointsController = GameObject.FindGameObjectWithTag("PointsController")?.GetComponent<PointsController>();
    }

  
}
