using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CollectableItem : MonoBehaviour
{
    [Header("Item Type")]
    [Tooltip("Drag the ItemDefinition asset for this item type.")]
    public ItemDefinition itemDefinition;
    
    private bool _hasBeenDelivered = false;

    private void Start()
    {
        // Checking for null references and logging warnings
        if (itemDefinition == null)
            Debug.LogWarning($"[CollectableItem] '{gameObject.name}' has no ItemDefinition assigned!", this);
    }
    
    public  void DeliverToZone(string zoneID)
    {
        if (_hasBeenDelivered) return; 
        if (itemDefinition == null) return;

        _hasBeenDelivered = true;

        Debug.Log($"[CollectableItem] '{itemDefinition.itemID}' delivered to '{zoneID}'");
        GameEvents.ItemDeliveredToZone(itemDefinition.itemID, zoneID);

        Destroy(gameObject);
    }
}
