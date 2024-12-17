using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    

    private void Start()
    {
                
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);
        

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable", "");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString) ?? new Highscores { highscoreEntryList = new List<HighscoreEntry>() };
        highscoreEntryList = highscores.highscoreEntryList;

        // Sort the highscore list
        highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep only the top 5 entries
        if (highscoreEntryList.Count > 5)
        {
            highscoreEntryList.RemoveRange(5, highscoreEntryList.Count - 5);
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

        SaveHighscores(new Highscores { highscoreEntryList = highscoreEntryList });
    }
    private void loadScores()
    {
        
        string jsonString = PlayerPrefs.GetString("highscoreTable", "");
        Highscores loadHighscores = JsonUtility.FromJson<Highscores>(jsonString) ?? new Highscores { highscoreEntryList = new List<HighscoreEntry>() };
        highscoreEntryList = loadHighscores.highscoreEntryList;

        
        highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));
        if (highscoreEntryList.Count > 5)
            highscoreEntryList.RemoveRange(5, highscoreEntryList.Count - 5);

        
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject); 
        }

        
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
        
        
        string updatedJson = JsonUtility.ToJson(new Highscores { highscoreEntryList = highscoreEntryList });
        PlayerPrefs.SetString("highscoreTable", updatedJson);
        PlayerPrefs.Save();
    }

    

    private void SaveHighscores(Highscores highscores)
    {
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        Debug.Log("Highscores saved: " + json);
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 122f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * (transformList.Count + 1));
        entryTransform.gameObject.SetActive(true);

        TMPro.TMP_Text posText = entryTransform.Find("posText").GetComponent<TMPro.TMP_Text>();
        TMPro.TMP_Text scoreText = entryTransform.Find("scoreText").GetComponent<TMPro.TMP_Text>();
        TMPro.TMP_Text nameText = entryTransform.Find("nameText").GetComponent<TMPro.TMP_Text>();

        int rank = transformList.Count + 1;

        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        posText.text = rankString;
        scoreText.text = highscoreEntry.score.ToString();
        nameText.text = highscoreEntry.name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable", "");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString) ?? new Highscores { highscoreEntryList = new List<HighscoreEntry>() };

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Sort the list
        highscores.highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep only the top 5 entries
        if (highscores.highscoreEntryList.Count > 5)
        {
            highscores.highscoreEntryList.RemoveRange(5, highscores.highscoreEntryList.Count - 5);
        }

        // Save updated Highscores
        SaveHighscores(highscores);
        loadScores();
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
