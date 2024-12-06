using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.VisualScripting;

public partial class BasicTower : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject BoostProjectilePrefab;
    [SerializeField] public AudioClip projectileClip;
    [SerializeField] public AudioClip boostedClip;
    [SerializeField] public AudioClip placeClip;
    public AudioSource placingAudioSource;  
    public AudioSource projectileAudioSource;
    public AudioSource boostedAudioSource;
    
    List<GameObject> enemies;
    [SerializeField] private float shootSpeed;
    [SerializeField] public bool boosted = false;
    private float timeLeft;
  
    public enum TowerState { Broken, Bought, Available};
    TowerState state = TowerState.Available;
    public IPAddress IP;

    public TowerState State { get => state; set => state = value; }

    // Start is called before the first frame update
    void Awake()
    {
        timeLeft = shootSpeed;
        placingAudioSource = SoundController.instance.PlaySoundFXClip(placeClip, transform, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {   
        if(State == TowerState.Bought){
            if(boosted){
                shoot();
                
            }
            
            
            
        }
        
    }

    void shoot(){
        updateEnemyList();
        if(enemies.Count != 0){            
                GameObject nearestEnemy = findNearestEnemy();
                projectileAudioSource = SoundController.instance.PlaySoundFXClip(projectileClip, transform, 0.5f);
                GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity); 
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>()); 
                projectile.GetComponent<Projectile>().initialize(nearestEnemy);
                timeLeft = shootSpeed;              
        }
        boosted = false;
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
    private IEnumerator StopSoundAfterDelay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (source != null)
        {
            SoundController.instance.StopSound(source);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("PathCollider")){
            gameObject.GetComponent<TowerHealthController>().die();
        }
    }
}
