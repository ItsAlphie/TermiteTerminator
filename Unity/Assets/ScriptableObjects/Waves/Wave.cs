using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code heavily inspired by https://hal-brooks.medium.com/enemy-waves-with-scriptable-objects-6dd1685437f5 

[CreateAssetMenu(fileName ="Wave.asset", menuName ="ScriptableObjects/Wave")]
public class Wave : ScriptableObject 
{
    public List<GameObject> waveSequence;
    public float timeBetweenSpawn;
}