using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthController : HealthController
{
    public bool killed = false;
    // Implement the abstract method die()
    protected override void die()
    {
        // Make object inactive
        // Take tower out of inventory
        // Send kill message
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
