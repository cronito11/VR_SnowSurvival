using UnityEngine;

[CreateAssetMenu(menuName = "Survival/Temperature Config", fileName = "TemperatureConfig")]
public class SurvivalTemperatureConfig : ScriptableObject
{
    [Header("Body temperature (°C)")]
    [Tooltip("Player's normal/stable body temperature.")]
    public float normalBodyTempC = 37f;

    [Tooltip("Clamp min to avoid going to unrealistic numbers.")]
    public float minBodyTempC = 30f;

    [Tooltip("Clamp max to avoid going to unrealistic numbers.")]
    public float maxBodyTempC = 42f;

    [Header("Cooling / Warming rates (°C per second)")]
    [Tooltip("How fast body temp drops when exposed (no storm, no stabilization).")]
    public float baseCoolingPerSec = 0.05f;

    [Tooltip("How fast body temp rises back toward normal when stabilized (fire pit/indoors).")]
    public float stabilizedWarmUpPerSec = 0.25f;

    [Tooltip("Extra cooling added while storm is active.")]
    public float stormExtraCoolingPerSec = 0.20f;

    [Header("Cold damage")]
    [Tooltip("At or above this temperature, player takes no cold damage.")]
    public float coldDamageStartsBelowC = 35f;

    [Tooltip("If true: use a curve to compute HP/sec drain from temperature.")]
    public bool useHealthDrainCurve = true;

    [Tooltip("X axis: body temp (°C). Y axis: health drain (HP/sec). Only used below coldDamageStartsBelowC.")]
    public AnimationCurve healthDrainPerSecByTempC = new AnimationCurve(
        new Keyframe(35f, 0f),
        new Keyframe(34f, 0.5f),
        new Keyframe(33f, 1.25f),
        new Keyframe(31f, 3.0f),
        new Keyframe(30f, 5.0f)
    );

    [Tooltip("If not using curve, use this steady drain once below coldDamageStartsBelowC.")]
    public float steadyHealthDrainPerSec = 1.0f;

    [Tooltip("Storm multiplies whatever health drain is calculated.")]
    public float stormHealthDrainMultiplier = 2.0f;

    [Tooltip("If true, stabilized zones prevent cold damage even if temp is low.")]
    public bool stabilizedZonesPreventColdDamage = true;

    [Header("Update")]
    [Tooltip("How often the system updates temperature & applies damage (seconds).")]
    [Min(0.02f)]
    public float tickIntervalSec = 0.2f;

    public float ClampBodyTemp(float tempC)
    {
        return Mathf.Clamp(tempC, minBodyTempC, maxBodyTempC);
    }

    public float GetCoolingPerSec(bool isStormActive, bool isStabilized)
    {
        if (isStabilized) return 0f;
        return baseCoolingPerSec + (isStormActive ? stormExtraCoolingPerSec : 0f);
    }

    public float GetWarmUpPerSec(bool isStabilized)
    {
        return isStabilized ? stabilizedWarmUpPerSec : 0f;
    }

    public float GetHealthDrainPerSec(float bodyTempC, bool isStormActive, bool isStabilized)
    {
        if (stabilizedZonesPreventColdDamage && isStabilized)
            return 0f;

        if (bodyTempC >= coldDamageStartsBelowC)
            return 0f;

        float drain = useHealthDrainCurve
            ? Mathf.Max(0f, healthDrainPerSecByTempC.Evaluate(bodyTempC))
            : Mathf.Max(0f, steadyHealthDrainPerSec);

        if (isStormActive)
            drain *= stormHealthDrainMultiplier;

        return drain;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (minBodyTempC > maxBodyTempC)
            maxBodyTempC = minBodyTempC;

        normalBodyTempC = Mathf.Clamp(normalBodyTempC, minBodyTempC, maxBodyTempC);
        coldDamageStartsBelowC = Mathf.Clamp(coldDamageStartsBelowC, minBodyTempC, maxBodyTempC);
    }
#endif
}
