using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_TemperatureManager : MonoBehaviour
{
    [SerializeField] private TemperatureRuntimeState temperatureRuntimeState;
    [SerializeField] private Color maxTemperatureColor;
    [SerializeField] private Color maxTemperatureColor_TemperatureBar;
    [SerializeField] private Color minTemperatureColor;
    [SerializeField, Range(0,1)] readonly private float currentTemperature;

    [Header("Visual Reference")]
    [SerializeField] private TMPro.TextMeshProUGUI temperatureText;
    [SerializeField] private Image temperatureBar;
    [SerializeField] private RawImage playerImage;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.OutCubic;

    private float displayedTemperature;

    private void Start()
    {
        if (temperatureRuntimeState != null)
        {
            temperatureRuntimeState.OnBodyTempChanged += OnTemperatureChange;
        }
    }

    private void OnDestroy()
    {
        if (temperatureRuntimeState != null)
        {
            temperatureRuntimeState.OnBodyTempChanged -= OnTemperatureChange;
        }

        // Clean up tweens when the object is destroyed
        DOTween.Kill(this);
    }


    public void OnTemperatureChange(float temperature)
    { 
        
        
        // Kill any existing tweens to prevent conflicts
        DOTween.Kill(this);

        // Animate the temperature value
        DOTween.To(() => displayedTemperature, x => displayedTemperature = x, temperatureRuntimeState.CurrentBodyTempC, animationDuration)
            .SetEase(animationEase)
            .SetTarget(this)
            .OnUpdate(() => {
                // Update the temperature text during animation
                if (temperatureText != null)
                {
                    temperatureText.text = $"{displayedTemperature:F0}°C";
                }
            });

        // Calculate the fill amount for the temperature bar (use normalized value directly)
        float fillAmount = temperature;
        Color targetColor = Color.Lerp(minTemperatureColor, maxTemperatureColor, fillAmount);
        Color targetColorTemperatureBar = Color.Lerp(minTemperatureColor, maxTemperatureColor_TemperatureBar, fillAmount);

        // Animate the temperature bar fill amount
        if (temperatureBar != null)
        {           
            // Animate the temperature bar color
            temperatureBar.DOColor(targetColorTemperatureBar, animationDuration)
                .SetEase(animationEase)
                .SetTarget(this);
        }

        // Animate the player image color based on temperature
        if (playerImage != null)
        {
            playerImage.DOColor(targetColor, animationDuration)
                .SetEase(animationEase)
                .SetTarget(this);
        }
    }
}
