using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("The type of item this spawner creates.")]
    public ItemDefinition itemType;
    
    [Tooltip("The actual physical prefab to spawn (e.g., your Apple or Wood model).")]
    public GameObject itemPrefab;

    // We keep track of the current spawned item so we don't accidentally spawn duplicates
    private GameObject _currentSpawnedItem;

    private void OnEnable()
    {
        // Listen to the Game Events
        GameEvents.OnQuestActivated += HandleQuestStarted;
        GameEvents.OnQuestProgressUpdated += HandleItemDelivered;
        
        // Clean up leftovers when a quest ends or fails
        GameEvents.OnQuestCompleted += CleanUpLeftovers;
        GameEvents.OnQuestFailed += CleanUpLeftovers;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestActivated -= HandleQuestStarted;
        GameEvents.OnQuestProgressUpdated -= HandleItemDelivered;
        GameEvents.OnQuestCompleted -= CleanUpLeftovers;
        GameEvents.OnQuestFailed -= CleanUpLeftovers;
    }

    private void HandleQuestStarted(QuestState quest)
    {
        // If the new quest needs THIS spawner's item type, spawn the very first one!
        if (quest.sourceQuest.requiredItem == itemType)
        {
            SpawnItem();
        }
    }

    private void HandleItemDelivered(QuestState quest)
    {
        // If the delivered item was for THIS spawner's type...
        if (quest.sourceQuest.requiredItem == itemType)
        {
            // Check if we still need more to complete the quest
            if (quest.currentAmount < quest.sourceQuest.requiredCount)
            {
                SpawnItem(); // Respawn!
            }
        }
    }

    private void CleanUpLeftovers(QuestState quest)
    {
        // If the quest is over (win or lose) and the item is still sitting there, destroy it
        if (quest.sourceQuest.requiredItem == itemType && _currentSpawnedItem != null)
        {
            Destroy(_currentSpawnedItem);
        }
    }

    private void SpawnItem()
    {
        // Safety check: if there is already an item here, destroy it before making a new one
        if (_currentSpawnedItem != null)
        {
            Destroy(_currentSpawnedItem);
        }

        // Spawn the prefab exactly where this Spawner GameObject is located
        _currentSpawnedItem = Instantiate(itemPrefab, transform.position, transform.rotation);
        
        Debug.Log($"[ItemSpawner] Spawned a new {itemType.displayName} at {gameObject.name}");
    }
}