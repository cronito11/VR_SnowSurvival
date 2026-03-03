using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Survival/Temperature Runtime State", fileName = "TemperatureState")]
public class TemperatureRuntimeState : ScriptableObject
{
    [Header("Runtime values (read-only in design, written at runtime)")]
    [SerializeField] private float currentBodyTempC = 37f;
    [SerializeField] private bool isStabilized = false;
    [SerializeField] private bool isStormActive = false;

    public Action<float> OnBodyTempChanged; // optional event for UI/dev hooks, fires when body temp changes

    [Header("Optional: reset behavior")]
    public bool resetOnEnable = true;
    public float startBodyTempC = 37f;

    public float CurrentBodyTempC => currentBodyTempC;
    public bool IsStabilized => isStabilized;
    public bool IsStormActive => isStormActive;

    private void OnEnable()
    {
        if (resetOnEnable)
        {
            currentBodyTempC = startBodyTempC;
            isStabilized = false;
            isStormActive = false;
        }
    }

    public void SetBodyTempC(float tempC, SurvivalTemperatureConfig config)
    {
        if (config != null)
            tempC = config.ClampBodyTemp(tempC);

        currentBodyTempC = tempC;

        OnBodyTempChanged?.Invoke(GetBodyTemp01(config));
    }

    public void SetStabilized(bool value) => isStabilized = value;
    public void SetStormActive(bool value) => isStormActive = value;

    // UI helper (0..1)
    public float GetBodyTemp01(SurvivalTemperatureConfig config)
    {
        if (config == null) return 0f;
        float range = Mathf.Max(0.0001f, config.maxBodyTempC - config.minBodyTempC);
        return Mathf.Clamp01((currentBodyTempC - config.minBodyTempC) / range);
    }
}
