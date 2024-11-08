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
            GameObject nearestEnemy = findNearestEnemy();
            GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);  
            projectile.GetComponent<Projectile>().initialize(nearestEnemy);
            timeLeft = shootSpeed;
        }
        }
        
        
      
    }

    void OnMouseDown(){
        Destroy(gameObject);
    }

    public GameObject findNearestEnemy(){
        float closestDistance = 1000000;
        List<GameObject> enemies = EnemySpawner.enemyList;
        if (enemies.Count == 0) return null;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                return enemy;
            }
        }
        return null;
    }
}
