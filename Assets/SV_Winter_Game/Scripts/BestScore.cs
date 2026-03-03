using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class BestScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private bool showTop5Only = true;

    private const string BEST_SCORES_KEY = "BestScores";

    void Start()
    {
        DisplayBestScores();
    }

    public void DisplayBestScores()
    {
        if (bestScoreText == null) return;

        List<float> bestScores = LoadBestScores();
        
        if (bestScores.Count == 0)
        {
            bestScoreText.text = "No scores yet!";
            return;
        }

        int displayCount = showTop5Only ? Mathf.Min(5, bestScores.Count) : bestScores.Count;
        string scoreDisplay = "\n";

        for (int i = 0; i < displayCount; i++)
        {
            scoreDisplay += $"{i + 1}. {FormatTime(bestScores[i])}\n";
        }

        bestScoreText.text = scoreDisplay;
    }

    private List<float> LoadBestScores()
    {
        List<float> scores = new List<float>();
        string savedData = PlayerPrefs.GetString(BEST_SCORES_KEY, "");

        if (!string.IsNullOrEmpty(savedData))
        {
            string[] scoreStrings = savedData.Split(',');
            foreach (string scoreStr in scoreStrings)
            {
                if (float.TryParse(scoreStr, out float score))
                {
                    scores.Add(score);
                }
            }
        }

        return scores;
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void ClearScores()
    {
        PlayerPrefs.DeleteKey(BEST_SCORES_KEY);
        PlayerPrefs.Save();
        DisplayBestScores();
    }
}
