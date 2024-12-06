using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{

    private bool alive = true;

    private float t0;

    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        t0 = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - t0) > 3 && alive){
            Explode();
        }
    }



    void Explode(){
        alive = false;
        animator.SetBool("isDead", true);
        //damage all towers in range
        List<GameObject> towers = TowerSpawner.Instance.towers;
        foreach (GameObject t in towers)
        {
            if(t == null) continue;

            float distance = Vector2.Distance(transform.position, t.transform.position);
            if (distance < 3)
            {
                t.GetComponent<TowerHealthController>().takeDamage(10);
            }
        }
        Destroy(gameObject, 1);    
    }
}
