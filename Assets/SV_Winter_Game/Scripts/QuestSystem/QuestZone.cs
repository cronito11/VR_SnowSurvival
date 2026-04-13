using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestZone : MonoBehaviour
{
    [Header("Zone Identity")]
    public string zoneID = "Zone_1";

    private BoxCollider _collider;
    private CollectableAgent_LO agent;

    private List<CollectableItem> collectable = new List<CollectableItem>();

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        agent  = GetComponentInChildren<CollectableAgent_LO>();
        _collider.isTrigger = true;
    }

    public CollectableItem Collect(CollectableItem item)
    {

        bool isAccepted = QuestManager.Instance.TryDeliverItem(item.itemDefinition.itemID, zoneID);

        if (isAccepted)
        {
            collectable.Remove(item);
            // The item was accepted! Destroy it.
            Destroy(item.gameObject);
        }
        else
        {
            // Wrong item, wrong zone, or no active quest. 
            // The item just falls on the floor.
            Debug.Log($"[QuestZone] {item.itemDefinition.itemID} was rejected by {zoneID}.");
        }

        if(collectable.Count == 0)
            return null;
        

        return collectable[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectableItem>();
        if (item == null || item.itemDefinition == null) return;

        if (!QuestManager.Instance.IsItemDeliverable(item.itemDefinition.itemID, zoneID))
        {
            Debug.Log($"[QuestZone] {item.itemDefinition.itemID} was rejected by {zoneID}.");
            return;
        }

        collectable.Add(item);
        agent.ChangeTarget(item.transform);

        /*
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
        */
    }
}