using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    [SerializeField] public int playerScore;

    
    private HighscoreTable highscoreTable;
    public static PointsController globalPointsController;
    void Awake()
    {
        if (globalPointsController == null)
        {
            globalPointsController = this;  
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    void Start()
    {
        playerScore = 0;
        highscoreTable = GameObject.FindGameObjectWithTag("HighscoreTable")?.GetComponent<HighscoreTable>();
    }

    public void AddPoints(int enemyHealth)
    {
        playerScore += enemyHealth;
    }

    public int GetScore()
    {
        return playerScore;
    }
    
    public void ResetScore()
    {
        playerScore = 0;
    }
}
