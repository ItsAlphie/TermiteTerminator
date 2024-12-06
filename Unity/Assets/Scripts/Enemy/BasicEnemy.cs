using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{   
    [SerializeField] protected Transform[] Points;
    [SerializeField] protected float moveSpeed = 0.5f;

    [SerializeField] protected int pointsIndex;

    protected bool alive = true;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool Alive { get => alive; set => alive = value; }

    [SerializeField] public int type = 0; // 0 = normal, 1 = fire, 2 = ??, 3 = ??

    private int environment = 0;

    // Start is called before the first frame update
    void Start()
    {

        environment = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetEnvironment();
        if(environment == type && environment != 0){
            gameObject.GetComponent<EnemyHealthController>().DoubleHealth();
            //to do: add visual effect to differentiate boosted enemies
        }

        transform.position = Points[pointsIndex].transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(pointsIndex <= Points.Length - 1){
            transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);
            if(transform.position == Points[pointsIndex].transform.position){
                pointsIndex+=1;
            }
        }
        else{
            if(!LevelManager.Instance.GameOver){
                LevelManager.Instance.TriggerGameOver();
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Spell"){
            moveSpeed = 0.1f;
            Debug.Log("Slowed down");
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Spell"){
            moveSpeed = 0.5f;
            Debug.Log("Speed up");
        }
    }

}

