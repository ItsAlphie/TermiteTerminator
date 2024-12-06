using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : BasicEnemy
{
   
    [SerializeField] private GameObject bombPrefab;

    private bool bombDropped = false;


    // Start is called before the first frame update
    void Start()
    {
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
        else{
            if(!LevelManager.Instance.GameOver){
                LevelManager.Instance.TriggerGameOver();
            }
            
        }

        float distance = FindNearestTower();
        if(distance < 2f && !bombDropped){ // drop bomb if enemy is close to tower
            DropBomb();
        }

    }

    void DropBomb(){
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bombDropped = true;
    }


    float FindNearestTower(){
        float closestDistance = 1000000;
        List<GameObject> towers = TowerSpawner.Instance.towers;
        if (towers.Count == 0) return 10000f;
        foreach (GameObject t in towers)
        {
            if(t == null) continue;
            
            float distance = Vector2.Distance(transform.position, t.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                return closestDistance;
            }   
            
            
        }
        return 1000f;
    }
     
}
