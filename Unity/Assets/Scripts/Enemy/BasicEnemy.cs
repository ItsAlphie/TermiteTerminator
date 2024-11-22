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


    // Start is called before the first frame update
    void Start()
    {
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

    

}

