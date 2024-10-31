using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{   
[SerializeField] Transform[] Points;
[SerializeField] private float moveSpeed = 0.5f;

[SerializeField] private int pointsIndex;

private bool alive = true;

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
    }

    

}

