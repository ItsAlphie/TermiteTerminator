using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{   

    private int level = 1;

    private int environment = 0; //To do: change this to an enum

    [SerializeField] private Transform[] Path;
    //tba -> maps and paths for each level

    int GetLevel(){
        return level;
    }

    public int GetEnvironment(){
        return environment;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
