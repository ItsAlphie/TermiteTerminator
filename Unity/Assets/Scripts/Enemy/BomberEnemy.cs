using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BomberEnemy : BasicEnemy
{
   
    [SerializeField] private GameObject bombPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Points[pointsIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        if (checkTowersPresent()){
            Vector3 towerPos = FindNearestTower();
            towerPos.x = towerPos.x - 0.913f;
            towerPos.y = towerPos.y + 0.913f;
            transform.position = Vector2.MoveTowards(transform.position, towerPos, moveSpeed * Time.deltaTime);
            if(transform.position == towerPos){
                DropBomb();
                pointsIndex = Points.Length-1;
            }
        }
        else{
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
        }

    }

    void DropBomb(){
        if(!transform.GetChild(1).GetComponent<BombScript>().Activated)
        transform.GetChild(1).GetComponent<BombScript>().initializeExplosion();
    }


    Vector3 FindNearestTower(){
        float closestDistance = 1000000;
        List<GameObject> towers = TowerSpawner.Instance.towers;
        foreach (GameObject t in towers)
        {
            if(t == null || t.GetComponent<BasicTower>().State != BasicTower.TowerState.Bought) continue;
            
            float distance = Vector2.Distance(transform.position, t.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance; 
                return t.transform.position;
            }   
            
            
        }
        return Vector3.zero;
    }
     
     bool checkTowersPresent(){
        List<GameObject> towers = TowerSpawner.Instance.towers;
        foreach (GameObject t in towers){
            if (t.GetComponent<BasicTower>().State == BasicTower.TowerState.Bought){
                return true;
            }
        }
        return false;
     }
}
