using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TemperatureZoneVolume : MonoBehaviour
{
    [Header("State to affect")]
    [SerializeField] private TemperatureRuntimeState state;

    [Header("Filter")]
    [SerializeField] private string playerTag = "Player";

    [Header("Zone behavior")]
    [Tooltip("If true, this zone counts as 'stabilized' (indoors/fire pit safe area).")]
    public bool stabilizesTemperature = true;

    private readonly HashSet<Collider> inside = new HashSet<Collider>();

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stabilizesTemperature) return;
        if (!other.CompareTag(playerTag)) return; // CompareTag is the recommended tag check API. [web:102]

        inside.Add(other);
        UpdateState();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!stabilizesTemperature) return;
        if (!other.CompareTag(playerTag)) return; // [web:102]

        inside.Remove(other);
        UpdateState();
    }

    private void OnDisable()
    {
        inside.Clear();
        UpdateState();
    }

    private void UpdateState()
    {
        if (state == null) return;
        state.SetStabilized(inside.Count > 0);
    }
}
