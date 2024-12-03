using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthController : HealthController
{
    [SerializeField] GameObject healthDisplay;
    // Implement the abstract method die()
    protected override void die()
    {
        gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Broken;
        InventoryManager.Instance.moveToBroken(gameObject);
        // Make object inactive
        // Take tower out of inventory
        // Send kill message
    }

    protected override void Start()
    {
        base.Start();
    }
    
    void Update(){
        if(currHealth > 0){
            InventoryManager.Instance.removeFromBroken(gameObject);
        }
    }

}
