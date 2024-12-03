using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthController : HealthController
{
    public bool killed = false;
    [SerializeField] GameObject healthDisplay;
    // Implement the abstract method die()
    protected override void die()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Broken;
        // Make object inactive
        // Take tower out of inventory
        // Send kill message
    }

    protected override void Start()
    {
        base.Start();
    }
    
    

}
