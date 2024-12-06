using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthController : HealthController
{
    [SerializeField] GameObject healthDisplay;
    // Implement the abstract method die()
    public override void die()
    {
        gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Broken;
        InventoryManager.Instance.moveToBroken(gameObject);
        CommunicationController.Instance.SendMsg("k", gameObject.GetComponent<BasicTower>());

        // Make object inactive
        // Take tower out of inventory
        // Send kill message
    }

    protected override void Start()
    {
        base.Start();
    }
    
    void Update(){
    }

    public void repair(){
        gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Bought;
        InventoryManager.Instance.removeFromBroken(gameObject);
        CommunicationController.Instance.SendMsg("r", gameObject.GetComponent<BasicTower>());
    }
}
