using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestZone : MonoBehaviour
{
    [Header("Zone Identity")]
    public string zoneID = "Zone_1";

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectableItem>();
        if (item == null || item.itemDefinition == null) return;

        // Ask the Manager: Is this the correct item for the CURRENT task in THIS zone?
        bool isAccepted = QuestManager.Instance.TryDeliverItem(item.itemDefinition.itemID, zoneID);

        if (isAccepted)
        {
            // The item was accepted! Destroy it.
            Destroy(other.gameObject);
        }
        else
        {
            // Wrong item, wrong zone, or no active quest. 
            // The item just falls on the floor.
            Debug.Log($"[QuestZone] {item.itemDefinition.itemID} was rejected by {zoneID}.");
        }
    }
}