using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{

    private bool activated = false;

    private float t0;

    private Animator animator;
    [SerializeField] private int damage = 10;

    private int bombCountdown = 3;
    public bool Activated { get => activated; set => activated = value; }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(Activated){
            if((Time.time - t0) > bombCountdown){
            Explode();
            }
        }
    }


    public void initializeExplosion(){
        if(!activated){
            t0 = Time.time;
            activated = true;
            animator.SetTrigger("isActive"); 
        }
    }

    public void Explode(){
        Activated = false;
        //damage all towers in range
        List<GameObject> towers = TowerSpawner.Instance.towers;
        foreach (GameObject t in towers)
        {
            if(t == null) continue;

            float distance = Vector2.Distance(transform.position, t.transform.position);
            if (distance < 3)
            {
                t.GetComponent<TowerHealthController>().takeDamage(damage);
            }
        }
    }
}
