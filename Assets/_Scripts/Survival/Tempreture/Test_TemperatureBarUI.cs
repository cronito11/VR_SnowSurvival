using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private SurvivalTemperatureConfig config;
    [SerializeField] private TemperatureRuntimeState state;

    [Header("UI")]
    [SerializeField] private Image fillImage;          // Image Type = Filled
    [SerializeField] private TMP_Text tempText;        // optional

    private void Update()
    {
        if (config == null || state == null) return;

        float t01 = state.GetBodyTemp01(config);

        if (fillImage != null)
            fillImage.fillAmount = t01; // 0..1 [web:144]

        if (tempText != null)
            tempText.text = $"{state.CurrentBodyTempC:0.0}°C";
    }


    // in case of slider
    /*[SerializeField] private SurvivalTemperatureConfig config;
    [SerializeField] private TemperatureRuntimeState state;

    [SerializeField] private Slider slider;     // set min=0 max=1
    [SerializeField] private TMP_Text tempText; // optional

    private void Update()
    {
        if (config == null || state == null) return;

        float t01 = state.GetBodyTemp01(config);

        if (slider != null)
            slider.value = t01;

        if (tempText != null)
            tempText.text = $"{state.CurrentBodyTempC:0.0}°C";
    }*/
}
