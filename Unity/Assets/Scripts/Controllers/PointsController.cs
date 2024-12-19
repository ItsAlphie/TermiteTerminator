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
        //playerScore = 0;
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
        // Find the HighscoreTable instance (assuming it's attached to a GameObject tagged "HighscoreTable")
        highscoreTable = GameObject.FindGameObjectWithTag("HighscoreTable")?.GetComponent<HighscoreTable>();
        //Debug.Log("going to end the game");
        //EndOfGame();
    }

    public void AddPoints(int enemyHealth)
    {
        playerScore += enemyHealth;
        //Debug.Log("Points Added! Current Score: " + playerScore);
    }

    public void SubtractPointsForTower()
    {
        playerScore -= 20;
        //Debug.Log("Points Subtracted for Tower Destruction! Current Score: " + playerScore);
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void EndOfGame()
    {
        // Ensure the highscoreTable reference is valid
        //Debug.Log("Ending the game");
        highscoreTable.AddHighscoreEntry(playerScore,"bbb");
        //Debug.Log("Added to Highscore Table: " + playerScore);

        
        
    }
}
