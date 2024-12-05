using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
public abstract partial class BasicSpell : MonoBehaviour
{

    protected float t0;

    void Start()
    {
        //get time
        t0 = Time.time;
        //spell_effect()
    }

    void Update()
    {   
        //check time
        if (Time.time - t0 > 5)
        {
            
            Destroy(gameObject);
        }
        //if time is up, destroy object
    }

    abstract protected void spell_effect();

    abstract protected void finish_effect();

}