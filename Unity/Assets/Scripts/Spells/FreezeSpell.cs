using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSpell : BasicSpell
{
    
    [SerializeField] private float newSpeed = 0.01f;

    override protected void spell_effect()
    {
    
    }

    override protected void finish_effect()
    {
    
    }

    public float GetNewSpeed()
    {
        return newSpeed;
    }
    
}
