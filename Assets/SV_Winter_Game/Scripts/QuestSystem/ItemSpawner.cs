using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("The type of item this spawner creates.")]
    public ItemDefinition itemType;

    [Tooltip("The actual physical prefab to spawn (e.g., your GoldBar model).")]
    public GameObject itemPrefab;

    private GameObject _currentSpawnedItem;

    private void OnEnable()
    {
        GameEvents.OnQuestActivated       += HandleQuestStarted;
        GameEvents.OnQuestProgressUpdated += HandleItemDelivered;
        GameEvents.OnQuestCompleted       += HandleQuestCompleted;
        GameEvents.OnQuestFailed          += HandleQuestFailed;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestActivated       -= HandleQuestStarted;
        GameEvents.OnQuestProgressUpdated -= HandleItemDelivered;
        GameEvents.OnQuestCompleted       -= HandleQuestCompleted;
        GameEvents.OnQuestFailed          -= HandleQuestFailed;
    }

    private void HandleQuestStarted(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;

        SpawnItem();
    }

    private void HandleItemDelivered(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;

        // Respawn every time an item is delivered, no limit
        SpawnItem();
    }

    private void HandleQuestCompleted(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;

        // Destroy any leftover item still sitting in the world
        if (_currentSpawnedItem != null)
            Destroy(_currentSpawnedItem);
    }

    private void HandleQuestFailed(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;

        // Destroy leftover item but keep the spawner alive
        // The quest goes back into the pool to retry, so we'll need to spawn again
        if (_currentSpawnedItem != null)
            Destroy(_currentSpawnedItem);
    }

    private void SpawnItem()
    {
        if (itemPrefab == null)
        {
            Debug.LogError($"[ItemSpawner] itemPrefab is not assigned on '{gameObject.name}'!");
            return;
        }

        // Destroy existing item before spawning a new one
        if (_currentSpawnedItem != null)
            Destroy(_currentSpawnedItem);

        _currentSpawnedItem = Instantiate(itemPrefab, transform.position, transform.rotation);
        Debug.Log($"[ItemSpawner] Spawned '{itemType.displayName}' at '{gameObject.name}'");
    }
}