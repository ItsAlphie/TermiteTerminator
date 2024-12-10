using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class HealthController : MonoBehaviour
{
    public int currHealth;
    [SerializeField] protected int totalHealth;
    [SerializeField] protected UnityEvent<int, int> onHeal;
    [SerializeField] protected UnityEvent<int, int> onDamage;
    [SerializeField] protected UnityEvent onDied;


    public abstract void die();
    
    protected virtual void Start(){
        currHealth = totalHealth;
    }

    public void takeDamage(int damage)
    {
        currHealth -= damage;
        onDamage.Invoke(currHealth, totalHealth);
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
        onHeal.Invoke(currHealth, totalHealth);
        
    }

    public void DoubleHealth(){
        totalHealth = 2*totalHealth;
        currHealth = 2*currHealth;
    }

}
