using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthController : MonoBehaviour
{
    protected int currHealth;
    [SerializeField] protected int totalHealth;

    protected abstract void die();

    public void takeDamage(int damage)
    {
        currHealth -= damage;
        if(currHealth <= 0){
            currHealth = 0;
            die();
        }
    }

    public void heal(int healAmount){
        if(currHealth == totalHealth){
            return;
        }
        else{
            if((currHealth += healAmount) > totalHealth){
                currHealth = totalHealth;
            };
        }
        
    }

    public void DoubleHealth(){
        totalHealth = 2*totalHealth;
        currHealth = 2*currHealth;
    }

}
