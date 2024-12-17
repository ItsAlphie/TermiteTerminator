using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] HighscoreTable highscoreTable;

    private int score = 3000; 

    public void regulateInput()
    {
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
