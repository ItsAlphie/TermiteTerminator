using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] public Sprite[] healthDisplaySprites;
    
    void Start(){
        gameObject.GetComponent<SpriteRenderer>().sprite = healthDisplaySprites[4];
    }
    public void updateHealthDisplay(int currHealth, int totalHealth){
        if(currHealth > 0.75*totalHealth ){
            gameObject.GetComponent<SpriteRenderer>().sprite = healthDisplaySprites[4];
        }
        else if(currHealth > 0.5*totalHealth && currHealth <= 0.75*totalHealth){
            gameObject.GetComponent<SpriteRenderer>().sprite = healthDisplaySprites[3];

        }
        else if(currHealth > 0.25*totalHealth && currHealth <= 0.5*totalHealth){
            gameObject.GetComponent<SpriteRenderer>().sprite = healthDisplaySprites[2];

        }
        else if(currHealth > 0 && currHealth <= 0.25*totalHealth){
            gameObject.GetComponent<SpriteRenderer>().sprite = healthDisplaySprites[1];

        }
        else{
            gameObject.GetComponent<SpriteRenderer>().sprite = healthDisplaySprites[0];

        }

    }

    public void deadDisplay(){
        gameObject.GetComponent<SpriteRenderer>().sprite = healthDisplaySprites[0];
    }

}
