using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public partial class BasicTower : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject BoostProjectilePrefab;
    List<GameObject> enemies;
    [SerializeField] private float shootSpeed;
    public bool boosted = false;
    private float timeLeft;
    public enum TowerState { Broken, Bought, Available};
    TowerState state = TowerState.Available;
    [SerializeField] public IPAddress IP;

    public TowerState State { get => state; set => state = value; }

    // Start is called before the first frame update
    void Awake()
    {
        timeLeft = shootSpeed;
    }

    // Update is called once per frame
    void Update()
    {   
        if(State == TowerState.Bought){
            shoot();
        }
        
    }

    void shoot(){
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
    }

    void OnMouseDown(){
        gameObject.GetComponent<TowerHealthController>().takeDamage(10);
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
