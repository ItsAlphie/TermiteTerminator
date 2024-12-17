using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] HighscoreTable highscoreTable;
    private int score;
    private void Start(){
        //pointsController = GameObject.FindGameObjectWithTag("PointsController")?.GetComponent<PointsController>();
        score = 0;
    }
    public void regulateInput()
    {
        score = PointsController.globalPointsController.GetScore(); 
        Debug.Log("Button Pressed");
        string input = inputField.text;
        if (input.Length > 3)
        {
            Debug.LogError("Name too long");
        }
        else
        {
            highscoreTable.AddHighscoreEntry(score, input);
            Debug.Log("Highscore should be submitted");
        }
    }
}
