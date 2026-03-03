using UnityEngine;

public class UI_TimeManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI timer;

    private void Update()
    {
        // Update the timer text every frame
        if (timer != null)
        {
            float time = Time.timeSinceLevelLoad;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timer.text = $"{minutes:00}:{seconds:00}";
        }
    }
}