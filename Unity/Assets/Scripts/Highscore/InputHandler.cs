using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InputHandler : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] HighscoreTable highscoreTable;
    private int score;
    private bool submitted = false;
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
            if(!submitted){
                highscoreTable.AddHighscoreEntry(score, input);
                submitted = true;}
        }
    }
    public void startNewGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
