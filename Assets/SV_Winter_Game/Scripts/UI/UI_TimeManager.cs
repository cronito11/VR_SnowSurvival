using UnityEngine;

public class UI_TimeManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI timer;
    [SerializeField] private GameplayManager gameplayManager;

    private void Start()
    {
        if (gameplayManager == null)
        {
            gameplayManager = FindObjectOfType<GameplayManager>();
        }

        if (gameplayManager == null)
        {
            Debug.LogWarning("GameplayManager not found. UI_TimeManager will use Time.timeSinceLevelLoad instead.");
        }
    }

    private void Update()
    {
        if (timer != null)
        {
            float time = gameplayManager != null ? gameplayManager.CurrentGameTime : Time.timeSinceLevelLoad;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timer.text = $"{minutes:00}:{seconds:00}";
        }
    }
}