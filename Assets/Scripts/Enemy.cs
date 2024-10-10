using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
[SerializeField] Transform[] Points;
[SerializeField] private float moveSpeed;

[SerializeField] private int pointsIndex;



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
