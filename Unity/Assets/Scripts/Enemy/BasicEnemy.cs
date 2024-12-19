using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{   
    [SerializeField] protected Transform[] Points;
    [SerializeField] protected float moveSpeed = 0.5f;

    private float freezeMultiplier = 1f;

    [SerializeField] protected int pointsIndex;

    protected bool alive = true;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool Alive { get => alive; set => alive = value; }
    public int CoinDrop { get => coinDrop; set => coinDrop = value; }

    [SerializeField] public int type = 0; // 0 = normal, 1 = fire, 2 = ??, 3 = ??

    private int environment = 0;

    [SerializeField] private int coinDrop = 2;

    // Start is called before the first frame update
    void Start()
    {

        environment = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetEnvironment();
        if(environment == type && environment != 0){
            gameObject.GetComponent<EnemyHealthController>().DoubleHealth();
        }

        transform.position = Points[pointsIndex].transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(pointsIndex <= Points.Length - 1){
            transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, freezeMultiplier*moveSpeed * Time.deltaTime);
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
            EnterFreezeSpell(other);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Spell"){
            ExitFreezeSpell();
        }
    }

    void EnterFreezeSpell(Collider2D other){
        freezeMultiplier = 0.1f;
    }

    void ExitFreezeSpell(){
        freezeMultiplier = 1f;
    }
}

