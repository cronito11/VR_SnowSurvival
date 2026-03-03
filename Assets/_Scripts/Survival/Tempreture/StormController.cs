using System.Collections;
using UnityEngine;

public class StormController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private TemperatureRuntimeState state;

    [Header("Random timing (seconds)")]
    [Tooltip("Wait time range before a storm starts (when no storm is active).")]
    [SerializeField] private Vector2 timeBetweenStormsRange = new Vector2(20f, 60f);

    [Tooltip("Storm duration range once it starts.")]
    [SerializeField] private Vector2 stormDurationRange = new Vector2(10f, 25f);

    [Header("VFX")]
    [Tooltip("Your snowfall ParticleSystem (can be in scene, disabled initially).")]
    [SerializeField] private ParticleSystem snowParticles;

    [Tooltip("If true, stop will clear remaining particles immediately.")]
    [SerializeField] private bool clearOnStop = true;

    [Header("Debug")]
    [SerializeField] private bool autoStartOnEnable = true;
    [SerializeField] private bool logStorms;

    private Coroutine loop;

    private void OnEnable()
    {
        if (autoStartOnEnable)
            loop = StartCoroutine(StormLoop());
    }

    private void OnDisable()
    {
        if (loop != null)
            StopCoroutine(loop);

        SetStorm(false);
    }

    private IEnumerator StormLoop()
    {
        SetStorm(false);

        while (true)
        {
            float wait = Random.Range(timeBetweenStormsRange.x, timeBetweenStormsRange.y);
            yield return new WaitForSeconds(wait);

            SetStorm(true);

            float duration = Random.Range(stormDurationRange.x, stormDurationRange.y);
            yield return new WaitForSeconds(duration);

            SetStorm(false);
        }
    }

    private void SetStorm(bool active)
    {
        if (state != null)
            state.SetStormActive(active);

        if (snowParticles != null)
        {
            if (active)
            {
                snowParticles.Play(true);
            }
            else
            {
                var behavior = clearOnStop
                    ? ParticleSystemStopBehavior.StopEmittingAndClear
                    : ParticleSystemStopBehavior.StopEmitting;

                snowParticles.Stop(true, behavior);
            }
        }

        if (logStorms)
            Debug.Log(active ? "Storm started" : "Storm ended");
    }

    // Optional: allow other systems (or cheats) to force a storm
    public void ForceStormOn() => SetStorm(true);
    public void ForceStormOff() => SetStorm(false);
}
