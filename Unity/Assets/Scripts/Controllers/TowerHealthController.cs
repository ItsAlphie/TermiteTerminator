using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthController : HealthController
{
    [SerializeField] GameObject healthDisplay;
    
    public override void takeDamage(int damage)
    {
        currHealth -= damage;
        onDamage.Invoke(currHealth, totalHealth);
        HintManager.Instance.TowerDamaged(gameObject);
        if(currHealth <= 0){
            currHealth = 0;
            die();
        }
    }

    // Implement the abstract method die()
    public override void die()
    {   
        PointsController.globalPointsController.SubtractPointsForTower();
        gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Broken;
        InventoryManager.Instance.moveToBroken(gameObject);
        CommunicationController.Instance.SendMsg("k", gameObject.GetComponent<BasicTower>());
        onDied.Invoke();
        HintManager.Instance.TowerDowned(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        //pointsController = GameObject.FindGameObjectWithTag("PointsController")?.GetComponent<PointsController>();
    }
    
    void Update(){
    }

    public void repair(){
        gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Bought;
        InventoryManager.Instance.removeFromBroken(gameObject);
        CommunicationController.Instance.SendMsg("r", gameObject.GetComponent<BasicTower>());
        //pointsController = GameObject.FindGameObjectWithTag("PointsController")?.GetComponent<PointsController>();
    
    }
}
