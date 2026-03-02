using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class Requirement
{
    public ItemType type;
    public int targetCount;
    [HideInInspector] public int currentCount;
}

public class CollectingSpot : MonoBehaviour
{
    [Header("Requirements List")]
    public List<Requirement> requirements;

    [Header("Visuals")]
    public GameObject indicator;

    public UnityEvent onTasksComplete;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has a CollectibleItem script
        CollectibleItem item = other.GetComponent<CollectibleItem>();
        
        if (item != null)
        {
            // Find if this item type is in our requirement list
            foreach (var req in requirements)
            {
                if (req.type == item.itemType && req.currentCount < req.targetCount)
                {
                    req.currentCount++;
                    Debug.Log($"{req.type.name}: {req.currentCount}/{req.targetCount}");
                    
                    Destroy(other.gameObject); // Consume item
                    CheckAllRequirements();
                    return; // Item found and used, stop looking
                }
            }
        }
    }

    private void CheckAllRequirements()
    {
        foreach (var req in requirements)
        {
            if (req.currentCount < req.targetCount) return; // Not done yet
        }

        OnCollectionComplete();
    }

    private void OnCollectionComplete()
    {
        if (indicator) indicator.SetActive(false);
        onTasksComplete.Invoke();
        Debug.Log("Collection Complete!");
        // this.enabled = false;
    }
}