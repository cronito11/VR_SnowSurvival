using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameplayManager : MonoBehaviour
{
    [Header("References")]
    private GameSceneManager sceneManager;
    [SerializeField] private string menuSceneName = "MainMenu";

    [Header("Best Scores Settings")]
    [SerializeField] private int maxScoresToSave = 5;

    private float gameStartTime;
    private bool isGameOver;

    public float CurrentGameTime => isGameOver ? (gameOverTime - gameStartTime) : (Time.time - gameStartTime);

    private float gameOverTime;

    private const string BEST_SCORES_KEY = "BestScores";

    private void Start()
    {
        sceneManager = GetComponent<GameSceneManager>();
        gameStartTime = Time.time;
        isGameOver = false;

        Health playerHealth = GetComponentInChildren<Health>();
        if (playerHealth != null)
        {
            playerHealth.Died += OnPlayerDied;
        }
        else
        {
            Debug.LogWarning("Player Health component not found. GameplayManager cannot detect player death.");
        }
    }

    private void OnDestroy()
    {
        Health playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponentInChildren<Health>();
        if (playerHealth != null)
        {
            playerHealth.Died -= OnPlayerDied;
        }
    }

    private void OnPlayerDied(Health health)
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverTime = Time.time;

        float survivalTime = CurrentGameTime;
        SaveBestScore(survivalTime);

        Debug.Log($"Player died! Survival time: {FormatTime(survivalTime)}");

        Invoke(nameof(ReturnToMenu), 3f);
    }

    private void SaveBestScore(float time)
    {
        List<float> bestScores = GetBestScores();
        bestScores.Add(time);
        bestScores = bestScores.OrderByDescending(x => x).Take(maxScoresToSave).ToList();

        string scoresData = string.Join(",", bestScores);
        PlayerPrefs.SetString(BEST_SCORES_KEY, scoresData);
        PlayerPrefs.Save();
    }

    public List<float> GetBestScores()
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

    public void ClearBestScores()
    {
        PlayerPrefs.DeleteKey(BEST_SCORES_KEY);
        PlayerPrefs.Save();
    }

    private void ReturnToMenu()
    {
        if (sceneManager != null)
        {
            sceneManager.LoadScene(menuSceneName);
        }
        else
        {
            Debug.LogError("GameSceneManager not found. Cannot return to menu.");
        }
    }

    public string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
