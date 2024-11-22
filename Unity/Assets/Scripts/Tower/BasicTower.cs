using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class BasicTower : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject BoostProjectilePrefab;
    List<GameObject> enemies;
    [SerializeField] private float shootSpeed;
    public bool Booster = false;
    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = shootSpeed;
    }

    // Update is called once per frame
    void Update()
    {   
        updateEnemyList();
        if(enemies.Count != 0){
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0){
                GameObject nearestEnemy = findNearestEnemy();
                // TODO: get boost from Controller
                if(Booster){
                    print("Boosted Projectile");
                    GameObject projectile = Instantiate(BoostProjectilePrefab, transform.position, Quaternion.identity);
                    projectile.GetComponent<Projectile>().initialize(nearestEnemy);
                    timeLeft = shootSpeed;
                }
                else{
                    GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);  
                    projectile.GetComponent<Projectile>().initialize(nearestEnemy);
                    timeLeft = shootSpeed;
                }
            }
        }
        
    }

    void OnMouseDown(){
        Destroy(gameObject);
    }

    public GameObject findNearestEnemy(){
        float closestDistance = 1000000;
        updateEnemyList();
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

    public void TakeDamage(int damage){
        Destroy(gameObject);
    }

    public void updateEnemyList(){
        enemies = EnemySpawner.Instance.enemyList;
    }
}
