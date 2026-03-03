using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CollectableItem : MonoBehaviour
{
    [Header("Item Type")]
    [Tooltip("Drag the ItemDefinition asset for this item type.")]
    public ItemDefinition itemDefinition;

    private void Start()
    {
        // Checking for null references and logging warnings
        if (itemDefinition == null)
            Debug.LogWarning($"[CollectableItem] '{gameObject.name}' has no ItemDefinition assigned!", this);
    }
}
