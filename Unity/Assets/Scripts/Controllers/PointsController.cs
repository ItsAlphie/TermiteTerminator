using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    private int playerScore;

    // Reference to the HighscoreTable
    private Highscoretable highscoreTable;

    void Start()
    {
        // Initialize the score to zero at the start of the game
        Debug.Log("Started the pointercontroller");
        playerScore = 9000;
        

        // Find the HighscoreTable instance (assuming it's attached to a GameObject tagged "HighscoreTable")
        highscoreTable = GameObject.FindGameObjectWithTag("HighscoreTable")?.GetComponent<Highscoretable>();
        Debug.Log("going to end the game");
        EndOfGame();
    }

    public void AddPoints(int enemyHealth)
    {
        playerScore += enemyHealth;
        Debug.Log("Points Added! Current Score: " + playerScore);
    }

    public void SubtractPointsForTower()
    {
        playerScore -= 20;
        Debug.Log("Points Subtracted for Tower Destruction! Current Score: " + playerScore);
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void EndOfGame()
    {
        // Ensure the highscoreTable reference is valid
        Debug.Log("Ending the game");
        highscoreTable.AddHighscoreEntry(playerScore, "BBB");
        Debug.Log("Added to Highscore Table: " + playerScore);

        
        
    }
}
