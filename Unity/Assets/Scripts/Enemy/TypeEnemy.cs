using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEnemy : BasicEnemy
{

    //these enemies should receive double damage by certain towers, where to implement?

    private int type = 1; //To do: change this to an enum?

    private int environment = 0; //To do: change this to an enum?

    // Start is called before the first frame update
    void Start()
    {
        environment = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetEnvironment();
        if(environment == type ){
            gameObject.GetComponent<EnemyHealthController>().DoubleHealth();
            //to do: add visual effect to differentiate boosted enemies
        }

        transform.position = Points[pointsIndex].transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        if(pointsIndex <= Points.Length - 1){  //update position of the enemy
            transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);
            if(transform.position == Points[pointsIndex].transform.position){
                pointsIndex+=1;
            }
        }
    }
}
