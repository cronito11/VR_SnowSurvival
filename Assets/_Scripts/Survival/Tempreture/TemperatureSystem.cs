using UnityEngine;

public class TemperatureSystem : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SurvivalTemperatureConfig config;
    [SerializeField] private TemperatureRuntimeState state;

    [Header("Optional: debug")]
    [SerializeField] private bool logTempChanges;

    private float tickTimer;

    private void Reset()
    {
        tickTimer = 0f;
    }

    private void Update()
    {
        if (config == null || state == null) return;

        tickTimer += Time.deltaTime;
        if (tickTimer < config.tickIntervalSec) return;

        float dt = tickTimer; // consume accumulated time to be robust under frame spikes
        tickTimer = 0f;

        StepTemperature(dt);
    }

    private void StepTemperature(float dt)
    {
        float tempC = state.CurrentBodyTempC;

        bool stabilized = state.IsStabilized;
        bool storm = state.IsStormActive;

        float coolingPerSec = config.GetCoolingPerSec(storm, stabilized);
        float warmUpPerSec = config.GetWarmUpPerSec(stabilized);

        // Cooling: move away from normal downwards when exposed
        if (!stabilized && coolingPerSec > 0f)
            tempC -= coolingPerSec * dt;

        // Warming: move back up toward normal when stabilized
        if (stabilized && warmUpPerSec > 0f)
            tempC = Mathf.MoveTowards(tempC, config.normalBodyTempC, warmUpPerSec * dt);

        tempC = config.ClampBodyTemp(tempC);

        if (logTempChanges)
            Debug.Log($"BodyTempC: {tempC:0.00} (01={state.GetBodyTemp01(config):0.00})");

        state.SetBodyTempC(tempC, config);
    }

    // Convenience for UI or other systems:
    public float GetTempC() => state != null ? state.CurrentBodyTempC : 0f;
    public float GetTemp01() => (config != null && state != null) ? state.GetBodyTemp01(config) : 0f;
}
