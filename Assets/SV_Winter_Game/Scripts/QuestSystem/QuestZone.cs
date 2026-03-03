using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestZone : MonoBehaviour
{
    [Header("Zone Identity")]
    [Tooltip("Unique ID. Must match 'targetZoneID'.")]
    public string zoneID = "Zone_1";

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
        gameObject.name = $"QuestZone_{zoneID}";
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectableItem>();
        if (item == null) return;

        item.DeliverToZone(zoneID);
    }
}