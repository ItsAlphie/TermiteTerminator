using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    private float timeUntilSpawn;

    // Start is called before the first frame update
    void Start()
    {
        setTimeUntilSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime; //reduce time by amount of time that has passed in a frame
        if(timeUntilSpawn <= 0){
            Instantiate(EnemyPrefab, SpawnPoint.transform.position, Quaternion.identity);
            setTimeUntilSpawn();
        }
    }

    private void setTimeUntilSpawn(){
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
