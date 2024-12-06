using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 5f;
    public float maxDistance = 30f;
    private int damage = 40;
    private Vector2 startPosition;
    //[SerializeField] private AudioClip shootSoundclip;
   
    
     
    void Start()
    {
        direction = new Vector2(1, 0);
        startPosition = transform.position;
        //SoundController.instance.PlaySoundFXClip(shootSoundclip, transform, 1f);
    }
    
    

    void Update()
    {
        direction = updateVector(direction.x);
        transform.Translate(direction * Time.deltaTime);

        float distanceTraveled = Vector2.Distance(startPosition,transform.position);
        Debug.Log(distanceTraveled);
        Debug.Log("maxDistance"+ maxDistance);
        if(distanceTraveled >=maxDistance){
            Destroy(gameObject);
        } 
    }
    
    void OnTriggerEnter2D(Collider2D collision){
        GameObject collidedObject = collision.gameObject;
        HealthController healthController = collidedObject.GetComponent<HealthController>();

        if (collidedObject.tag == "Enemy")
        {   
            if (collidedObject.GetComponent<BasicEnemy>() != null){
                if(collidedObject.GetComponent<BasicEnemy>().type == 1){
                    damage *= 2;
                }
                healthController.takeDamage(damage);
            }
        }
        else{
            if(collidedObject != null){
                healthController.takeDamage(damage);
            }
        }
        //Destroy(gameObject);
      }
    
    private Vector2 updateVector(float xvalue)
    {
        direction.x = xvalue * 1.015f;
        return direction;
    }
    
    
   
}
