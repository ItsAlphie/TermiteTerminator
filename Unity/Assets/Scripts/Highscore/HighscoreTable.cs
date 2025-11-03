using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    [SerializeField] private Transform highscoreContainer;
    [SerializeField] private TMP_Text yourScoreText;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList = new List<Transform>();
    private void Start()
    {
        //ClearAllPlayerPrefs();
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        //highscoreContainer = transform.Find("highscoreEntryContainer");
        entryTemplate.gameObject.SetActive(false);
        highscoreContainer.gameObject.SetActive(false);
        // Load and display highscores
        LoadScores();
    }

    private void LoadScores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable", "");
        Highscores loadHighscores = JsonUtility.FromJson<Highscores>(jsonString) ?? new Highscores { highscoreEntryList = new List<HighscoreEntry>() };
        highscoreEntryList = loadHighscores.highscoreEntryList;

        // Sort and trim the highscore list to top 5
        highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));
        if (highscoreEntryList.Count > 5)
        {
            highscoreEntryList.RemoveRange(5, highscoreEntryList.Count - 5);
        }

        // Clear existing entries before adding new ones
        ClearExistingEntries();

        // Create new entries from the sorted list
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

        // Optionally save the updated highscore list back to PlayerPrefs
        string updatedJson = JsonUtility.ToJson(new Highscores { highscoreEntryList = highscoreEntryList });
        PlayerPrefs.SetString("highscoreTable", updatedJson);
        PlayerPrefs.Save();
    }

    public void showScore()
    {
        yourScoreText.text = "Your score: " + PointsController.globalPointsController.GetScore().ToString();
    }

    private void ClearExistingEntries()
    {
        // Destroy all previously instantiated entries
        foreach (Transform entry in highscoreEntryTransformList)
        {
            Destroy(entry.gameObject);
        }
        
        // Clear the list of transforms
        highscoreEntryTransformList.Clear();
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 122f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        entryTransform.gameObject.SetActive(true); // Make sure the entry is visible

        // Adjust the position of the entry
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * (transformList.Count + 1));

        // Set the text for the position, score, and name
        TMP_Text posText = entryTransform.Find("posText").GetComponent<TMP_Text>();
        TMP_Text scoreText = entryTransform.Find("scoreText").GetComponent<TMP_Text>();
        TMP_Text nameText = entryTransform.Find("nameText").GetComponent<TMP_Text>();

        int rank = transformList.Count + 1;

        string rankString = rank + "TH";
        switch (rank)
        {
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        posText.text = rankString;
        scoreText.text = highscoreEntry.score.ToString();
        nameText.text = highscoreEntry.name;

        // Add the created entry to the list of transforms
        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, string name)
    {
        // Create new HighscoreEntry
        HighscoreEntry newEntry = new HighscoreEntry { score = score, name = name };

        // Load existing highscores and add the new entry
        string jsonString = PlayerPrefs.GetString("highscoreTable", "");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString) ?? new Highscores { highscoreEntryList = new List<HighscoreEntry>() };

        highscores.highscoreEntryList.Add(newEntry);

        // Sort the highscores and keep top 5
        highscores.highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));
        if (highscores.highscoreEntryList.Count > 5)
        {
            highscores.highscoreEntryList.RemoveRange(5, highscores.highscoreEntryList.Count - 5);
        }

        // Save updated highscores
        string updatedJson = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", updatedJson);
        PlayerPrefs.Save();

        // Reload and display the updated highscore table
        LoadScores();
    }

    public void ClearAllPlayerPrefs()
    {
        // Delete all PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs entries have been cleared.");
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
