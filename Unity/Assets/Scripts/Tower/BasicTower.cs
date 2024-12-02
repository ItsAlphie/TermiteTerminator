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
    public bool boosted = false;
    private float timeLeft;
    [SerializeField] public IPAddress IP;
    // Start is called before the first frame update
    void Awake()
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
                if(boosted){
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

        if(Input.GetMouseButtonDown(2)){
            Debug.Log("Middle click");
            gameObject.GetComponent<ShopManager>().buyItem();

        }
        
    }

    void OnMouseDown(){
        gameObject.GetComponent<ShopManager>().sellItem();
        Debug.Log("mouse down");
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

    public IPAddress GetIP(){
        return IP;
    }

    public void SetIP(IPAddress newIP){
        IP = newIP;
    }
}
