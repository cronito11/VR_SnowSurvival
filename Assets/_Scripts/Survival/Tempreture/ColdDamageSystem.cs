using UnityEngine;
using UnityEngine.Events;

public class ColdDamageSystem : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SurvivalTemperatureConfig config;
    [SerializeField] private TemperatureRuntimeState state;

    [Header("Output")]
    public UnityEvent<float> onColdDamage; // amount to subtract from health

    [Header("Optional: debug")]
    [SerializeField] private bool logDrain;

    private float tickTimer;

    private void Update()
    {
        if (config == null || state == null) return;

        tickTimer += Time.deltaTime;
        if (tickTimer < config.tickIntervalSec) return;

        float dt = tickTimer;
        tickTimer = 0f;

        ApplyColdDamage(dt);
    }

    private void ApplyColdDamage(float dt)
    {
        float tempC = state.CurrentBodyTempC;
        bool storm = state.IsStormActive;
        bool stabilized = state.IsStabilized;

        float drainPerSec = config.GetHealthDrainPerSec(tempC, storm, stabilized);
        if (drainPerSec <= 0f) return;

        float damage = drainPerSec * dt;

        if (logDrain)
            Debug.Log($"ColdDamage: {damage:0.00} (rate={drainPerSec:0.00}/s, temp={tempC:0.00}C, storm={storm}, stab={stabilized})");

        onColdDamage?.Invoke(damage);
    }
}
