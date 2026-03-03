using System.Collections.Generic;
using UnityEngine;

public class TemperatureZoneReceiver : MonoBehaviour
{
    [Header("State to affect")]
    [SerializeField] private TemperatureRuntimeState state;

    private readonly HashSet<TemperatureZoneVolume> activeZones = new HashSet<TemperatureZoneVolume>();

    private void OnTriggerEnter(Collider other)
    {
        var zone = other.GetComponent<TemperatureZoneVolume>();
        if (zone == null) return;
        if (!zone.stabilizesTemperature) return;

        activeZones.Add(zone);
        UpdateState();
    }

    private void OnTriggerExit(Collider other)
    {
        var zone = other.GetComponent<TemperatureZoneVolume>();
        if (zone == null) return;

        activeZones.Remove(zone);
        UpdateState();
    }

    private void OnDisable()
    {
        activeZones.Clear();
        UpdateState();
    }

    private void UpdateState()
    {
        if (state == null) return;

        // If later you add “warmth strength”, this is where you’d compute it.
        state.SetStabilized(activeZones.Count > 0);
    }
}
