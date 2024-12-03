using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class HealthController : MonoBehaviour
{
    public int currHealth;
    [SerializeField] protected int totalHealth;
    [SerializeField] private UnityEvent onHeal;
    [SerializeField] private UnityEvent onDamage;


    protected abstract void die();
    
    protected virtual void Start(){
        currHealth = totalHealth;
    }

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
