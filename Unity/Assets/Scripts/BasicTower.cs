using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;

    [SerializeField] private float shootSpeed;
    
    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = shootSpeed;
    }

    // Update is called once per frame
    void Update()
    {   
        List<GameObject> enemies = EnemySpawner.enemyList;
        if(enemies.Count != 0){
            timeLeft -= Time.deltaTime;
            
        if(timeLeft <= 0){
            Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);  
            timeLeft = shootSpeed;
        }
        }
        
      
    }

    void OnMouseDown(){
        Destroy(gameObject);
    }
}
