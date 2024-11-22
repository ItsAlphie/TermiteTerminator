using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthController : HealthController
{
    // Implement the abstract method die()
    protected override void die()
    {
        //TowerSpawner.towers.Remove(gameObject);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
