using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthController
{
    private Animator animator;
    private SpriteRenderer hatSpriteRenderer;
    private GameObject enemySprite;
    BasicEnemy enemy;

    [SerializeField] private int healthClass; //3:120-90 2:90-60 1:60-30 0:30-0 

    [SerializeField] public BasicEnemy nextForm;

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
        hatSpriteRenderer = enemy.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
    }

    public override void takeDamage(int damage)
    {
        Debug.Log("takeDamage");

        currHealth -= damage;
        onDamage.Invoke(currHealth, totalHealth);
        if(currHealth <= 0){
            currHealth = 0;
            die();
        }



        if(currHealth >= 120){ // 6
            healthClass = 4;
            //change enemy class: animator, sprite and speed
            Debug.Log("health 120+");
            Debug.Log(nextForm.name);
        }
        else if(currHealth >= 90){ // 5
            if(healthClass == 4){
                animator.runtimeAnimatorController = nextForm.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController;
                //hatSpriteRenderer.sprite = nextForm.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
                enemy.transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(0.2f,0.2f,0.2f);
                enemy.MoveSpeed = nextForm.GetComponent<BasicEnemy>().MoveSpeed;
                nextForm = nextForm.GetComponent<EnemyHealthController>().nextForm;
                Debug.Log(nextForm.name);
            }
            healthClass = 3;
            Debug.Log("health 90+");
        }
        else if(currHealth >= 60){ //4
            if(healthClass == 3){
                animator.runtimeAnimatorController = nextForm.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController;
                //hatSpriteRenderer.sprite = nextForm.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
                enemy.transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(0.15f,0.15f,0.15f);
                enemy.MoveSpeed = nextForm.GetComponent<BasicEnemy>().MoveSpeed;
                nextForm = nextForm.GetComponent<EnemyHealthController>().nextForm;
                Debug.Log(nextForm.name);
            }
            healthClass = 2;
            Debug.Log("health 60+");
        }
        else if(currHealth >= 30){ //3
            if(healthClass == 2){
                animator.runtimeAnimatorController = nextForm.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController;
                //hatSpriteRenderer.sprite = nextForm.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
                enemy.transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(0.10f,0.10f,0.10f);
                enemy.MoveSpeed = nextForm.GetComponent<BasicEnemy>().MoveSpeed;
                nextForm = nextForm.GetComponent<EnemyHealthController>().nextForm;
                Debug.Log(nextForm.name);
            }
            healthClass = 1;
            Debug.Log("health 30+");
        }
        else{
            if(healthClass == 1){ //2
                animator.runtimeAnimatorController = nextForm.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController;
                //hatSpriteRenderer.sprite = nextForm.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
                enemy.MoveSpeed = nextForm.GetComponent<BasicEnemy>().MoveSpeed;
                nextForm = nextForm.GetComponent<EnemyHealthController>().nextForm;
                Destroy(gameObject.transform.GetChild(1).gameObject);
                Debug.Log(nextForm.name);
            }
            healthClass = 0;
            Debug.Log("health 0+");
        }

    }

}
